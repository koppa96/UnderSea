import React from "react";
import { IBriefUnitInfo } from "../../../../api/Client";
import { BasePortUrl } from "../../../..";

export class AttackItem extends React.Component<IBriefUnitInfo> {
  state = {
    value: 0
  };
  render() {
    const { defendingCount, id, imageUrl, name } = this.props;
    return (
      <div className="attack-item">
        <div className="rectangle">
          <img alt="." src={BasePortUrl + imageUrl} />
        </div>
        <div className="attack-description">
          <span>
            {name}: {this.state.value} példány
          </span>

          <input
            onChange={e => this.setState({ value: e.target.value })}
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
