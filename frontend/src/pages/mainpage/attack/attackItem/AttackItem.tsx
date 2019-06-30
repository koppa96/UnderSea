import React from "react";
import { IBriefUnitInfo } from "../../../../api/Client";
import { BasePortUrl } from "../../../..";
import { defendingTrop } from "../interface";

import QuestionMark from "./../../../../assets/images/question.svg";
export class AttackItem extends React.Component<defendingTrop> {
  state = {
    value: 0
  };

  changeUnit = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ value: e.currentTarget.value });
    this.props.setTrop &&
      this.props.setTrop(this.props.id, e.currentTarget.value);
  };

  render() {
    const { defendingCount, id, imageUrl, name } = this.props;
    const image = imageUrl ? BasePortUrl + imageUrl : QuestionMark;
    return (
      <div className="attack-item">
        <div className="rectangle">
          <img alt="." src={image} />
        </div>
        <div className="attack-description">
          <span>
            {name}: {this.state.value} példány
          </span>

          <input
            onChange={e => this.changeUnit(e)}
            type="range"
            min="0"
            value={this.state.value}
            max={defendingCount}
          />
        </div>
      </div>
    );
  }
}
