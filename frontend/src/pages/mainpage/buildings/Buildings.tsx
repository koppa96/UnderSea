import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { BuildingItem } from "./buildingItem";
import { BuildingProps } from "./Interface";

export class Buildings extends React.Component<BuildingProps> {
  componentDidMount() {
    document.title = title;
    console.log("Building mounted");
    if (!this.props.ownedBuildingState.isLoaded) {
      this.props.getAllBuilding();
    }
  }
  state = {
    id: -1,
    cost: 0,
    selectedBuilding: false,
    exceedMoney: false
  };

  addBuildingByID = (e: React.MouseEvent<HTMLInputElement>) => {
    const currentValue = e.currentTarget.value;
    const { count, ownedBuildingState, totalpearl } = this.props;
    const sendBuilding = count.find(x => x.id === +currentValue);

    console.log("Kiválasztott épület: ", sendBuilding);

    const checkMoney = ownedBuildingState.buildings.find(
      item => item.id === +currentValue
    );
    var tempMoneyChecker = false;
    if (!checkMoney || checkMoney.cost > totalpearl) {
      tempMoneyChecker = true;
    }
    this.setState({
      id: sendBuilding ? sendBuilding.id : -1,
      selectedBuilding: true,
      exceedMoney: tempMoneyChecker
    });
  };

  beginAddBuilding() {
    const { addBuilding, ownedBuildingState, totalpearl } = this.props;
    const costOfBuilding = ownedBuildingState.buildings.find(
      item => item.id == this.state.id
    );

    console.log(costOfBuilding, "costofBuiilding undefined");
    console.log(costOfBuilding && costOfBuilding.cost, "costofBuiilding cost");

    if (costOfBuilding && costOfBuilding.cost <= totalpearl) {
      console.log("eljutottam ide");
      addBuilding({ id: this.state.id, cost: costOfBuilding.cost });
    }
  }

  render() {
    const { addBuilding, ownedBuildingState, totalpearl, count } = this.props;
    const error = ownedBuildingState.error && ownedBuildingState.error;

    const tempProgress = count.find(item => item.inProgress === true);

    return (
      <div className="main-component">
        <ComponentHeader
          title={title}
          mainDescription={mainDescription}
          description={description}
        />
        <div className="building-page hide-scroll">
          {ownedBuildingState.isRequesting && <span>Betöltés...</span>}

          {ownedBuildingState.isLoaded &&
            ownedBuildingState.buildings.length > 0 &&
            ownedBuildingState.buildings.map(item => {
              const curentCount = count.find(c => c.id === item.id);
              return (
                <label key={item.id}>
                  {totalpearl >= item.cost && (
                    <input
                      onClick={e => this.addBuildingByID(e)}
                      value={item.id}
                      name="select"
                      className="sr-only"
                      type="radio"
                    />
                  )}
                  <BuildingItem
                    id={item.id}
                    amount={curentCount ? curentCount.count : 0}
                    title={item.name ? item.name : ""}
                    price={item.cost + " Gyöngy/darab"}
                    description={
                      item.description
                        ? item.description
                        : "Nem tartozik leírás"
                    }
                    imageUrl={item.imageUrl ? item.imageUrl : ""}
                  />
                </label>
              );
            })}
        </div>
        <span>{error && error}</span>
        <button
          disabled={
            !this.state.selectedBuilding || ownedBuildingState.isPostRequesting
          }
          onClick={() => this.beginAddBuilding()}
        >
          {tempProgress
            ? "Épül"
            : ownedBuildingState
            ? ownedBuildingState.isPostRequesting
              ? "Töltés"
              : "Megveszem"
            : "Hiba"}
        </button>
      </div>
    );
  }
}
/*
                
                />*/

const title: string = "Épületek";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
const description: string = "Egyszerre csak egy épület épülhet!";
