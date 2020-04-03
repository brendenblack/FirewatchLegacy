import React, { Component } from 'react';
import { FormGroup, InputGroup, Checkbox, Button } from "@blueprintjs/core";
import { StyleSheet, css } from 'aphrodite';
import { QueryParameterNames } from './ApiAuthorizationConstants';
import authService, { AuthenticationResultStatus } from './AuthorizeService';
import { AuthenticationClient, LoginModel } from '../../firewatch.service';

const styles = StyleSheet.create({
  root: {
	display: 'flex',
	justifyContent: 'center',
	alignItems: 'center',
  },
  loginForm: {
    width: 400,
  },
});

interface LoginPageProps {

}

interface LoginPageState {
  returnUrl: string;
  message: string | undefined;

  username: string;
  password: string;
  rememberMe: boolean;
}

export class LoginPage extends Component<LoginPageProps, LoginPageState> {
  constructor(props: LoginPageProps) {
    super(props);

    this.state = {
      message: undefined,
      returnUrl: '',
      username: '',
      password: '',
      rememberMe: false,
    };

    this.handlePasswordChange = this.handlePasswordChange.bind(this);
    this.handleUsernameChange = this.handleUsernameChange.bind(this);
    this.handleRememberMe = this.handleRememberMe.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount() {
    // const action = this.props.action;

  }

  handleUsernameChange(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      ...this.state,
      username: event.target.value,
    });
  }

  handlePasswordChange(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      ...this.state,
      password: event.target.value,
    });
  }

  handleRememberMe(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({
      ...this.state,
      rememberMe: event.target.checked,
    });
  }

  async handleSubmit() {
	  try {
		const client = new AuthenticationClient();
		const model = new LoginModel();
		model.email = this.state.username;
		model.password = this.state.password;
		model.rememberMe = this.state.rememberMe;
		const loginResult = await client.login(model);
		console.log('Login result', loginResult);

		const result = await authService.signIn('');
		console.log('Sign in result', result);
		switch (result.status) {
			case AuthenticationResultStatus.Success:
				await this.navigateToReturnUrl('~');
				break;
			default:
				console.log('Received status ' + result.status);
		}
    }
    catch (e) {
    	console.warn('Caught login exception', e);
    }
  }

  get canSubmit(): boolean {
    return (this.state.username && this.state.password) ? true : false;
  }

  async login(returnUrl: string) {
    const state = { returnUrl };
    try
      {
      const result = await authService.signIn(state) as { status: string, message?: string };
      switch (result.status) {
          case AuthenticationResultStatus.Redirect:
              break;
          case AuthenticationResultStatus.Success:
              await this.navigateToReturnUrl(returnUrl);
              break;
          case AuthenticationResultStatus.Fail:
              this.setState({ message: result.message });
              break;
          default:
              throw new Error(`Invalid status result ${result.status}.`);
      }
    }
    catch (e) {
      
    }
}

navigateToReturnUrl(returnUrl: string) {
  // It's important that we do a replace here so that we remove the callback uri with the
  // fragment containing the tokens from the browser history.
  window.location.replace(returnUrl);
}

  getReturnUrl(state: LoginPageState) {
    const params = new URLSearchParams(window.location.search);
    const fromQuery = params.get(QueryParameterNames.ReturnUrl);
    if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
        // This is an extra check to prevent open redirects.
        throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
    }
    return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`;
}

  render () {
    const message = this.state.message;
    const action = ''
    if (!!message) {
      return <div>{message}</div>
    } 
    // else {
    //   switch (action) {
    //     default:
    //       break;
    //   }
    // }

      return (
          <div className={css(styles.root)}>
            <div className={css(styles.loginForm)}>
              <h1>Log in</h1>
              <p>Log in with a local account.</p>
              <hr />
              <FormGroup label="Email" labelFor="email">
                  <InputGroup id="email" 
                    placeholder="your@email.com" 
                    value={this.state.username} 
                    onChange={this.handleUsernameChange} />
              </FormGroup>
              <FormGroup label="Password" labelFor="password">
                  <InputGroup id="password" 
                    type="password"
                    value={this.state.password}
                    onChange={this.handlePasswordChange} />
              </FormGroup>

              <Checkbox label="Remember me?" checked={this.state.rememberMe} onChange={this.handleRememberMe} />
              <Button intent="primary" rightIcon="log-in" disabled={!this.canSubmit} onClick={this.handleSubmit}>Log in</Button>
              <Button minimal>Forgot your password?</Button>
            </div>
          </div>
      
      );
  }
};