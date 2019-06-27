import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { RankProps } from "./Interface";

export class Rank extends React.Component<RankProps> {
  componentDidMount() {
    document.title = title;
    this.props.getAllBuilding();
  }
  handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();

    console.log(e.target.value);
  };

  render() {
    const { totalRank } = this.props;
    return (
      <div className="main-component rank-width">
        <ComponentHeader title={title} />
        <input
          onChange={this.handleChange}
          className="rank-input"
          placeholder="Felhasználónév"
        />
        {totalRank.loading ? (
          <span>Betöltés...</span>
        ) : (
          <ul className="rank-page">
            {totalRank.rank.map(item => (
              <li key={item.name}>
                <span>{item.rank}.</span>
                <span>{item.name}</span>
                <span className="rank-point">{item.score}</span>
              </li>
            ))}
          </ul>
        )}
      </div>
    );
  }
}
const title: string = "Ranglista";
