import React from "react";
import { BasePortUrl } from "../../../..";
import { defendingTrop } from "../interface";
import Slider from "@material-ui/lab/Slider";

import QuestionMark from "./../../../../assets/images/question.svg";
import { withStyles } from "@material-ui/styles";
const PrettoSlider = withStyles({
  root: {
    color: "#9ffff0",
    height: 8
  },
  thumb: {
    height: 15,
    width: 15,
    backgroundColor: "#9ffff0"
  },
  track: {
    height: 8,
    borderRadius: 4
  },
  rail: {
    height: 8,
    borderRadius: 4
  }
})(Slider);

interface Troops {
  reset: boolean;
  imageUrl: string | null;
  id: number;
  defendingCount: number;
  name: string;
  setTrop?: Function;
  count: number;
}

export class AttackItem extends React.Component<Troops> {
  state = {
    value: 0
  };

  componentWillReceiveProps() {
    if (this.props.reset) {
      this.setState({ value: 0 });
    }
  }

  changeUnit = (e: React.ChangeEvent<{}>, value: number | number[]) => {
    this.setState({ value: value });
    this.props.setTrop && this.props.setTrop(this.props.id, value);
  };

  render() {
    const { defendingCount, imageUrl, name } = this.props;
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
          <div className="overflowV">
            <PrettoSlider
              onChange={(e, v) => this.changeUnit(e, v)}
              min={0}
              value={this.state.value}
              max={defendingCount}
            />
          </div>
        </div>
      </div>
    );
  }
}
