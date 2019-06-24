import React from "react";

export class AttackItem extends React.Component {
  render() {
    return (
      <div className="attack-item">
        <div className="rectangle">
          <img alt="." />
        </div>
        <div className="attack-description">
          <span>Lézercápa: 20 példány</span>

          <input type="range" min="1" max="100" />
        </div>
      </div>
    );
  }
}
