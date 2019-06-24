import * as React from "react";
import BuildingImg from "./../../../../assets/images/development-bg.png";
import { DevelopmentProps } from "./Interface";

export const DevelopmentItem = (props: DevelopmentProps) => {
  const { title, description, isDeveloped, time, imageUrl } = props.development;

  return (
    <div className="development-item">
      <div className="development-img">
        {(time !== undefined) && (+time[0] != 0) && <p> még {time} kör</p>}
        {isDeveloped && (
          <div>
            <span>a</span>
          </div>
        )}
        <img alt="building" src={BuildingImg} />
      </div>
      <div>
        <p className="building-font-bold">{title}</p>
        <p>{description}</p>
        <div />
      </div>
    </div>
  );
};
