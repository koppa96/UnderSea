import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ArmyItem } from "./ArmyItem";
import { ArmyInitialState, ArmyUnit } from "./store/store";
import { ArmyProps } from "./Interface";

const initialState = {
  troopsToAdd: [
    {
      unitId: 1,
      count: 0
    },
    {
      unitId: 2,
      count: 0
    },
    {
      unitId: 3,
      count: 0
    }
  ]
};

export class Army extends React.Component<ArmyProps> {
  componentDidMount() {
    document.title = title;
    if (!this.props.ownedUnitState.isLoaded) {
      this.props.getArmy();
    }
  }
  state = {
    ...initialState,
    unitsAdded: false
    //troops: ArmyInitialState
  };

  currentSoldiers = (id: number, troop: number) => {
    const newtTemp = this.state.troopsToAdd.map(unit => {
      if (unit.unitId == id) {
        return { ...unit, count: troop };
      }
      return unit;
    });
    console.log(newtTemp.some(item => item.count != 0));
    this.setState({ troopsToAdd: newtTemp });
    if (newtTemp.some(item => item.count != 0)) {
      this.setState({ unitsAdded: true });
    } else {
      this.setState({ unitsAdded: false });
    }
  };

  render() {
    const { addUnits, ownedUnitState } = this.props;
    console.log(this.state.troopsToAdd, "itt");
    return (
      <div className="main-component army-component">
        <ComponentHeader title={title} mainDescription={mainDescription} />
        {ownedUnitState.isRequesting && "loading..."}

        {ownedUnitState.isLoaded && (
          <div className="army-page hide-scroll">
            {ownedUnitState.units.length > 0 &&
              ownedUnitState.units.map(item => (
                <ArmyItem
                  key={item.id}
                  unit={item}
                  currentTroops={this.currentSoldiers}
                />
              ))}
          </div>
        )}

        <button
          disabled={!this.state.unitsAdded || ownedUnitState.isPostRequesting}
          onClick={() => addUnits({ unitsToAdd: this.state.troopsToAdd })}
        >
          {ownedUnitState.isPostRequesting ? "töltés.." : "Megveszem"}
        </button>
        {ownedUnitState.error && <p>{ownedUnitState.error}</p>}
      </div>
    );
  }
}
const title: string = "Sereg";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
