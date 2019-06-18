import * as React from "react";
import { BrowserRouter as Router, Route, Link, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";
import { Login } from "./pages/account/login/index";

import "./app.scss";
import { Navbar } from "reactstrap";
import { NotFound } from "./pages/notFound/NotFound";

export const App = () => {
  return (
    <Router>
      <div className="App">
        <Navbar>
          <Link to="/login">Login</Link>
          <span> and </span>
          <Link to="/register">Register</Link>
        </Navbar>
        <h1>My react app</h1>
      </div>

      <Switch>
        <Route exact path="/" component={Login} />
        <Route path="/register" component={Register} />
        <Route path="/login" component={Login} />
        <Route component={NotFound} />
      </Switch>
    </Router>
  );
};
