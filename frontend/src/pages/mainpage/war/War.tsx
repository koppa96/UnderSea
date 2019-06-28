import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { WarProps } from "./Interface";

export class War extends React.Component<WarProps> {
  componentDidMount() {
    document.title = "Harc";
    this.props.getAllWar();
  }

  render() {
    const { war, error, loading } = this.props.totalWar;
    return (
      <div className="main-component war-component">
        <ComponentHeader title={title} />
        <ul className="war-page">
          {error
            ? error
            : war.map(item => (
                <li key={item.id}>
                  <span className="war-country">
                    {item.targetCountryName
                      ? item.targetCountryName
                      : "Hiba a név betöltésnél"}
                  </span>
                  <div className="war-scroll">
                    {item.units
                      ? item.units.map(single => (
                          <div key={single.id}>
                            <span className="war-margin">
                              {single.totalCount}
                            </span>
                            <span>{single.name ? single.name : "egység"}</span>
                          </div>
                        ))
                      : "Nincsenek egységek"}
                  </div>
                </li>
              ))}
        </ul>
        <div />
      </div>
    );
  }
}

const mockData = [
  {
    id: "1",
    title: "Zátorvány",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "2",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "3",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "3",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "3",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "3",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "3",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  },
  {
    id: "3",
    title: "Áramlásirányító",
    trops: [
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      },
      {
        type: "Csatacsikó",
        amount: 3
      }
    ]
  }
];

const title: string = "Harc";
