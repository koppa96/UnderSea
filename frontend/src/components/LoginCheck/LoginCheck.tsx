import * as React from "react";
import { LogCheckProps, LoginCheckInterface } from "./Interface";
import { Redirect } from "react-router";

export class LoginCheck extends React.Component<LogCheckProps> {
  render = () => {
    const { serverToken } = this.props;

    return (this.props.login && this.props.serverResponseLogin) ||
      (!this.props.login && !this.props.serverResponseLogin) ? (
      this.props.children
    ) : this.props.serverResponseLogin ? (
      <Redirect to="/" />
    ) : (
      <Redirect to="/login" />
    );
  };
}
