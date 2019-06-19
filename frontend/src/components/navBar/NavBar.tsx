import * as React from "react";
import { NavBarIcon } from "./../navBarIcons/index";

export class NavBar extends React.Component {
  render() {
    return (
      <div className="navbar-bg">
        <div className="navbar container-fluid navbar-color ">
          <ul className="nav navbar-nav">
            <li>
              <span>4.kör</span>
            </li>
            <li>
              <span>23.hely</span>
            </li>
            <li>
              <NavBarIcon url="" amount="4" />
            </li>
            <li>
              <NavBarIcon url="" amount="4" info="20/sajt" />
            </li>
            <li>
              <NavBarIcon url="" amount="400" info="200/sajt" />
            </li>
            <li>
              <NavBarIcon url="" amount="4" info="20/sajt" />
            </li>
            <li>
              <NavBarIcon url="" amount="4" info="20/sajt" />
            </li>
            <li>
              <NavBarIcon url="" amount="4" info="20/sajt" />
            </li>
            <li>
              <NavBarIcon url="" amount="4" info="20/sajt" />
            </li>
          </ul>
        </div>
      </div>
    );
  }
}
