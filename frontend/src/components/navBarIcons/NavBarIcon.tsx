import * as React from "react";
import { NavBarIconProp } from "./Interface";

import ProfileImg from "./../../assets/images/profile-bg.svg";

export const NavBarIcon = (props: NavBarIconProp) => {
  const info = props.info && (
    <span className="navbaricon-info">{props.info}</span>
  );
  //TODO: képek
  return (
    <div className="navbaricon-bg">
      <div className="navbaricon-rectangle">
        <img src={ProfileImg} alt="Icon" />
      </div>
      <div className="navbaricon-font">
        <span className="navbaricon-amount">{props.amount}</span>
        <span className="navbaricon-amount">{props.id}</span>
        {info}
      </div>
    </div>
  );
};
