import * as React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";
import { Login } from "./pages/account/login/index";

import "./app.scss";
import { NotFound } from "./pages/notFound/index";
import { LoginCheck } from "./components/LoginCheck/LoginCheck";
import { MainPage } from "./pages/mainpage";
import { ConnectedRouter } from "connected-react-router";
import { createBrowserHistory } from "history";

export const App = () => {
  const loggedin = true;

  //TODO: Router kiszervezés
  return (
    <ConnectedRouter history={createBrowserHistory({ basename: "" })}>
      <div className="App">
        <div className="bg-image">
          <Switch>
            <Route exact path="/">
              <LoginCheck login={loggedin}>
                <MainPage />
              </LoginCheck>
            </Route>
            <Route path="/account">
              <LoginCheck login={loggedin}>
                <MainPage />
              </LoginCheck>
            </Route>
            <Route path="/register" component={Register} />

            <Route path="/login">
              <LoginCheck login={!loggedin}>
                <Login />
              </LoginCheck>
            </Route>
            <Route component={NotFound} />
          </Switch>
        </div>
      </div>
    </ConnectedRouter>
  );
};
