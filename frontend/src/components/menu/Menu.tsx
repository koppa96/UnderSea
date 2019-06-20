import * as React from "react";
import { Link } from "react-router-dom";

export const Menu = () => {
  return (
    <ul className="menu-bg">
      <li>
        <Link to="/account/buildings">Épületek</Link>
      </li>
      <li>
        <Link to="/asd">Támadás</Link>
      </li>
      <li>
        <Link to="/account/development">Fejlesztések</Link>
      </li>
      <li>
        <Link to="/asd">Harc</Link>
      </li>
      <li>
        <Link to="/asd">Ranglista</Link>
      </li>
      <li>
        <Link to="/asd">Sereg</Link>
      </li>
    </ul>
  );
};
