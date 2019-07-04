import * as React from "react";
import { NavBarIconProp } from "./Interface";

import PearlImage from "./../../assets/images/shell.svg";
import CoralImage from "./../../assets/images/coral.svg";
import QuestionMark from "./../../assets/images/question.svg";
import { BasePortUrl } from "../..";

export const NavBarIcon = (props: NavBarIconProp) => {
  const image = props.imageUrl
    ? BasePortUrl + "/" + props.imageUrl
    : props.name
    ? props.name === "gyöngy"
      ? PearlImage
      : props.name === "koral"
      ? CoralImage
      : QuestionMark
    : QuestionMark;
  //TODO: képek
  return (
    <div title={props.name ? props.name : "titok"} className="navbaricon-bg">
      <div className="navbaricon-rectangle">
        <img src={image} alt="icon" />
      </div>
      <div className="navbaricon-font">
        <span className="navbaricon-amount">
          {props.units ? props.units + "/" + props.count : props.count}
          {(props.units ? props.units > props.count : false) && <p>!</p>}
        </span>
        {props.info && <span className="navbaricon-info">{props.info}</span>}
      </div>
    </div>
  );
};
