import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ArmyItem } from "./ArmyItem";
import {  ArmyInitialState, ArmyUnit } from "./store/store";
import { ArmyProps } from "./Interface";

const initialState = {
  troopsToAdd: [
    {
        unitId:1,
        count: 0
    },
    {
        unitId:2,
        count: 0
    },
    {
        unitId:3,
        count: 0
    }
  ]
}


export class Army extends React.Component<ArmyProps> {
  componentDidMount() {
    document.title = title;
  }
  state = {
    ...initialState,
    unitsAdded: false
    //troops: ArmyInitialState
  }

  currentSoldiers = (id: number, troop: number) => {
    const newtTemp = this.state.troopsToAdd.map(
      unit => {
        
        if(unit.unitId == id){
          return {...unit, count: troop}
        }
        return unit;
      }
    )
    this.setState({troopsToAdd: newtTemp}, () => {
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
        onClick = {() => addUnits({unitsToAdd: this.state.troopsToAdd})}>Megveszem</button>
      </div>
    );
  }
}
const title: string = "Sereg";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";