import * as React from "react";
import { BuildingProps } from "./Interface";
import { BasePortUrl } from "../../../..";
import QuestionMark from "./../../../../assets/images/question.svg";

export const BuildingItem = (props: BuildingProps) => {
  const { title, description, price, amount, imageUrl } = props;

  const image = imageUrl ? BasePortUrl + imageUrl : QuestionMark;
  return (
    <div className="building-item">
      <img alt="building" src={image} />
      <div>
        <p>{title}</p>
        <p>{description}</p>
        <div>
          <p>{amount} db</p>
          <p>{price}</p>
        </div>
      </div>
    </div>
  );
};
