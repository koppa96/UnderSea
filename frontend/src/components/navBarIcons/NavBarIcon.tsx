import * as React from "react";
import { NavBarIconProp } from "./Interface";

import PearlImage from "./../../assets/images/shell.svg";
import CoralImage from "./../../assets/images/coral.svg";
import { BasePortUrl } from "../..";

export const NavBarIcon = (props: NavBarIconProp) => {
  const info = props.info && (
    <span className="navbaricon-info">{props.info}</span>
  );

  const image = props.imageUrl ? (
    <img src={BasePortUrl + "/" + props.imageUrl} alt="Icon" />
  ) : props.money && props.money === true ? (
    <img src={PearlImage} alt="Icon" />
  ) : (
    <img src={CoralImage} alt="Icon" />
  );
  //TODO: képek
  return (
    <div className="navbaricon-bg">
      <div className="navbaricon-rectangle">{image}</div>
      <div className="navbaricon-font">
        <span className="navbaricon-amount">{props.count}</span>
        <span className="navbaricon-amount">{props.info}</span>
      </div>
    </div>
  );
};
