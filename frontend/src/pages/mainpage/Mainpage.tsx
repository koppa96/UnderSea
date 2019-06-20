import React from "react";
import { NavBar } from "../../components/navBar";
import { Menu } from "../../components/menu";
import { ProfileContainer } from "../../components/profileContainer";
import { Buildings } from "./buildings";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { Development } from "./development/Development";
import { War } from "./war";

export class MainPage extends React.Component {
  componentWillMount() {
    document.title = "Ország";
  }

  render() {
    return (
      <Router>
        <div className="main-page">
          <NavBar />
          <div className="mainpage-content">
            <div className="side-menu">
              <Menu />
              <div>
                <ProfileContainer />
              </div>
              <h3 className="undersea-font-mainpage">UNDERSEA</h3>
            </div>
            <main>
              <Switch>
                <Route path="/account/buildings">
                  <Buildings />
                </Route>

                <Route path="/account/development">
                  <Development />
                </Route>
                <Route path="/account/war">
                  <War />
                </Route>
              </Switch>
            </main>
          </div>
        </div>
      </Router>
    );
  }
}
