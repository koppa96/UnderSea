import React from "react";
import { NavBar } from "../../components/navBar";
import { Menu } from "../../components/menu";
import { ProfileContainer } from "../../components/profileContainer";

export class MainPage extends React.Component {
  render() {
    return (
      <div>
        <NavBar />
        <div className="side-menu">
          <Menu />
          <ProfileContainer />
        </div>
        <h3>Gratulálok, bejelentkeztél</h3>
        <span>Placeholder szöveg</span>
      </div>
    );
  }
}
