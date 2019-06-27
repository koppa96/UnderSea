import * as React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { AttackItem } from "./attackItem";

export class Attack extends React.Component {
  render() {
    return (
      <div className="main-component">
        <ComponentHeader title={title} />
        <div className="attack-page hide-scroll">
          <div>
            <span>1. Jelöld ki, kit szeretnél megtámadni:</span>
            <input className="rank-input" placeholder="Felhasználónév" />
            <ul className="rank-page">
              {mockData.map(item => (
                <li key={item.id}>
                  <span>{item.name}</span>
                  {item.checked && <div className="circle">.</div>}
                </li>
              ))}
            </ul>
          </div>

          <div>
            <span>2. Állítsd be, kiket küldesz harcba:</span>
            <AttackItem />
          </div>
        </div>
        <button>Megtámadom!</button>
      </div>
    );
  }
}
const title: string = "Támadás";

const mockData = [
  {
    id: 1,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 2,
    name: "Zátorvány",
    checked: true
  },
  {
    id: 3,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 4,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 5,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 6,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 7,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 8,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 9,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 10,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 11,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 12,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 13,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 14,
    name: "Zátorvány",
    checked: false
  },
  {
    id: 15,
    name: "Zátorvány",
    checked: false
  }
];
