import * as React from "react";
import { DevelopmentProps } from "./Interface";
import { BasePortUrl } from "../../../..";
import Checkmark from "./../../../../assets/images/check_mark.png";
import QuestionMark from "./../../../../assets/images/question.svg";

export const DevelopmentItem = (props: DevelopmentProps) => {
  const { inProgress, count, info } = props;
  const image = info
    ? info.imageUrl
      ? BasePortUrl + info.imageUrl
      : QuestionMark
    : QuestionMark;
  return (
    <div className="development-item">
      <div className="development-img">
        {inProgress !== undefined && inProgress !== 0 && (
          <p>Fejlesztés alatt</p>
        )}
        {count > 0 && (
          <div className="checkmark">
            <img src={Checkmark} alt="checked" />
          </div>
        )}
        <div className="img-size-content">
          <img alt="building" src={image} />
        </div>
      </div>
      <div>
        <p className="building-font-bold">{info && info.name}</p>
        <p>{info && info.description}</p>
        <p className="development-price">Ár: {info && info.cost} gyöngy</p>
        <div />
      </div>
    </div>
  );
};
