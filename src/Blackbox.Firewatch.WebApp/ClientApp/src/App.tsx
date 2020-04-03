import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import { TransactionsPage } from './components/transactions/TransactionsPage';
import { ImportTransactionsPage } from './components/transactions/ImportTransactionsPage';
import { DashboardPage } from './components/dashboard/DashboardPage';
import { LoginPage } from './components/api-authorization/LoginPage';



export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/login' component={LoginPage} />
        <Route path='/counter' component={Counter} />
        <Route path='/dashboard' component={DashboardPage} />
        <AuthorizeRoute path='/transactions/import' exact component={ImportTransactionsPage} />
        <AuthorizeRoute path='/transactions' exact component={TransactionsPage} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
