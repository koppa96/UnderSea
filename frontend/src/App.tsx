import * as React from "react";
import { BrowserRouter as Router, Route, Link, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";
import { Login } from "./pages/account/login/index";

import "./app.scss";
import { Navbar } from "reactstrap";
import { NotFound } from "./pages/notFound/index";
import { LoginCheck } from "./components/LoginCheck/LoginCheck";
import { MainPage } from "./pages/mainpage/Mainpage";

export const App = () => {
  const loggedin = true;

  return (
    <Router>
      <div className="App">
        <div className="bg-image">
          <div className="mainpage-width">
            <span className="game-name">Undersea</span>
            <Switch>
              <Route exact path="/">
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

          <Navbar>
            <Link to="/login">Login</Link>
            <span> and </span>
            <Link to="/register">Register</Link>
          </Navbar>
          <h1>My react app</h1>
        </div>
      </div>
    </Router>
  );
};
