import React from "react";
import { ComponentHeader } from "../componentHeader";
import { ArmyItem } from "./ArmyItem";

export class Army extends React.Component {
  componentWillMount() {
    document.title = title;
  }

  render() {
    return (
      <div className="main-component">
        <ComponentHeader title={title} mainDescription={mainDescription} />
        <div className="building-page hide-scroll" />
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
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    amount: "1 db",
    price: "45 Gyöngy / db"
  },
  {
    id: 2,
    imageUrl: "Rohamfóka",
    title: "Áramlásirányító",
    description: "200 egység nyújt szállást",
    amount: "0 db",
    price: "35 Gyöngy / db"
  },
  {
    id: 3,
    imageUrl: "asdasd",
    title: "Csatacsikó",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    amount: "1 db",
    price: "45 Gyöngy / db"
  }
];
