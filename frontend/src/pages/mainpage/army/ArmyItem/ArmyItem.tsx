import React from "react";
import { ArmyItemResponse } from "./Interface";
import { BasePortUrl } from "../../../..";
import { ArmyInfoWoCount } from "../store/actions/ArmyActions.get";

interface ArmyProps {
  unit: ArmyInfoWoCount;
  currentTroops: Function;
  count: number;
}

export class ArmyItem extends React.Component<ArmyProps> {
  state = {
    currentTroop: 0
  };

  addTroop = () =>
    this.setState({ currentTroop: this.state.currentTroop + 1 }, () =>
      this.props.currentTroops(this.props.unit.id, this.state.currentTroop)
    );
  removeTroop = () => {
    if (this.state.currentTroop > 0)
      this.setState({ currentTroop: this.state.currentTroop - 1 }, () =>
        this.props.currentTroops(this.props.unit.id, this.state.currentTroop)
      );
  };
  render() {
    const {
      imageUrl,
      name,
      maintenanceCoral,
      maintenancePearl,
      costPearl,
      attackPower,
      defensePower
    } = this.props.unit;
    const { currentTroop } = this.state;
    return (
      <div className="solider-item">
        <div className="rectangle army-rectangle">
          <img alt="solider" src={BasePortUrl + imageUrl} />
        </div>
        <h3>{name}</h3>
        <div>
          <span>Birtokodban:</span>
          <span>{this.props.count}</span>
        </div>
        <div>
          <span>Támadás/Védekezés</span>
          <span>{attackPower + "/" + defensePower}</span>
        </div>
        <div>
          <span>Zsold(/kör/példány)</span>
          <span>{maintenancePearl}</span>
        </div>
        <div>
          <span>Ellátmány(/kör/példány)</span>
          <span>{maintenanceCoral}</span>
        </div>
        <div>
          <span>Ár</span>
          <span>{costPearl}</span>
        </div>
        <div className="army-circle">
          <div onClick={this.removeTroop} className="circle">
            <p>-</p>
          </div>
          <span>{currentTroop}</span>
          <div onClick={this.addTroop} className="circle">
            <p>+</p>
          </div>
        </div>
      </div>
    );
  }
}
