import React from "react";
import { NavBar } from "../../components/navBar";
import { Menu } from "../../components/menu";
import { ProfileContainer } from "../../components/profileContainer";
import { Buildings } from "./buildings";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { Development } from "./development/Development";
import { War } from "./war";
import { Rank } from "./rank";
import { Army } from "./army";
import { TestConnected } from "../gyakorlás/connect";
import { BuildingsConnected } from "./buildings/connect";
import { NavBarConnected } from "../../components/navBar/connect";
import { Attack } from "./attack";
import { NavBarIconProp } from "../../components/navBarIcons/Interface";
import { NavBarIcon } from "../../components/navBarIcons";
import { NavbarState } from "../../components/navBar/store/store";
import { ArmyConnected } from "./army/connect";
import { MainPageProps } from "./Interface";
//import { ArmyConnected } from "./army/connect";

export class MainPage extends React.Component<MainPageProps> {
  // constuct
  componentDidMount() {
    // document.title = "Ország";
    console.log("MainPage mount");
    this.props.beginFetchMainpage();
  }
  componentDidUpdate() {
    console.log("MainPage Update");
  }

  render() {
    return (
      <div className="main-page">
        <NavBarConnected />
        <div className="mainpage-content">
          <div className="side-menu">
            <Menu />
            <div>
              <ProfileContainer />
              <h3 className="undersea-font-mainpage">UNDERSEA</h3>
            </div>
          </div>
          <main>
            <Switch>
              <Route path="/account/buildings">
                <BuildingsConnected />
              </Route>
              <Route path="/account/development">
                <Development />
              </Route>
              <Route path="/account/war">
                <War />
              </Route>
              <Route path="/account/attack">
                <Attack />
              </Route>
              <Route path="/account/rank">
                <Rank />
              </Route>
              <Route path="/account/army">
                <ArmyConnected isNative />
              </Route>
              <Route path="/account/gyak">
                <TestConnected isNative />
              </Route>
            </Switch>
          </main>
        </div>
      </div>
    );
  }
}
