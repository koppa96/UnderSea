import * as React from "react";
import { NavBarIcon } from "./../navBarIcons/index";
import { NavBarProps } from "./Interface";

export class NavBar extends React.Component<NavBarProps> {
  render() {
    const { navBarIcons } = this.props.navbar;
    const rank = navBarIcons && <span>{navBarIcons.rank}.hely</span>;
    const round = navBarIcons && <span>{navBarIcons.round}.kör</span>;
    const navbarBuildings = navBarIcons && navBarIcons.buildings;
    const navbarArmy = navBarIcons && navBarIcons.armyInfo;
    const navbarPearl = navBarIcons && navBarIcons.pearls;
    const navbarCollar = navBarIcons && navBarIcons.corals;

    const navbarPearlPerRound = navBarIcons && navBarIcons.pearlsPerRound;
    const navbarCoralPerRound = navBarIcons && navBarIcons.coralsPerRound;
    console.log("navbararmy", navbarArmy);
    return (
      <div className="navbar-bg">
        <div className="navbar-color ">
          <div className="navbar-info">
            {navBarIcons && navBarIcons.rank > 0 && rank}
            {round}
          </div>
          <div className="navbar-wrapper">
            <ul className="nav navbar-nav">
              {navbarArmy &&
                navbarArmy.map(item => (
                  <li key={item.id}>
                    <NavBarIcon
                      id={item.id}
                      imageUrl={item.imageUrl && item.imageUrl}
                      count={item.totalCount && item.totalCount}
                      name={item.name && item.name}
                      info={item.defendingCount + " véd"}
                    />
                  </li>
                ))}
            </ul>
            <NavBarIcon
              count={navbarCollar ? navbarCollar : 0}
              id={0}
              info={navbarCoralPerRound + "/kör"}
              name="koral"
            />
            <NavBarIcon
              count={navbarPearl ? navbarPearl : 0}
              id={1}
              info={navbarPearlPerRound + "/kör"}
              name="gyöngy"
            />
            <ul className="nav navbar-nav">
              {navbarBuildings &&
                navbarBuildings.map(item => (
                  <li key={item.id}>
                    <NavBarIcon
                      id={item.id}
                      imageUrl={item.iconImageUrl ? item.iconImageUrl : ""}
                      count={item.count ? item.count : 0}
                      info={
                        item.inProgressCount
                          ? item.inProgressCount + " épül"
                          : "0 épül"
                      }
                    />
                  </li>
                ))}
            </ul>
          </div>
        </div>
      </div>
    );
  }
}
