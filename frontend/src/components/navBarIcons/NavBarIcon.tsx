import * as React from "react";
import { NavBarIconProp } from "./Interface";

export const NavBarIcon = (props: NavBarIconProp) => {
  const info = props.info && (
    <span className="navbaricon-info">{props.info}</span>
  );
  //TODO: képek
  return (
    <div className="navbaricon-bg">
      <div className="navbaricon-rectangle">
        <div>asd</div>
      </div>
      <div className="navbaricon-font">
        <span className="navbaricon-amount">{props.amount}</span>
        {info}
      </div>
    </div>
  );
};
