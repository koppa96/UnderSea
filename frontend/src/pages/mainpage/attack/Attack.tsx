import * as React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { AttackItem } from "./attackItem";
import { TargetProps } from "./interface";
import { ITargetInfo } from "../../../api/Client";
import CheckMark from "./../../../assets/images/check_mark.png";

const initFilter: ITargetInfo[] = [];

export class Attack extends React.Component<TargetProps> {
  componentDidMount() {
    document.title = title;
    if (!this.props.targets.isLoaded) {
      this.props.getTargets();
    }
  }

  state = {
    targetCountryId: -1,
    units: [
      {
        unitId: 1,
        amount: 1
      }
    ],
    filtered: "",

    filteredrank: initFilter
  };

  filter = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { targets } = this.props;
    this.setState({ filtered: e.target.value });
    if (e.target.value.length > 2) {
      var filteredrank: ITargetInfo[] = [];
      targets.targets.forEach(x => {
        if (x.countryName && x.countryName.startsWith(e.target.value)) {
          filteredrank.push(x);
        }
        this.setState({ filteredrank: filteredrank }, () =>
          console.log("filter", this.state.filteredrank)
        );
      });
    }
  };
  render() {
    const { targets, units } = this.props;
    console.log("total targets: ", targets);
    return (
      <div className="main-component">
        <ComponentHeader title={title} />
        <div className="attack-page hide-scroll">
          <div>
            <span>1. Jelöld ki, kit szeretnél megtámadni:</span>
            <input
              onChange={e => this.filter(e)}
              className="rank-input"
              placeholder="Felhasználónév"
            />

            <ul className="rank-page">
              {targets.isLoaded ? (
                this.state.filtered.length > 2 ? (
                  this.state.filteredrank.map(item => (
                    <li
                      key={item.countryId}
                      onClick={() =>
                        this.setState({ targetCountryId: item.countryId })
                      }
                    >
                      <span>{item.countryName}</span>
                      {item.countryId === this.state.targetCountryId && (
                        <div className="circle">
                          <img src={CheckMark} alt="Checkmark" />
                        </div>
                      )}
                    </li>
                  ))
                ) : (
                  targets.targets.map(item => (
                    <li
                      key={item.countryId}
                      onClick={() =>
                        this.setState({ targetCountryId: item.countryId })
                      }
                    >
                      <span>{item.countryName}</span>
                      {item.countryId === this.state.targetCountryId && (
                        <div className="circle">
                          <img src={CheckMark} alt="Checkmark" />
                        </div>
                      )}
                    </li>
                  ))
                )
              ) : (
                <span>Betöltés...</span>
              )}
            </ul>
          </div>

          <div>
            <span>2. Állítsd be, kiket küldesz harcba:</span>
            {units.map(item => (
              <AttackItem
                id={item.id}
                imageUrl={item.imageUrl}
                name={item.name}
                defendingCount={item.defendingCount}
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
/* */
