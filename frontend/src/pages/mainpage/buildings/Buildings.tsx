import React from "react";
import { ComponentHeader } from "../componentHeader";
import { BuildingItem } from "./buildingItem";
import { BuildingProps } from "./Interface";

export class Buildings extends React.Component<BuildingProps> {
  componentWillMount() {
    document.title = "Buildings";
  }
  state = {
    buildIDs: []
  };
  /*
addBuildingById = (e)=>{
this.setState({
  buildIDs:[...this.state.buildIDs, e.target.value]
})
}*/

  render() {
    const { addBuilding, buildingState } = this.props;
    return (
      <div className="main-component">
        <ComponentHeader
          title={title}
          mainDescription={mainDescription}
          description={description}
        />
        <div className="building-page hide-scroll">
          {mockData.length > 0 &&
            mockData.map(item => (
              <label key={item.id}>
                <input value={item.id} className="sr-only" type="checkbox" />
                <BuildingItem building={item} />
              </label>
            ))}
        </div>
        <button onClick={() => addBuilding({ buildingIDs: [1, 2, 3] })}>
          Megveszem
        </button>
      </div>
    );
  }
}

const mockData = [
  {
    id: 1,
    imageUrl: "asdasd",
    title: "Zátorvány",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    amount: "1 db",
    price: "45 Gyöngy / db"
  },
  {
    id: 2,
    imageUrl: "Áramlásirányító",
    title: "Áramlásirányító",
    description: "200 egység nyújt szállást",
    amount: "0 db",
    price: "35 Gyöngy / db"
  },
  {
    id: 3,
    imageUrl: "asdasd",
    title: "Áramlásirányító",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    amount: "1 db",
    price: "45 Gyöngy / db"
  }
];

const title: string = "Épületek";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
const description: string = "Egyszerre csak egy épület épülhet!";
