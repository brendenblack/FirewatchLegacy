import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { TransactionsClient, TransactionModel2, ITransactionModel, AddTransactionModel } from '../../firewatch.service';
import { ImportCsv } from './components/ImportCsv';
import { TransactionsTable } from './components/TransactionsTable';
import { Button, HTMLTable, Checkbox, Tag, Colors } from '@blueprintjs/core';
import { StyleSheet, css } from 'aphrodite';
import { testTransactions } from './parsedresults';
import { Table, Column, ICellRenderer, Cell } from '@blueprintjs/table';
import { theme } from '../theme';

interface ImportTransactionsPageProps extends RouteComponentProps<any> {

}

interface ImportTransactionsPageState {
  transactions: ImportTransactionModel[];
}

export interface ImportTransactionModel extends ITransactionModel {
  shouldImport: boolean;
}

const styles = StyleSheet.create({
  root: {
    display: 'flex',
    position: 'relative',
    
  },
  leftPanel: {
    flexGrow: 1,
    padding: '1em',
    paddingRight: '20em',
  },
  rightPanel: {
    width: '20em',
    padding: '1em',
    position: 'absolute',
    top: 0,
    right: 0,
  },
  container: {
    marginBottom: theme.spacing * 8,
  },
  footer: {
    width: '100%',
    padding: `${theme.spacing}px ${theme.spacing * 2}px`,
    position: 'fixed',
    bottom: 0,
    backgroundColor: theme.BACKGROUND5,
    borderTop: `1px solid ${theme.BACKGROUND1}`,
    display: 'flex',
  },
  footerText: {
    flexGrow: 1,
    verticalAlign: 'middle',
  },
});

export class ImportTransactionsPage extends Component<ImportTransactionsPageProps, ImportTransactionsPageState> {

  constructor(props: ImportTransactionsPageProps) {
    super(props);

    this.state = {
      transactions: testTransactions,
    };

    this.amountFormat = this.amountFormat.bind(this);
    this.amountColor = this.amountColor.bind(this);
    this.handleParseResults = this.handleParseResults.bind(this);
    this.renderTransactionRow = this.renderTransactionRow.bind(this);
    this.handleSave = this.handleSave.bind(this);
  }

  componentDidMount() {

  }


  handleParseResults(transactions: ITransactionModel[]) {
    // TODO
    // const transactions2: TransactionModel2[] = [];
    // for (let tx of transactions) {
    //   const tx2 = new TransactionModel2();
    //   tx2.date = tx.date;
    //   tx2.descriptions = tx.descriptions;
    //   tx2.amount = tx.amount;
    //   tx2.currency = tx.currency;
    //   tx2.accountNumber = tx.accountNumber;
    //   transactions2.push(tx2);
    // }

    const models: ImportTransactionModel[] = [];
    for (let tx of transactions) {
      const model = tx as ImportTransactionModel;
      model.shouldImport = !tx.isLikelyDuplicate;
      models.push(model);
    }

    console.log(models);

    this.setState({
      ...this.state,
      transactions: models,
    });
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

  

  handleCheckbox(model: ImportTransactionModel, event: React.ChangeEvent<HTMLInputElement>) {
    this.setShouldImport(model, event.target.checked);
  }

  handleRowClick(model: ImportTransactionModel, event: React.MouseEvent<HTMLTableRowElement, MouseEvent>) {
    this.setShouldImport(model, !model.shouldImport);
  }

  setShouldImport(model: ImportTransactionModel, shouldImport: boolean) {
    const transactions = [ ...this.state.transactions];
    const index = transactions.findIndex(tx => tx === model);
    if (index >= 0) {
      transactions[index].shouldImport = shouldImport;
    }

    this.setState({
      ...this.state,
      transactions: transactions,
    });
  }

  renderTransactionRow(tx: ImportTransactionModel, key: string): JSX.Element {
    const transactionRow = (
      <tr key={key} className={(tx.shouldImport) ? "text-muted" : ""} onClick={this.handleRowClick.bind(this, tx)}>
        
        <td>{tx.date.toLocaleDateString()}</td>
        <td>{tx.accountNumber?.toString()}</td>
        <td>{tx.descriptions?.join('\n')}</td>
        <td className={this.amountColor(tx.amount)}>{this.amountFormat(tx.amount, tx.currency)}</td>
        <td>{tx.isLikelyDuplicate && <Tag intent="warning">Duplicate</Tag>}</td>
        <td><Checkbox checked={tx.shouldImport} onChange={this.handleCheckbox.bind(this, tx)} style={{ marginBottom: 0 }}/></td>
      </tr>);

      return transactionRow;
  }

  handleSave() {
    const client = new TransactionsClient();
    const models: AddTransactionModel[] = [];
    for (let tx of this.state.transactions.filter(tx => tx.shouldImport)) {
      const model = new AddTransactionModel();
      model.accountNumber = tx.accountNumber;
      model.amount = tx.amount;
      model.currency = tx.currency;
      model.date = tx.date;
      model.descriptions = tx.descriptions;
      models.push(model);
    }
    client.addTransactions(models).then(response => {
      console.log(response);
    });
  }

  render() {
      const transactionRows: JSX.Element[] = [];
      let counter = 0;
      for (let tx of this.state.transactions) {
        transactionRows.push(this.renderTransactionRow(tx, `tx${counter}`));
        counter++;
      }

      

      return (
        <div>
        <main className={css(styles.container)}>        
          <div className={css(styles.root)}>
            <div className={css(styles.leftPanel)}>
              <HTMLTable striped interactive>
                <thead>
                  <tr>
                    <th>Date</th>
                    <th>Account</th>
                    <th>Descriptions</th>
                    <th>Amount</th>
                    <th></th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  {transactionRows}
                </tbody>
              </HTMLTable>
              {/* <Table numRows={20}>
                <Column name="Date" cellRenderer={this.dateCellRenderer} />
              </Table> */}
            </div>

            <div className={css(styles.rightPanel)}>
              <ImportCsv onParseResults={this.handleParseResults} />
            </div>
          </div>
        </main>
        <section className={css(styles.footer)}>
          <span className={css(styles.footerText) + " bp3-text-large"}>Found {this.state.transactions.length} transactions ({this.state.transactions.filter(tx => tx.isLikelyDuplicate).length} duplicates).</span>
            <Button 
              intent="primary"
              large
              disabled={this.state.transactions.length <= 0}
              onClick={this.handleSave}>Save {this.state.transactions.filter(tx => tx.shouldImport).length} transaction(s)</Button>
        </section>
        </div>
        );
  }

}