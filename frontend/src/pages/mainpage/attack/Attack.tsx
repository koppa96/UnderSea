import * as React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { AttackItem } from "./attackItem";
import { TargetProps } from "./interface";

export class Attack extends React.Component<TargetProps> {
  componentDidMount() {
    document.title = "Támadás";
    this.props.getTargets();
  }

  render() {
    const { targets, unit } = this.props;
    return (
      <div className="main-component">
        <ComponentHeader title={title} />
        <div className="attack-page hide-scroll">
          <div>
            <span>1. Jelöld ki, kit szeretnél megtámadni:</span>
            <input className="rank-input" placeholder="Felhasználónév" />
            <ul className="rank-page">
              {targets.map(item => (
                <li key={item.countryId}>
                  <span>{item.countryName}</span>
                  ide
                </li>
              ))}
            </ul>
          </div>

          <div>
            <span>2. Állítsd be, kiket küldesz harcba:</span>
            {unit.map(item => (
              <AttackItem
                id={item.id}
                imageUrl={item.imageUrl}
                name={item.name}
                defendingCount={item.defendingCount}
                totalCount={item.totalCount}
              />
            ))}
          </div>
        </div>
        <button>Megtámadom!</button>
      </div>
    );
  }
}
const title: string = "Támadás";
/* {item.checked && <div className="circle">.</div>}*/
