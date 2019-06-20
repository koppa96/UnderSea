import React from "react";
import { ComponentHeader } from "../componentHeader";

export class War extends React.Component {
  componentWillMount() {
    document.title = "Harc";
  }

  render() {
    return (
      <div className="main-component">
        <ComponentHeader title={title} />
        <ul className="war-page">
          {mockData.map(item => (
            <li key={item.id}>
              <span>{item.title}</span>
              <div>
                {item.trops.map(single => (
                  <div>
                    <span>{single.amount}</span>
                    <span>{single.type}</span>
                  </div>
                ))}
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
  }
];

const title: string = "Harc";
