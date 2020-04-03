import React, { Component, ComponentProps } from 'react';
import { RouteComponentProps } from 'react-router';
import { AccountsClient, AccountSummaryModel } from '../../firewatch.service';
import { Column, Table } from "@blueprintjs/table";
import { HTMLTable, Button } from '@blueprintjs/core';
import { IconNames } from "@blueprintjs/icons";

interface AccountsWidgetProps extends ComponentProps<any> {

}

interface AccountsWidgetState {
  accounts: AccountSummaryModel[];
}


export class AccountsWidget extends Component<AccountsWidgetProps, AccountsWidgetState> {
  constructor(props: AccountsWidgetProps) {
    super(props);

    this.state = {
      accounts: [],
    };    
  }

  componentDidMount() {
    this.populateDashboard();
  }

  render() {
    const tableBodyContents: JSX.Element[] = [];
    for (let account of this.state.accounts) {
      tableBodyContents.push(
        <tr>
          <td>{account.accountNumber}</td>
          <td>{account.transactions}</td>
        </tr>
      );
    }

      return (
        <div className="root">
          <HTMLTable>
            <thead>
              <tr>
                <th>Account</th>
                <th>Transactions</th>
              </tr>
            </thead>
            <tbody>
              {tableBodyContents}
            </tbody>
          </HTMLTable>
          <Button icon={IconNames.INSERT}>Add account</Button>
        </div>);
  }

  async populateDashboard() {
      const client = new AccountsClient();
      const response = await client.getAccountSummaries();
      console.log('Account summaries response', response);
      this.setState({
        ...this.state,
        accounts: response.accounts ?? [],
      });
  }

}