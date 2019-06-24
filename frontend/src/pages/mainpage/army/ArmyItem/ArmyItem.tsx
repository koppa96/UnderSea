import React from "react";
import { ArmyItemProps } from "./Interface";

export class ArmyItem extends React.Component<ArmyItemProps> {
  state = {
    currentTrop: 0
  };

  addTrop = () =>
    this.setState({ currentTrop: this.state.currentTrop + 1 }, () =>
      this.props.currentTrops(this.props.solider.id, this.state.currentTrop)
    );
  removeTrop = () =>
    this.setState({ currentTrop: this.state.currentTrop - 1 }, () =>
      this.props.currentTrops(this.props.solider.id, this.state.currentTrop)
    );
  render() {
    const {
      imageUrl,
      title,
      amount,
      price,
      price2,
      price3,
      stat
    } = this.props.solider;
    const { currentTrop } = this.state;
    return (
      <div className="solider-item">
        <div className="rectangle army-rectangle">
          <img alt="solider" src={imageUrl} />
        </div>
        <h3>{title}</h3>
        <div>
          <span>Birtokodban:</span>
          <span>{amount}</span>
        </div>
        <div>
          <span>Támadás/Védekezés</span>
          <span>{stat}</span>
        </div>
        <div>
          <span>Zsold(/kör/példány)</span>
          <span>{price}</span>
        </div>
        <div>
          <span>Ellátmány(/kör/példány)</span>
          <span>{price2}</span>
        </div>
        <div>
          <span>Ár</span>
          <span>{price3}</span>
        </div>
        <div className="army-circle">
          <div onClick={this.removeTrop} className="circle">
            <p>-</p>
          </div>
          <span>{currentTrop}</span>
          <div onClick={this.addTrop} className="circle">
            <p>+</p>
          </div>
        </div>
      </div>
    );
  }
}
