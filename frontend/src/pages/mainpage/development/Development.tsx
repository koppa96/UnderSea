import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { DevelopmentItem } from "./developmentItem";
import { DevelopmentProps } from "./Interface";

export class Development extends React.Component<DevelopmentProps> {
  componentDidMount() {
    document.title = "Development";
    this.props.getAllDevelopment();
  }

  state = {
    id: -1
  };

  completed = false;

  render() {
    const { totalDevelopment, totalResourcesDesc } = this.props;
    var inProgress = false;
    this.props.totalResourcesDesc.forEach(dev => {
      if (dev.inProgressCount > 0) {
        inProgress = true;
      }
    });

    const buttonState =
      this.state.id === -1 ||
      this.props.totalDevelopment.isPostRequesting ||
      this.props.totalDevelopment.loading ||
      inProgress ||
      this.completed;
    const buttonClass = buttonState ? "button-disabled" : "button";
    return (
      <div className="main-component">
        <ComponentHeader
          title={title}
          mainDescription={mainDescription}
          description={description}
        />
        <div className="development-page hide-scroll">
          {totalResourcesDesc.map(item => {
            const mappedDeveleopmnet = totalDevelopment.development.find(
              x => x.id === item.id
            );
            return (
              <label key={item.id}>
                <input
                  value={item.id}
                  className="sr-only"
                  type="radio"
                  name="select"
                  onChange={e => {
                    this.setState({ id: e.target.value });
                    this.props.totalResourcesDesc.forEach(dev => {
                      if (+e.target.value === dev.id) {
                        if (dev.count > 0) {
                          this.completed = true;
                        } else {
                          this.completed = false;
                        }
                      }
                    });
                    console.log(this.completed);
                  }}
                />
                <DevelopmentItem
                  count={item.count}
                  inProgress={item.inProgressCount}
                  info={mappedDeveleopmnet ? mappedDeveleopmnet : null}
                />
              </label>
            );
          })}
        </div>
        <button
          onClick={() => this.props.addDevelopment(this.state.id)}
          disabled={buttonState}
          className={buttonClass}
        >
          Elkezdem
        </button>
      </div>
    );
  }
}

const title: string = "Fejlesztések";
const mainDescription: string = "Kattints rá, amelyiket szeretnéd megvenni.";
const description: string =
  "Minden fejlesztés 15 kört vesz igénybe, egyszerre csak egy dolog fejleszthető és minden csak egyszer fejleszthető ki (nem lehet két kombájn).";
