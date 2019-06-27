import * as React from "react";
import { NavBarIcon } from "./../navBarIcons/index";
import { NavBarProps } from "./Interface";
const PearlImage = "src/assets/images/pearl.svg";

export class NavBar extends React.Component<NavBarProps> {
  render() {
    //2x render?
    const rank = this.props.navbar.navBarIcons && (
      <span>{this.props.navbar.navBarIcons.rank}.hely</span>
    );
    const round = this.props.navbar.navBarIcons && (
      <span>{this.props.navbar.navBarIcons.round}.kör</span>
    );
    const navbarBuildings =
      this.props.navbar.navBarIcons && this.props.navbar.navBarIcons.buildings;
    const navbarArmy =
      this.props.navbar.navBarIcons && this.props.navbar.navBarIcons.armyInfo;
    const navbarPearl =
      this.props.navbar.navBarIcons && this.props.navbar.navBarIcons.pearls;
    const navbarCollar =
      this.props.navbar.navBarIcons && this.props.navbar.navBarIcons.corals;
    // console.log("navbar render", navbar);
    return (
      <div className="navbar-bg">
        <div className="navbar-color ">
          <div>
            {rank}
            {round}
          </div>
          <ul className="nav navbar-nav">
            {navbarArmy &&
              navbarArmy.map(item => (
                <li key={item.id}>
                  <NavBarIcon
                    id={item.id}
                    imageUrl={item.imageUrl ? item.imageUrl : ""}
                    count={item.count ? item.count : 0}
                  />
                </li>
              ))}
            <NavBarIcon
              count={navbarCollar ? navbarCollar : 0}
              money={true}
              id={0}
            />
            <NavBarIcon
              count={navbarPearl ? navbarPearl : 0}
              money={false}
              id={1}
            />
            {navbarBuildings &&
              navbarBuildings.map(item => (
                <li key={item.id}>
                  <NavBarIcon
                    id={item.id}
                    imageUrl={item.imageUrl ? item.imageUrl : ""}
                    count={item.count ? item.count : 0}
                    info={item.inProgressCount ? item.count : 0}
                  />
                </li>
              ))}
          </ul>
        </div>
      </div>
    );
  }
}
