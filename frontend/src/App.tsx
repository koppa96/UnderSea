import * as React from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import * as Register from "./pages/account/register/index";

import "./app.scss";

export const App = () => {
  return (
    <Router>
      <div className="App">
        <Register.Register />
        <h1>My react app</h1>
      </div>
    </Router>
  );
};
