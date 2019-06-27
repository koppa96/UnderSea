import * as React from "react";
import { BuildingProps } from "./Interface";
import { BasePortUrl } from "../../../..";

export const BuildingItem = (props: BuildingProps) => {
  const { title, description, price, amount, imageUrl } = props;

  return (
    <div className="building-item">
      <img alt="building" src={BasePortUrl + imageUrl} />
      <div>
        <p>{title}</p>
        <p>{description}</p>
        <div>
          <p>{amount}</p>
          <p>{price}</p>
        </div>
      </div>
    </div>
  );
};
