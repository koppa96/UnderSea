import * as React from "react";
import BuildingImg from "./../../../../assets/images/development-bg.png";
import { DevelopmentProps } from "./Interface";
import { BasePortUrl } from "../../../..";

export const DevelopmentItem = (props: DevelopmentProps) => {
  const { inProgress, count, info } = props;

  return (
    <div className="development-item">
      <div className="development-img">
        {inProgress !== undefined && inProgress !== 0 && (
          <p>{inProgress} épül</p>
        )}
        {count > 0 && (
          <div>
            <img src="src\assets\images\check_mark.png" alt="checked" />
          </div>
        )}
        <img
          alt="building"
          src={BasePortUrl + (info && info.imageUrl ? info.imageUrl : "")}
        />
      </div>
      <div>
        <p className="building-font-bold">{info && info.name}</p>
        <p>{info && info.description}</p>
        <div />
      </div>
    </div>
  );
};
