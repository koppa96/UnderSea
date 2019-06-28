import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { DevelopmentItem } from "./developmentItem";
import { DevelopmentProps } from "./Interface";

export class Development extends React.Component<DevelopmentProps> {
  componentDidMount() {
    document.title = "Development";
  }
  onChecked(e: React.ChangeEvent<HTMLInputElement>) {
    console.log("sd");
  }

  render() {
    const { development, error, loading } = this.props.totalDevelopment;
    return (
      <div className="main-component">
        <ComponentHeader
          title={title}
          mainDescription={mainDescription}
          description={description}
        />
        <div className="development-page hide-scroll">
          {development.length > 0 &&
            development.map(item => (
              <label key={item.id}>
                <input
                  value={item.id}
                  className="sr-only"
                  type="radio"
                  onChange={e => this.onChecked(e)}
                />
                ideideiideideiieiided
              </label>
            ))}
        </div>
        <button>Megveszem</button>
      </div>
    );
  }
}
/* <DevelopmentItem development={item} />*/
const mockData = [
  {
    id: "1",
    imageUrl: "asdasd",
    title: "Zátorvány",
    description: "növeli a krumpli termesztést 10%-kal",
    time: "1 db",
    isDeveloped: false
  },
  {
    id: "2",
    imageUrl: "Áramlásirányító",
    title: "Áramlásirányító",
    description: "növeli a korall termesztést 15%-kal",
    time: "0 db",
    isDeveloped: false
  },
  {
    id: "3",
    imageUrl: "asdasd",
    title: "Áramlásirányító",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",

    isDeveloped: true
  },
  {
    id: "4",
    imageUrl: "asdasd",
    title: "Áramlásirányító",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    time: "1 db"
  },
  {
    id: "5",
    imageUrl: "asdasd",
    title: "Áramlásirányító",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként"
  },
  {
    id: "6",
    imageUrl: "asdasd",
    title: "Áramlásirányító",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként"
  }
];

const title: string = "Fejlesztések";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
const description: string =
  "Minden fejlesztés 15 kört vesz igénybe, egyszerre csak egy dolog fejleszthető és minden csak egyszer fejleszthető ki (nem lehet két kombájn).";
