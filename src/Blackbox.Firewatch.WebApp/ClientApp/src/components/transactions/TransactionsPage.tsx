import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { RouteComponentProps } from 'react-router';
import { TransactionsClient, ParseCsvModel } from '../../firewatch.service';

interface TransactionsPageProps extends RouteComponentProps<any> {

}

interface TransactionsPageState {
  userId?: string;
}


export class TransactionsPage extends Component<TransactionsPageProps, TransactionsPageState> {

  constructor(props: TransactionsPageProps) {
    super(props);

    this.state = {
        userId: props.match.params['userId'],
    };
    
  }

  componentDidMount() {
      this.populateTransactions();
  }

  render() {
      return (
        <div>
            <h1>Transactions</h1>
            {this.state.userId && <p>For user {this.state.userId}</p>}
        </div>);
  }

  async populateTransactions() {
    const token = await authService.getAccessToken();
    console.log('Fetcing data using token', token);

    const transactionsService = new TransactionsClient();
    transactionsService.parseCsv(new ParseCsvModel({
        bank: "rbc",
        csv: '',
        duplicates: false,
        userId: ''
    }));
        // const response = await fetch('weatherforecast', {
    //     headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    //   });
    //   const data = await response.json();
    //   this.setState({ forecasts: data, loading: false });
  }

}