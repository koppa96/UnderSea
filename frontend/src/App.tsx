import * as React from "react";
import { Route, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";

import axios from "axios";
import "./app.scss";
import { NotFound } from "./pages/notFound/index";
import { LoginConnected } from "./pages/account/login/connect";
import { MainPageConnected } from "./pages/mainpage/connect";
import { BasePortUrl } from ".";
import qs from "qs";
import { LoginProps } from "./Interface";
import { LoginCheck } from "./components/LoginCheck/LoginCheck";
import { LoginCheckConnected } from "./components/LoginCheck/connect";

export class App extends React.Component<LoginProps> {
  //TODO: Router kiszervezés
  componentDidMount() {
    this.props.getUserInfo();
  }

  render() {
    const { serverResponseLogin } = this.props;
    console.log("serverresponselogin", serverResponseLogin);
    return (
      <div className="App">
        <div className="bg-image">
          <Switch>
            <Route exact path="/">
              <LoginCheckConnected
                login={true}
                serverResponseLogin={serverResponseLogin}
              >
                <MainPageConnected />
              </LoginCheckConnected>
            </Route>
            <Route path="/account">
              <LoginCheckConnected
                login={true}
                serverResponseLogin={serverResponseLogin}
              >
                <MainPageConnected />
              </LoginCheckConnected>
            </Route>
            <Route path="/register" component={Register} />

            <Route path="/login">
              <LoginCheckConnected
                login={false}
                serverResponseLogin={serverResponseLogin}
              >
                <LoginConnected />
              </LoginCheckConnected>
            </Route>
            <Route component={NotFound} />
          </Switch>
        </div>
      </div>
    );
  }
}
