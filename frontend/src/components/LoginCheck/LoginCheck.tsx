import * as React from "react";
import { LogCheckProps } from "./Interface";
import { Redirect } from "react-router";

export class LoginCheck extends React.Component<LogCheckProps> {
  render = () => {
    const { children, login, serverLogin } = this.props;

    const loggedin = localStorage.getItem("access_token");

    return (login && loggedin) || (!login && !loggedin) ? (
      children
    ) : loggedin ? (
      <Redirect to="/" />
    ) : (
      <Redirect to="/login" />
    );
  };
}
