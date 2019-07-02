import * as React from "react";
import { Link } from "react-router-dom";
import { MenuProps } from "./Interface";

export class Menu extends React.Component<MenuProps> {
  render() {
    const { unseenReports } = this.props;
    return (
      <ul className="menu-bg">
        <li>
          <Link to="/account/buildings">Épületek</Link>
        </li>
        <li>
          <Link to="/account/attack">Támadás</Link>
        </li>
        <li>
          <Link to="/account/development">Fejlesztések</Link>
        </li>
        <li>
          <Link to="/account/war">Harc</Link>
        </li>
        <li>
          <Link to="/account/rank">Ranglista</Link>
        </li>
        <li>
          <Link to="/account/army">Sereg</Link>
        </li>
        <li>
          <div>
            {unseenReports > 0 && <p>{unseenReports}</p>}

            <Link to="/account/report">Csatajelentés</Link>
          </div>
        </li>
      </ul>
    );
  }
}
