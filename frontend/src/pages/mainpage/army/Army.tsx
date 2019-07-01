import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ArmyItem } from "./ArmyItem";
import { ArmyProps } from "./Interface";
import { ArmyUnit } from "./store/store";

interface InitialState {
  units: ArmyUnit[];
  unitsAdded: boolean;
  reset: boolean;
  priceAll: number;
  disableButton: boolean;
}

export class Army extends React.Component<ArmyProps, InitialState> {
  componentDidMount() {
    document.title = title;
    if (!this.props.ownedUnitState.isLoaded) {
      this.props.getArmy();
    }
  }
  state: InitialState = {
    units: [],
    unitsAdded: false,
    reset: false,
    priceAll: 0,
    disableButton: false
  };

  componentDidUpdate = async () => {
    if (!this.state.reset && this.props.ownedUnitState.isPostSuccessFull) {
      await this.setState({ reset: true, units: [], unitsAdded: false });
      this.props.resetUnits();
      this.setState({ reset: false, priceAll: 0 });
    }
  };

  currentSoldiers = async (id: number, troop: number, price: number) => {
    this.setState({ reset: false });
    const temp = this.state.units;
    const index = this.state.units.findIndex(unit => unit.unitId === id);
    if (index !== undefined && index !== -1) {
      temp[index].count = troop;
    } else {
      temp.push({ unitId: id, count: troop, price: price });
    }

    var priceTemp = 0;
    this.state.units.forEach(unit => {
      priceTemp += unit.price * unit.count;
    });
    this.setState({ priceAll: priceTemp });
    if (priceTemp > this.props.pearls) {
      this.setState({ disableButton: true });
    } else {
      this.setState({ disableButton: false });
    }

    this.setState({ units: temp });
    if (temp.some(item => item.count !== 0)) {
      this.setState({ unitsAdded: true });
    } else {
      this.setState({ unitsAdded: false });
    }
  };

  render() {
    const { addUnits, ownedUnitState, count } = this.props;
    const buttonState =
      !this.state.unitsAdded ||
      ownedUnitState.isPostRequesting ||
      this.state.disableButton;
    const buttonClass = buttonState ? "button-disabled" : "button";

    return (
      <div className="main-component army-component">
        <ComponentHeader title={title} mainDescription={mainDescription} />
        {ownedUnitState.isRequesting && "loading..."}
        <div className="army-text">
          <span>Összes ár: </span>
          <span>{this.state.priceAll}</span>
        </div>

        {ownedUnitState.isLoaded && (
          <div className="army-page hide-scroll">
            {ownedUnitState.units.length > 0 &&
              ownedUnitState.units.map(item => {
                const curentCount = count.find(c => c.id === item.id);
                return (
                  <ArmyItem
                    key={item.id}
                    unit={item}
                    count={curentCount ? curentCount.count : 0}
                    currentTroops={this.currentSoldiers}
                    reset={this.state.reset}
                  />
                );
              })}
          </div>
        )}

        <button
          disabled={buttonState}
          className={buttonClass}
          onClick={() => {
            addUnits({ unitsToAdd: this.state.units });
          }}
        >
          {ownedUnitState.isPostRequesting ? "töltés.." : "Megveszem"}
        </button>
        {ownedUnitState.error && (
          <p className="common-error-text">{ownedUnitState.error}</p>
        )}
      </div>
    );
  }
}
const title: string = "Sereg";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
