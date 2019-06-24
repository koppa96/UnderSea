import * as React from "react";
import { BuildingProps } from "./Interface";
import BuildingImg from "./../../../../assets/images/building-bg.png";

export const BuildingItem = (props: BuildingProps) => {
  const { title, description, price, amount, imageUrl } = props;

  return (
    <div className="building-item">
      <img alt="building" src={BuildingImg} />
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
