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
    reset: false,
    filteredrank: initFilter
  };

  componentDidUpdate() {
    if (this.state.reset) {
      this.setState({ reset: false });
    }
  }

  filter = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { targets } = this.props;
    var filter = e.target.value.toLowerCase();
    this.setState({ filtered: filter });
    if (e.target.value.length > 0) {
      var filtered: ITargetInfo[] = [];
      targets.targets.forEach(x => {
        const lowerc = x.username ? x.username.toLowerCase() : "";
        if (lowerc.startsWith(filter)) {
          filtered.push(x);
        }
        this.setState({ filteredrank: filtered });
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
    const buttonState =
      this.props.targets.isRequesting ||
      this.props.targets.isPostRequesting ||
      this.state.targetCountryId < 1 ||
      this.state.units.length < 2;
    const buttonClass = buttonState ? "button-disabled" : "button";
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
                this.state.filtered.length > 0 ? (
                  this.state.filteredrank.map(item => (
                    <li
                      key={item.countryId}
                      onClick={() =>
                        this.setState({ targetCountryId: item.countryId })
                      }
                    >
                      <span>{item.username}</span>
                      <span>
                        {item.countryName ? item.countryName : "Not Found"}
                      </span>
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
                      <span>{item.username}</span>
                      <span>
                        {item.countryName ? item.countryName : "Not Found"}
                      </span>
                      {item.countryId === this.state.targetCountryId && (
                        <div className="circle">
                          <img src={CheckMark} alt="Checkmark" />
                        </div>
                      )}
                    </li>
                  ))
                )
              ) : (
                <div className="loading-circle loading-button" />
              )}
            </ul>
          </div>

          <div>
            <span>2. Állítsd be, kiket küldesz harcba:</span>
            <span>Támadáshoz 1 parancsnokra szükséged van</span>
            {units.map(item => (
              <AttackItem
                key={item.id}
                count={item.count}
                id={item.id}
                imageUrl={item.imageUrl}
                name={item.name}
                defendingCount={item.defendingCount}
                setTrop={this.addUnit}
                reset={this.state.reset}
              />
            ))}
          </div>
        </div>
        <button
          className={buttonClass}
          disabled={buttonState}
          onClick={() => {
            this.props.attackTarget({
              targetCountryId: this.state.targetCountryId,
              units: this.state.units
            });
            this.setState({
              targetCountryId: -1,
              units: initUnit,
              filtered: "",
              filteredrank: initFilter,
              reset: true
            });
          }}
        >
          {this.props.targets.isRequesting ? (
            <div className="loading-circle loading-button" />
          ) : this.props.targets.isPostRequesting ? (
            <div className="loading-circle loading-button" />
          ) : this.state.targetCountryId < 1 || this.state.units.length < 2 ? (
            "Válassz"
          ) : (
            "Megtámadom"
          )}
        </button>
      </div>
    );
  }
}
const title: string = "Támadás";
/* */
