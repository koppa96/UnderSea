import React from "react";
import { Menu } from "../../components/menu";
import { Route, Switch } from "react-router-dom";
import { Development } from "./development/Development";
import { TestConnected } from "../gyakorlás/connect";
import { BuildingsConnected } from "./buildings/connect";
import { NavBarConnected } from "../../components/navBar/connect";
import { Attack } from "./attack";
import { ArmyConnected } from "./army/connect";
import { MainPageProps } from "./Interface";
import { RankConnected } from "./rank/connect";
import { ProfileContainerConnected } from "../../components/profileContainer/connect";
import { WarConnected } from "./war/connect";
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
              <ProfileContainerConnected />
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
                <WarConnected />
              </Route>
              <Route path="/account/attack">
                <Attack />
              </Route>
              <Route path="/account/rank">
                <RankConnected />
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
