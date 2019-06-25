import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ArmyItem } from "./ArmyItem";
import {  ArmyInitialState, ArmyUnit } from "./store/store";
import { ArmyProps } from "./Interface";


export class Army extends React.Component<ArmyProps> {
  componentDidMount() {
    document.title = title;
  }
  state = {
    troopsToAdd: [
      {
          id:1,
          amount: 0
      },
      {
          id:2,
          amount: 0
      },
      {
          id:3,
          amount: 0
      }
    ]
    //troops: ArmyInitialState
  }

  currentSoldiers = (id: number, troop: number) => {
    var temp = this.state.troopsToAdd;
    temp.map(
      unit => {
        if(unit.id == id)
          unit.amount = troop
      }
    )
    this.setState({troopsToAdd: temp}, () => {
      console.log(this.state.troopsToAdd);
    });
  };

  render() {
    const {addUnits, ownedUnitState} = this.props
    console.log(this.state.troopsToAdd, 'itt');
    return (
      <div className="main-component army-component">
        <ComponentHeader title={title} mainDescription={mainDescription} />
        <div className="army-page hide-scroll">
          {ownedUnitState.units.length > 0 &&
            ownedUnitState.units.map(item => (
              <ArmyItem unit={item} currentTroops={this.currentSoldiers} />
            ))}
        </div>
        <button
        onClick = {() => addUnits({unitsToAdd:this.state.troopsToAdd})}>Megveszem</button>
      </div>
    );
  }
}
const title: string = "Sereg";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";