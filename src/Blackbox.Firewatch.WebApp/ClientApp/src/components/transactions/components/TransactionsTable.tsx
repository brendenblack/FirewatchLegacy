import React, { Component } from 'react';
import { TransactionModel2 } from '../../../firewatch.service';
import { Table } from 'reactstrap';
import CSS from 'csstype';

interface TransactionsTableProps {
    transactions: TransactionModel2[];
}

interface TransactionsTableState {

}

const styles: { [key: string]: CSS.Properties} = {
  'container': {
      overflowY: 'scroll',
  }
}


export class TransactionsTable extends Component<TransactionsTableProps, TransactionsTableState> {

  constructor(props: TransactionsTableProps) {
    super(props);

    this.state = {};

    this.amountFormat = this.amountFormat.bind(this);
  }

  componentDidMount() {
  }

  amountColor(amount: number): string {
    return (amount < 0) ? "text-danger" : "";
  }

  amountFormat(amount: number, currency: string = 'CAD'): string {
      var formatter = new Intl.NumberFormat(undefined, {
        style: 'currency',
        currency: currency
      });

      return formatter.format(amount)
  }


  render() {
    const transactionRows: JSX.Element[] = [];
    let counter = 0;
    for (let tx of this.props.transactions) {
      const transactionRow = (<tr key={`tx${counter}`}>
          
        <td>{tx.date.toLocaleDateString()}</td>
        <td>{tx.accountNumber}</td>
        <td>{tx.descriptions?.join('\n')}</td>
        <td className={this.amountColor(tx.amount)}>{this.amountFormat(tx.amount, tx.currency)}</td>
      </tr>);
      transactionRows.push(transactionRow);
      counter++;
    }

      return (
          <div style={styles['container']}>
            <Table hover>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Account</th>
                        <th>Descriptions</th>
                        <th>Amount</th>
                    </tr>
                </thead>
                <tbody >
                    {transactionRows}
                </tbody>
            </Table>
          </div>
       );
  }
}