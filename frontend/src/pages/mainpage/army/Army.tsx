import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ArmyItem } from "./ArmyItem";
import { ArmyProps } from "./Interface";

interface Unit {
  unitId: number;
  count: number;
}
interface InitialState {
  units: Unit[];
  unitsAdded: boolean;
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
    unitsAdded: false
  };

  currentSoldiers = (id: number, troop: number) => {
    const temp = this.state.units;
    const index = this.state.units.findIndex(unit => unit.unitId == id);
    if (index !== undefined && index != -1) {
      temp[index].count = troop;
    } else {
      temp.push({ unitId: id, count: troop });
    }

    this.setState({ units: temp });
    if (temp.some(item => item.count != 0)) {
      this.setState({ unitsAdded: true });
    } else {
      this.setState({ unitsAdded: false });
    }
  };

  render() {
    const { addUnits, ownedUnitState } = this.props;
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
                  count={item.count}
                  currentTroops={this.currentSoldiers}
                />
              ))}
          </div>
        )}

        <button
          disabled={!this.state.unitsAdded || ownedUnitState.isPostRequesting}
          onClick={() => addUnits({ unitsToAdd: this.state.units })}
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
