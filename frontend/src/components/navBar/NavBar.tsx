import * as React from "react";
import { NavBarIcon } from "./../navBarIcons/index";
import { NavBarProps } from "./Interface";

export class NavBar extends React.Component<NavBarProps> {
  render() {
    //2x render?
    const { navbar } = this.props;
    // console.log("navbar render", navbar);
    return (
      <div className="navbar-bg">
        <div className="navbar-color ">
          <div>
            <span>4.kör</span>

            <span>23.hely</span>
          </div>
          <ul className="nav navbar-nav">
            {navbar.navBarIcons ? (
              navbar.navBarIcons.map(item => (
                <li key={item.id}>
                  <NavBarIcon
                    id={item.id}
                    imageUrl={item.imageUrl ? item.imageUrl : ""}
                    count={item.count ? item.count : 0}
                    info={item.inProgressCount ? item.count : 0}
                  />
                </li>
              ))
            ) : (
              <div>Nincs adat</div>
            )}
          </ul>
        </div>
      </div>
    );
  }
}
