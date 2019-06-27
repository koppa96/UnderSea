import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { BuildingItem } from "./buildingItem";
import { BuildingProps } from "./Interface";

export class Buildings extends React.Component<BuildingProps> {
  componentDidMount() {
    document.title = title;
    console.log("Building mounted");
    this.props.getAllBuilding();
  }
  state = {
    buildIDs: {
      id: -1,
      count: -1
    }
  };

  addBuildingByID = (e: React.MouseEvent<HTMLInputElement>) => {
    const currentValue = e.currentTarget.value;
    const { boughtBuildingState } = this.props;
    const sendBuilding =
      boughtBuildingState &&
      boughtBuildingState.find(x => x.id === +currentValue);
    const temp = {
      id: sendBuilding ? sendBuilding.id : -1,
      count: sendBuilding ? sendBuilding.count : -1
    };
    console.log(sendBuilding);

    this.setState({
      buildIDs: temp
    });
  };

  render() {
    const {
      addBuilding,
      boughtBuildingState,

      totalpearl
    } = this.props;
    return (
      <div className="main-component">
        <ComponentHeader
          title={title}
          mainDescription={mainDescription}
          description={description}
        />
        <div className="building-page hide-scroll">
          {boughtBuildingState &&
            boughtBuildingState.map(item => (
              <label key={item.id}>
                {totalpearl >= item.cost && (
                  <input
                    onClick={this.addBuildingByID}
                    value={item.id}
                    name="select"
                    className="sr-only"
                    type="radio"
                  />
                )}
                <BuildingItem
                  id={item.id}
                  amount={item.count}
                  title={item.name ? item.name : ""}
                  price={item.cost + " Gyöngy/darab"}
                  description={
                    item.description ? item.description : "Nem tartozik leírás"
                  }
                  imageUrl={item.imageUrl ? item.imageUrl : ""}
                />
              </label>
            ))}
        </div>
        <button onClick={() => addBuilding(this.state.buildIDs.id)}>
          Megveszem
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
