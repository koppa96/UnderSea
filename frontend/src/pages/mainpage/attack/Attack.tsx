import * as React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { AttackItem } from "./attackItem";
import { TargetProps, IUnitDetails } from "./interface";
import { ITargetInfo } from "../../../api/Client";
import CheckMark from "./../../../assets/images/check_mark.png";

const initFilter: ITargetInfo[] = [];
const initUnit: IUnitDetails[] = [];

export class Attack extends React.Component<TargetProps> {
  componentDidMount() {
    document.title = title;
    if (!this.props.targets.isLoaded) {
      this.props.getTargets();
    }
  }

  state = {
    targetCountryId: -1,
    units: initUnit,
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

  addUnit = (id: number, count: number) => {
    const filteredUnit = this.state.units.filter(item => item.unitId !== id);
    if (count <= 0) {
      this.setState({ units: [...filteredUnit] });
    } else {
      this.setState({
        units: [...filteredUnit, { unitId: id, amount: count }]
      });
    }
  };
  render() {
    const { targets, units } = this.props;
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
            <span>Támadáshoz 1 parancsnokra szükséged van</span>
            {units.map(item => (
              <AttackItem
                id={item.id}
                imageUrl={item.imageUrl}
                name={item.name}
                defendingCount={item.defendingCount}
                setTrop={this.addUnit}
              />
            ))}
          </div>
        </div>
        <button
          disabled={
            this.state.targetCountryId < 1 || this.state.units.length < 2
          }
          onClick={() =>
            this.props.attackTarget({
              targetCountryId: this.state.targetCountryId,
              units: this.state.units
            })
          }
        >
          {this.state.targetCountryId < 1 || this.state.units.length < 2
            ? "Válassz"
            : "Megtámadom"}
        </button>
      </div>
    );
  }
}
const title: string = "Támadás";
/* */
