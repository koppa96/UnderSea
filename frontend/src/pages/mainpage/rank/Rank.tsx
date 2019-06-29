import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { RankProps } from "./Interface";
import { IRankInfo } from "../../../api/Client";

const initFilter: IRankInfo[] = [];

export class Rank extends React.Component<RankProps> {
  componentDidMount() {
    document.title = title;
    this.props.getAllBuilding();
  }

  state = {
    filtered: "",
    filteredrank: initFilter
  };

  filter = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { totalRank } = this.props;
    this.setState({ filtered: e.target.value });
    if (e.target.value.length > 2) {
      var filteredrank: IRankInfo[] = [];
      totalRank.rank.forEach(x => {
        if (x.name && x.name.startsWith(e.target.value)) {
          filteredrank.push(x);
        }
        this.setState({ filteredrank: filteredrank }, () =>
          console.log("filter", this.state.filteredrank)
        );
      });
    }
  };

  render() {
    const { totalRank } = this.props;

    return (
      <div className="main-component rank-width">
        <ComponentHeader title={title} />
        <input
          onChange={e => this.filter(e)}
          className="rank-input"
          placeholder="Felhasználónév"
        />

        {totalRank.loading ? (
          <span>Betöltés...</span>
        ) : this.state.filtered.length > 2 ? (
          <ul className="rank-page">
            {this.state.filteredrank.map(item => (
              <li key={item.name}>
                <span>{item.rank}.</span>
                <span>{item.name}</span>
                <span className="rank-point">{item.score}</span>
              </li>
            ))}
          </ul>
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
