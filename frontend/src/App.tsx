import * as React from "react";
import { Route, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";
import { Login } from "./pages/account/login/index";

import "./app.scss";
import { NotFound } from "./pages/notFound/index";
import { MainPage } from "./pages/mainpage";
import { ConnectedRouter } from "connected-react-router";
import { createBrowserHistory } from "history";
import { LoginConnected } from "./pages/account/login/connect";
import { LoginCheckConnected } from "./components/LoginCheck/connect";
import { MainPageConnected } from "./pages/mainpage/connect";

export const App = () => {
  const loggedin = true;

  //TODO: Router kiszervezés
  return (
    <div className="App">
      <div className="bg-image">
        <Switch>
          <Route exact path="/">
            <LoginCheckConnected login={loggedin}>
              <MainPageConnected />
            </LoginCheckConnected>
          </Route>
          <Route path="/account">
            <LoginCheckConnected login={loggedin}>
              <MainPageConnected />
            </LoginCheckConnected>
          </Route>
          <Route path="/register" component={Register} />

          <Route path="/login">
            <LoginCheckConnected login={!loggedin}>
              <LoginConnected />
            </LoginCheckConnected>
          </Route>
          <Route component={NotFound} />
        </Switch>
      </div>
    </div>
  );
};
