import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { RankProps } from "./Interface";
import { IRankInfo } from "../../../api/Client";

const initFilter: IRankInfo[] = [];

export class Rank extends React.Component<RankProps> {
  componentDidMount() {
    document.title = title;
    if (!this.props.totalRank.isLoaded) {
      this.props.getAllBuilding();
    }
  }

  state = {
    filtered: "",
    filteredrank: initFilter
  };

  filter = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { totalRank } = this.props;
    const filter = e.target.value.toLowerCase();
    this.setState({ filtered: filter });
    if (e.target.value.length > 1) {
      var filteredrank: IRankInfo[] = [];
      totalRank.rank.forEach(x => {
        const name = x.name ? x.name.toLowerCase() : "";
        if (name.startsWith(filter)) {
          filteredrank.push(x);
        }
        this.setState({ filteredrank: filteredrank });
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

        {totalRank.isRequesting ? (
          <span>Betöltés...</span>
        ) : this.state.filtered.length > 1 ? (
          <ul className="rank-page">
            {this.state.filteredrank.map(item => (
              <li key={item.rank}>
                <span>{item.rank}.</span>
                <span>{item.name}</span>
                <span className="rank-point">{item.score}</span>
              </li>
            ))}
          </ul>
        ) : (
          <ul className="rank-page">
            {totalRank.rank.map(item => (
              <li key={item.rank}>
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
