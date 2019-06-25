import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { BuildingItem } from "./buildingItem";
import { BuildingProps } from "./Interface";

export class Buildings extends React.Component<BuildingProps> {
  componentDidMount() {
    document.title = title;
  }
  state = {
    buildIDs: []
  };

  addBuildingByID = (e: React.MouseEvent<HTMLInputElement>) => {
    const iDs = this.state.buildIDs;
    const currentValue = e.currentTarget.value;

    if (iDs.find(x => x == currentValue)) {
      const newIds = iDs.filter(x => x !== currentValue);

      this.setState({
        buildIDs: newIds
      });
    } else {
      this.setState({
        buildIDs: [...this.state.buildIDs, currentValue]
      });
    }
  };

  render() {
    const { addBuilding, boughtBuildingState } = this.props;
    return (
      <div className="main-component">
        <ComponentHeader
          title={title}
          mainDescription={mainDescription}
          description={description}
        />
        <div className="building-page hide-scroll">
          {boughtBuildingState.buildings.length > 0 &&
            boughtBuildingState.buildings.map(item => (
              <label key={item.id}>
                <input
                  onClick={this.addBuildingByID}
                  value={item.id}
                  className="sr-only"
                  type="checkbox"
                />
                <BuildingItem
                  id={item.id}
                  amount={item.amount}
                  title={item.title}
                  price={item.price}
                  description={item.description}
                  imageUrl={item.imageUrl}
                />
              </label>
            ))}
        </div>
        <button
          onClick={() => addBuilding({ buildingIDs: this.state.buildIDs })}
        >
          Megveszem
        </button>
      </div>
    );
  }
}

const title: string = "Épületek";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
const description: string = "Egyszerre csak egy épület épülhet!";
