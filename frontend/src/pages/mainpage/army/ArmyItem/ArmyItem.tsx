import React from "react";
import { BasePortUrl } from "../../../..";
import { ArmyInfoWoCount } from "../store/actions/ArmyActions.get";
import QuestionMark from "./../../../../assets/images/question.svg";

interface ArmyProps {
  unit: ArmyInfoWoCount;
  currentTroops: Function;
  count: number;
  reset: boolean;
}

export class ArmyItem extends React.Component<ArmyProps> {
  state = {
    currentTroop: 0
  };

  componentWillReceiveProps() {
    if (this.props.reset) {
      this.setState({ currentTroop: 0 });
    }
  }

  addTroop = () =>
    this.setState({ currentTroop: this.state.currentTroop + 1 }, () =>
      this.props.currentTroops(
        this.props.unit.id,
        this.state.currentTroop,
        this.props.unit.costPearl
      )
    );
  removeTroop = () => {
    if (this.state.currentTroop > 0)
      this.setState({ currentTroop: this.state.currentTroop - 1 }, () =>
        this.props.currentTroops(this.props.unit.id, this.state.currentTroop)
      );
  };
  handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!Number.isInteger(+e.target.value)) {
      return;
    }
    this.setState({ currentTroop: +e.target.value });
    this.props.currentTroops(
      this.props.unit.id,
      e.target.value,
      this.props.unit.costPearl
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
    var image;
    if (imageUrl === null || imageUrl.length < 1) {
      image = QuestionMark;
    } else {
      image = BasePortUrl + imageUrl;
    }

    return (
      <div className="solider-item">
        <div className="rectangle army-rectangle">
          <img alt="solider" src={image} />
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

          <input
            type="text"
            value={this.state.currentTroop}
            className="input-field"
            onChange={e => this.handleInputChange(e)}
          />

          <div onClick={this.addTroop} className="circle">
            <p>+</p>
          </div>
        </div>
      </div>
    );
  }
}
