import * as React from "react";
import { LogCheckProps } from "./Interface";
import { Redirect } from "react-router";

export class LoginCheck extends React.Component<LogCheckProps> {
  render = () => {
    const { children, login } = this.props;

    const mockedLogIn = false;
    return (login && mockedLogIn) || (!login && !mockedLogIn) ? (
      children
    ) : mockedLogIn ? (
      <Redirect to="/" />
    ) : (
      <Redirect to="/login" />
    );
  };
}
