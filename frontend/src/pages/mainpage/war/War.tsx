import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { WarProps } from "./Interface";
import Deletemark from "./../../../assets/images/x.svg";

export class War extends React.Component<WarProps> {
  componentDidMount() {
    document.title = title;
    if (!this.props.totalWar.isLoaded) {
      this.props.getAllWar();
    }
  }

  render() {
    const { war, error, isRequesting } = this.props.totalWar;
    console.log("itt az összes kirajzoló harc", war);
    return (
      <div className="main-component war-component">
        <ComponentHeader title={title} />
        <ul className="war-page">
          {isRequesting ? (
            <span>Betöltés...</span>
          ) : (
            war.map(item => (
              <li key={item.id}>
                <span className="war-country">
                  {item.targetCountryName ? (
                    item.targetCountryName
                  ) : (
                    <span>"Hiba a név betöltésnél"</span>
                  )}
                </span>
                <div className="war-scroll">
                  {item.units ? (
                    item.units.map(single => (
                      <div key={single.id}>
                        <span className="war-margin">{single.totalCount}</span>
                        <span>{single.name ? single.name : "egység"}</span>
                      </div>
                    ))
                  ) : (
                    <span>"Nincsenek egységek"</span>
                  )}
                  <div
                    onClick={() => this.props.deleteById(item)}
                    className="circle"
                  >
                    <img alt="del" src={Deletemark} />
                  </div>
                </div>
              </li>
            ))
          )}
        </ul>
        <div />
      </div>
    );
  }
}

const title: string = "Harc";
