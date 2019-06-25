import * as React from "react";
import { NavBarIcon } from "./../navBarIcons/index";
import { NavBarProps } from "./Interface";

export class NavBar extends React.Component<NavBarProps> {
  componentDidMount() {}

  componentDidUpdate() {
    console.log("Navbar Updated");
    // console.log(this.props.navbar.navBarIcons);
  }
  componentWillUpdate() {
    console.log("Navbar will Update");
    // console.log(this.props.navbar.navBarIcons);
  }

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
            {navbar.navBarIcons.map(item => (
              <li key={item.id}>
                <NavBarIcon
                  id={item.id}
                  imageUrl={item.imageUrl}
                  amount={item.amount}
                />
              </li>
            ))}
          </ul>
        </div>
      </div>
    );
  }
}
