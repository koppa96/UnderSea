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
import { DevelopmentConnected } from "./development/connect";
import { AttackConnected } from "./attack/connect";
import Wave from "./../../assets/images/wave.svg";
import Profile from "./Profile/Profile";

import * as signalR from "@aspnet/signalr";
import { BasePortUrl } from "../..";
import { Reports } from "./reports/Reports";
import { MenuConnected } from "../../components/menu/connect";
import { ReportsConnected } from "./reports/connect";

export class MainPage extends React.Component<MainPageProps> {
  componentWillUnmount() {
    this.props.mounted(false);
  }
  componentDidMount() {
    document.title = "Ország";
    this.props.mounted(true);
    this.props.beginFetchMainpage();

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(BasePortUrl + "hub", {
        accessTokenFactory: async () =>
          localStorage.getItem("access_token_not_bearer") || ""
        // skipNegotiation: true,
        //  transport: signalR.HttpTransportType.WebSockets
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    console.log("----------------------------------------------------------");
    console.log(connection, "sanyi");
    console.log("----------------------------------------------------------");
    connection.start().then(function() {
      console.log("connected");
    });
    connection.on("ReceiveResultsAsync", country => {
      console.log("Country signalR", country);
    });
  }

  render() {
    const { building, loading } = this.props;
    return (
      <>
        <div className="building-img-holder">
          {building &&
            building.map(
              item =>
                item.count > 0 &&
                item.imageUrl && (
                  <div key={item.id} className="bg-items-flex">
                    <img src={BasePortUrl + item.imageUrl} />
                  </div>
                )
            )}
        </div>
        <div className="main-page">
          <NavBarConnected />
          <div className="mainpage-content">
            <div className="side-menu">
              <MenuConnected />
              <div>
                <ProfileContainerConnected />
                <div>
                  <img className="main-wave" src={Wave} alt="wave" />
                  <h3 className="undersea-font-mainpage">UNDERSEA</h3>
                </div>
              </div>
            </div>
            <main>
              <Switch>
                <Route path="/account/buildings">
                  <BuildingsConnected />
                </Route>
                <Route path="/account/development">
                  <DevelopmentConnected />
                </Route>
                <Route path="/account/war">
                  <WarConnected />
                </Route>
                <Route path="/account/attack">
                  <AttackConnected />
                </Route>
                <Route path="/account/rank">
                  <RankConnected />
                </Route>
                <Route path="/account/army">
                  <ArmyConnected isNative />
                </Route>
                <Route path="/account/report">
                  <ReportsConnected />
                </Route>
                <Route path="/account/profile">
                  <Profile />
                </Route>
              </Switch>
            </main>
          </div>
        </div>
      </>
    );
  }
}
