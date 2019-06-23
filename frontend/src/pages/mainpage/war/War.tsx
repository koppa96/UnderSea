import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";

export class War extends React.Component {
  componentDidMount() {
    document.title = "Harc";
  }

  render() {
    return (
      <div className="main-component war-component">
        <ComponentHeader title={title} />
        <ul className="war-page">
          {mockData.map(item => (
            <li key={item.id}>
              <span className="war-country">{item.title}</span>
              <div className="war-scroll">
                {item.trops.map((single, index) => (
                  <div key={index}>
                    <span className="war-margin">{single.amount}</span>
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
