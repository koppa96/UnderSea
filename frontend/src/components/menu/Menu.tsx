import * as React from "react";
import { Link } from "react-router-dom";

export const Menu = () => {
  return (
    <ul className="menu-bg">
      <li>
        <Link to="/account/buildings">Épületek</Link>
      </li>
      <li>
        <Link to="/account/gyak">Gyakorlás</Link>
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
    </ul>
  );
};
