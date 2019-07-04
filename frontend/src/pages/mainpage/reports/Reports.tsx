import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ReportsProps } from "./Interface";
import { ICombatInfo } from "./store/actions/ReportAction.get";
import Deletemark from "./../../../assets/images/x.svg";
import Shield from "./../../../assets/images/shield.svg";
import Sword from "./../../../assets/images/sword.svg";

export class Reports extends React.Component<ReportsProps> {
  componentDidMount() {
    document.title = title;
    this.props.getAllReports();
  }
  state = {
    openId: null
  };

  reportHandle = (report: ICombatInfo) => {
    if (report.id !== this.state.openId) {
      this.setState({ openId: report.id });
    } else {
      this.setState({ openId: null });
    }
    if (!report.isSeen) {
      this.props.seenReport(report.id);
    }
  };

  render() {
    const { isRequesting, report } = this.props.reports;
    return (
      <div className="main-component reports-width">
        <ComponentHeader title={title} />
        <ul className="report-page">
          {isRequesting ? (
            <div className="loading-circle loading-button" />
          ) : (
            report &&
            report.map(item => (
              <div key={item.id} className="report-container">
                <div
                  title="Törlés"
                  onClick={() =>
                    this.props.deleteReport({
                      id: item.id,
                      isSeen: item.isSeen
                    })
                  }
                  className="circle delete-mark"
                >
                  <img alt="del" src={Deletemark} />
                </div>

                <li
                  title="Kattints"
                  onClick={() => this.reportHandle(item)}
                  className={
                    item.isWon
                      ? item.isSeen
                        ? "won-seen-color"
                        : "won-notseen-color"
                      : item.isSeen
                      ? "lost-seen-color"
                      : "lost-notseen-color"
                  }
                >
                  <div className="head">
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
                  </div>
                  {this.state.openId === item.id && (
                    <>
                      <div className="report-info">
                        {item.isAttack ? (
                          <img src={Sword} alt="Támadás" />
                        ) : (
                          <img src={Shield} alt="Védés" />
                        )}
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
                          {item.enemyUnits ? (
                            item.enemyUnits.map(x => (
                              <span key={x.id}>
                                {x.name} {x.totalCount} db
                              </span>
                            ))
                          ) : (
                            <span>Nincs egység</span>
                          )}
                        </div>
                      </div>
                      <div className="report-loses">
                        <p>Veszteség:</p>
                        <ul>
                          {item.lostUnits &&
                            item.lostUnits.map(x => (
                              <span key={x.id}>
                                {x.name} {x.totalCount} db
                              </span>
                            ))}
                        </ul>
                      </div>
                      <div className="report-total">
                        <p>Totál:</p>
                        <ul>
                          <span>Koral fosztás: {item.coralLoot}</span>
                          <span>Gyöngy fosztás: {item.pealLoot}</span>
                        </ul>
                      </div>
                    </>
                  )}
                </li>
              </div>
            ))
          )}
        </ul>
      </div>
    );
  }
}
const title: string = "Csatajelentés";
