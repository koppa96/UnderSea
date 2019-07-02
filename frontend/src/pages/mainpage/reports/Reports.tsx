import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ReportsProps } from "./Interface";

export class Reports extends React.Component<ReportsProps> {
  componentDidMount() {
    document.title = title;
    this.props.getAllReports();
  }

  render() {
    const {
      error,
      isRequesting,
      isLoaded,
      isPostRequesting,
      report
    } = this.props.reports;
    return (
      <div className="main-component reports-width">
        <ComponentHeader title={title} />
        <ul className="report-page">
          {isRequesting ? (
            <span>Betöltés...</span>
          ) : (
            report.map(item => (
              <li className={item.isWon ? "won-color" : "lost-color"}>
                <head>
                  <span className="round">{item.round}. kör</span>
                  <span className="is-won">
                    {item.isWon ? "Nyert" : "Vesztett"}
                  </span>
                  <span className="report-enemy-name">
                    {item.enemyCountryName
                      ? item.enemyCountryName
                      : "Nem található név"}{" "}
                    ellen
                  </span>
                </head>
                <div className="report-info">
                  <div>
                    <span className="report-units">Egységeid</span>
                    {item.yourUnits ? (
                      item.yourUnits.map(x => (
                        <span>
                          {x.name} {x.totalCount} db
                        </span>
                      ))
                    ) : (
                      <span>Nincs egység</span>
                    )}
                  </div>
                  <div>
                    <span className="report-units">Ellenfél egység</span>
                  </div>
                </div>
                <div className="report-loses">
                  <p>Veszteség:</p>
                  <ul>
                    {item.lostUnits &&
                      item.lostUnits.map(x => (
                        <span>
                          {x.name} {x.totalCount} db
                        </span>
                      ))}
                  </ul>
                </div>
                <div className="report-total">
                  <p>Totál:</p>
                  <ul>
                    {item.lostUnits &&
                      item.lostUnits.map(x => (
                        <span>
                          {x.name} {x.totalCount} db
                        </span>
                      ))}
                  </ul>
                </div>
              </li>
            ))
          )}
        </ul>
      </div>
    );
  }
}
const title: string = "Csatajelentés";
