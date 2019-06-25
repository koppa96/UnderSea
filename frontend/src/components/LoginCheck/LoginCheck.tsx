import * as React from "react";
import { LogCheckProps } from "./Interface";
import { Redirect } from "react-router";

export class LoginCheck extends React.Component<LogCheckProps> {
  render = () => {
    const { children, login, serverLogin } = this.props;

    return (login && serverLogin) || (!login && !serverLogin) ? (
      children
    ) : serverLogin ? (
      <Redirect to="/" />
    ) : (
      <Redirect to="/login" />
    );
  };
}
