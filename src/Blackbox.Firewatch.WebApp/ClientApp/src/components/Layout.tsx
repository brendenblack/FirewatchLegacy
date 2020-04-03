import React, { Component } from 'react';
import { NavMenu } from './NavMenu';
import { StyleSheet, css } from 'aphrodite';
import { Colors } from "@blueprintjs/core";

const styles = StyleSheet.create({
  root: {
    backgroundColor: Colors.DARK_GRAY4,
  },
})

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div className="bp3-dark">
        <NavMenu />
        
          {this.props.children}
 
      </div>
    );
  }
}
