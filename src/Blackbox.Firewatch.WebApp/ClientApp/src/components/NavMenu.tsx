import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import {
  Button,
  Navbar,
} from "@blueprintjs/core";
import authService from './api-authorization/AuthorizeService';
import { ApplicationPaths } from './api-authorization/ApiAuthorizationConstants';

interface NavMenuProps {
  
}

interface NavMenuState {
  collapsed: boolean;
  isAuthenticated: boolean;
}

export class NavMenu extends Component<NavMenuProps, NavMenuState> {
  static displayName = NavMenu.name;

  constructor (props: NavMenuProps) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      isAuthenticated: false,
    };
  }

  private _subscription: number | undefined;

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }

  async populateState() {
    const isAuthenticated = await authService.isAuthenticated();
    this.setState({
      ...this.state,
        isAuthenticated,
    });
}

renderAuthenticatedView(): JSX.Element {
	return (
		<Navbar.Group align='right'>
			<Link className="bp3-button bp3-minimal bp3-icon-credit-card" to="/transactions">Transactions</Link>
			<Link className="bp3-button bp3-minimal bp3-icon-chart" to="/dashboard">Dashboard</Link>
			<Navbar.Divider />
			<Button className="bp3-minimal" icon="user" />
			<Button className="bp3-minimal" icon="cog" />
			<Link className="bp3-button bp3-minimal bp3-icon-log-out" to={{ pathname: ApplicationPaths.LogOut, state: { local: true }}} />
		</Navbar.Group>);
}

renderAnonymousView(): JSX.Element {
	return (
    	<Navbar.Group align='right'>
			<Link className="bp3-button bp3-minimal" to={ApplicationPaths.Register}>Register</Link>
			<Link className="bp3-button bp3-minimal" to="/login">Log in</Link>
  		</Navbar.Group>);
}

  render () {

	const navbarContent = (this.state.isAuthenticated) 
		? this.renderAuthenticatedView() 
		: this.renderAnonymousView();

    return (
      <Navbar >
        <Navbar.Group align='left'>
          <Navbar.Heading>FIREwatch</Navbar.Heading>
        </Navbar.Group>
        {navbarContent}
      </Navbar>
      // <header>
      //   <Navbar className="navbar-expand-sm navbar-toggleable-sm sticky-top ng-white border-bottom box-shadow mb-3" light>
      //     <Container>
      //       <NavbarBrand tag={Link} to="/">Firewatch</NavbarBrand>
      //       <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
      //       <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
      //         <ul className="navbar-nav flex-grow">
      //           <NavItem>
      //             <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
      //           </NavItem>
      //           <NavItem>
      //             <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
      //           </NavItem>
      //           <NavItem>
      //             <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
      //           </NavItem>
      //           <UncontrolledDropdown nav inNavbar>
      //             <DropdownToggle nav caret>
      //               Transactions
      //             </DropdownToggle>
      //             <DropdownMenu right>
      //               <DropdownItem>
      //                 <NavLink tag={Link} className="text-dark" to="/transactions">Transactions</NavLink>
      //               </DropdownItem>
      //               <DropdownItem divider />
      //               <DropdownItem>
      //                 <NavLink tag={Link} className="text-dark" to="/transactions/import">Import</NavLink>
      //               </DropdownItem>
      //             </DropdownMenu>
      //           </UncontrolledDropdown>
      //           <NavItem>
                  
      //           </NavItem>
      //           <LoginMenu>
      //           </LoginMenu>
      //         </ul>
      //       </Collapse>
      //     </Container>
      //   </Navbar>
      // </header>
    );
  }
}
