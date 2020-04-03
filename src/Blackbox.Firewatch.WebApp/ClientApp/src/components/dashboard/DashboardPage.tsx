import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { } from '../../firewatch.service';
import { AccountsWidget } from './AccountsWidget';
import { WidgetContainer } from '../widget/WidgetContainer';
import { Colors } from "@blueprintjs/core";
import { StyleSheet, css } from 'aphrodite';

interface DashboardPageProps extends RouteComponentProps<any> {

}

interface DashboardPageState {
}

const styles = StyleSheet.create({
  root: {
    margin: '1em',
    backgroundColor: Colors.DARK_GRAY5,
    borderColor: Colors.DARK_GRAY3,
    borderStyle: 'solid',
    borderWidth: 1,
    
    padding: '1em,'
  },
});

export class DashboardPage extends Component<DashboardPageProps, DashboardPageState> {

  constructor(props: DashboardPageProps) {
    super(props);

    this.state = {
    };    
  }

  componentDidMount() {
      this.populateDashboard();
  }

  render() {
      return (
        <div className={css(styles.root)}>
          <WidgetContainer title="Accounts" onClose={() => { }}>
          <AccountsWidget />
          </WidgetContainer>
        </div>);
  }

  async populateDashboard() {
    
  }

}