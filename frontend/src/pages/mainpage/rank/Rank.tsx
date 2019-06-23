import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { RankProps } from "./Interface";

export class Rank extends React.Component /*<RankProps>*/ {
  componentDidMount() {
    document.title = title;
  }
  handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();

    console.log(e.target.value);
  };

  render() {
    //  const{ id,place,name, point}=this.props.rank;
    return (
      <div className="main-component rank-width">
        <ComponentHeader title={title} />
        <input
          onChange={this.handleChange}
          className="rank-input"
          placeholder="Felhasználónév"
        />
        <ul className="rank-page">
          {mockData.map(item => (
            <li key={item.id}>
              <span>{item.place}.</span>
              <span>{item.name}</span>
              <span className="rank-point">{item.point}</span>
            </li>
          ))}
        </ul>
      </div>
    );
  }
}
const title: string = "Ranglista";

const mockData = [
  {
    id: 1,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 2,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 3,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 4,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 5,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 6,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 7,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 8,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 9,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 10,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 11,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 12,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 13,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 14,
    name: "Zátorvány",
    point: 30000,
    place: 1
  },
  {
    id: 15,
    name: "Zátorvány",
    point: 30000,
    place: 1
  }
];
