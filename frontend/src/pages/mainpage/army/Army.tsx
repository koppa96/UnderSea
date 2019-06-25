import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ArmyItem } from "./ArmyItem";
import { initialItems } from "./Interface";

export class Army extends React.Component {
  componentDidMount() {
    document.title = title;
  }

  state = {
    trops: initialItems
  };

  currentSoliders = (id: string, trop: number) => {
    const asd = this.state.trops;
    const filtered = asd.filter(x => x.id !== id);
    this.setState({ trops: [...filtered, { id: id, amount: trop }] }, () => {
      console.log(this.state);
    });
  };

  render() {
    console.log("Army rendered")
    return (
      <div className="main-component army-component">
        <ComponentHeader title={title} mainDescription={mainDescription} />
        <div className="army-page hide-scroll">
          {mockData.length > 0 &&
            mockData.map(item => (
              <ArmyItem solider={item} currentTrops={this.currentSoliders} />
            ))}
        </div>
        <button>Megveszem</button>
      </div>
    );
  }
}
const title: string = "Sereg";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";

const mockData = [
  {
    id: 1,
    imageUrl: "asdasd",
    title: "Lézercápa",
    amount: 10,
    stat: "1/2",
    price: "45 Gyöngy / db",
    price2: "45 Gyöngy / db",
    price3: "45 Gyöngy / db"
  },
  {
    id: 2,
    imageUrl: "Rohamfóka",
    title: "Áramlásirányító",
    amount: 3,
    stat: "1/2",
    price: "45 Gyöngy / db",
    price2: "45 Gyöngy / db",
    price3: "45 Gyöngy / db"
  },
  {
    id: 3,
    imageUrl: "asdasd",
    title: "Csatacsikó",
    amount: 2,
    stat: "1/2",
    price: "45 Gyöngy / db",
    price2: "45 Gyöngy / db",
    price3: "45 Gyöngy / db"
  }
];
