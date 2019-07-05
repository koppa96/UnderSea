import React from "react";
import { Route, Switch } from "react-router-dom";
import { BuildingsConnected } from "./buildings/connect";
import { NavBarConnected } from "../../components/navBar/connect";
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
import { MenuConnected } from "../../components/menu/connect";
import { ReportsConnected } from "./reports/connect";
import { Modal } from "reactstrap";

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

    connection.start().then(function() {
      console.log("connected SignalR");
    });
    connection.on("ReceiveResultsAsync", country => {
      this.props.refreshCountryInfo(country);
    });
  }

  state = {
    showPopup: false
  };
  togglePopup = () => {
    this.setState({
      showPopup: !this.state.showPopup
    });
  };
  render() {
    const { building, event, researches } = this.props;
    return (
      <>
        <div className="main-page">
          <NavBarConnected />
          <div className="mainpage-content">
            <div className="side-menu">
              <MenuConnected />
              <div>
                <ProfileContainerConnected
                  togglePopup={() => this.togglePopup()}
                />
                <div>
                  <img className="main-wave" src={Wave} alt="wave" />
                  <h3 className="undersea-font-mainpage">UNDERSEA</h3>
                </div>
              </div>
            </div>
            <main>
              <div className="building-img-holder">
                <div className="building">
                  {building &&
                    building.map(
                      item =>
                        item.count > 0 &&
                        item.imageUrl && (
                          <div key={item.id} className="bg-items-flex">
                            <img
                              src={BasePortUrl + item.imageUrl}
                              alt="items"
                              className="building-item-animation"
                            />
                          </div>
                        )
                    )}
                </div>
                <div className="researches">
                  {researches &&
                    researches.map(
                      item =>
                        item.count > 0 &&
                        item.imageUrl && (
                          <div
                            key={item.id}
                            className={
                              item.imageUrl.indexOf("szonaragyu") >= 0
                                ? "bg-items-flex szonar"
                                : "bg-items-flex research-item-animation"
                            }
                          >
                            <img
                              className={
                                item.imageUrl.indexOf("szonaragyu") >= 0
                                  ? "szonar"
                                  : "img"
                              }
                              src={BasePortUrl + item.imageUrl}
                              alt="items"
                            />
                          </div>
                        )
                    )}
                </div>
                <div id="background-wrap">
                  <div className="bubble x1" />
                  <div className="bubble x2" />
                  <div className="bubble x3" />
                  <div className="bubble x4" />
                  <div className="bubble x5" />
                  <div className="bubble x6" />
                  <div className="bubble x7" />
                  <div className="bubble x8" />
                  <div className="bubble x9" />
                  <div className="bubble x10" />
                </div>
              </div>
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

              {this.state.showPopup && event ? (
                <Modal
                  className="main-component"
                  contentClassName="main-component"
                  isOpen={this.state.showPopup}
                  toggle={() => this.togglePopup()}
                >
                  <div className="popup-mainpage">
                    <h1>Hoppá!</h1>
                    <h2>{event && event.name ? event.name : "Titok"}</h2>
                    <span>
                      {event && event.description
                        ? event.description
                        : "Mi lehet a leírás?"}
                    </span>
                    <button onClick={() => this.togglePopup()}>
                      {event && event.flavourtext ? event.flavourtext : null}
                    </button>
                  </div>
                </Modal>
              ) : (
                () => this.setState({ showPopup: false })
              )}
            </main>
          </div>
        </div>
      </>
    );
  }
}
