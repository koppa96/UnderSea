import * as React from "react";
import BuildingImg from "./../../../../assets/images/development-bg.png";
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
  console.log(image, "image");
  return (
    <div className="development-item">
      <div className="development-img">
        {inProgress !== undefined && inProgress !== 0 && (
          <p>{inProgress} épül</p>
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
        <div />
      </div>
    </div>
  );
};
