import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { TransactionsClient, TransactionModel2, TransactionModel } from '../../firewatch.service';
import { Container, Row, Col, Collapse } from 'reactstrap';
import { TransactionsTable } from './components/TransactionsTable';

interface TransactionsPageProps extends RouteComponentProps<any> {

}

interface TransactionsPageState {
  userId?: string;
  transactions: TransactionModel2[];
  csvContents?: string;
}


export class TransactionsPage extends Component<TransactionsPageProps, TransactionsPageState> {

  constructor(props: TransactionsPageProps) {
    super(props);

    this.state = {
        userId: props.match.params['userId'],
        transactions: [],
    };

    this.filterTransactions = this.filterTransactions.bind(this);
    this.handleParseResults = this.handleParseResults.bind(this);
    
  }

  componentDidMount() {
      this.populateTransactions();
  }

  filterTransactions(): TransactionModel2[] {
    return this.state.transactions;
  }

  handleParseResults(transactions: TransactionModel[]) {
    // TODO
    const transactions2: TransactionModel2[] = [];
    for (let tx of transactions) {
      const tx2 = new TransactionModel2();
      tx2.date = tx.date;
      tx2.descriptions = tx.descriptions;
      tx2.amount = tx.amount;
      tx2.currency = tx.currency;
      tx2.accountNumber = tx.accountNumber;
      transactions2.push(tx2);
    }

    this.setState({
      ...this.state,
      transactions: transactions2,
    });
  }

  render() {
      return (
        <Container fluid={true}>
          <Row>
            <Col md={9}>
              <Collapse isOpen={false}>

              </Collapse>
            </Col>
            <Col md={3}>
            </Col>
          </Row>

          <Row>
            <Col md={9}>
              <TransactionsTable transactions={this.state.transactions} />
            </Col>
            <Col>
             
            </Col>
          </Row>
        </Container>);
  }

  async populateTransactions() {
    const transactionsService = new TransactionsClient();
    transactionsService.fetchTransactions()
      .then(response => {
        console.log('Response', response)
        this.setState({
          ...this.state,
          transactions: response.transactions!,
        });
      }).finally(() => {console.log('finally')});
    // transactionsService.parseCsv(new ParseCsvModel({
    //     bank: "rbc",
    //     csv: '',
    //     duplicates: false,
    //     userId: ''
    // }));
        // const response = await fetch('weatherforecast', {
    //     headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    //   });
    //   const data = await response.json();
    //   this.setState({ forecasts: data, loading: false });
  }

}