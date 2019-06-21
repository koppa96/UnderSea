import React from "react";
import { ArmyItemProps } from "./Interface";

export class ArmyItem extends React.Component<ArmyItemProps> {
  render() {
    const { imageUrl, title, properties } = this.props;
    return (
      <div className="building-item">
        <img alt="building" src={imageUrl} />
        <div>
          <p>{title}</p>
        </div>
      </div>
    );
  }
}
