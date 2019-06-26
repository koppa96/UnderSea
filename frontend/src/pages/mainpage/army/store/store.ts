import { ArmyItemProps } from "../ArmyItem/Interface";

export interface ArmyUnit{
    id: number;
    amount: number;
}

export interface ArmyState {
    units: ArmyItemProps[];
}
export const ArmyInitialState: ArmyState = {
    units: [{ 
        id: 1,
        imageUrl: "asdasd",
        title: "Lézercápa",
        amount: 10,
        stat: "1/2",
        price: "45 Gyöngy / db",
        price2: "45 Gyöngy / db",
        price3: "45 Gyöngy / db",
      },
    {
      id: 2,
      imageUrl: "Rohamfóka",
      title: "Áramlásirányító",
      amount: 25,
      stat: "1/2",
      price: "45 Gyöngy / db",
      price2: "45 Gyöngy / db",
      price3: "45 Gyöngy / db"
    },
    {
      id: 3,
      imageUrl: "asdasd",
      title: "Csatacsikó",
      amount: 20,
      stat: "1/2",
      price: "45 Gyöngy / db",
      price2: "45 Gyöngy / db",
      price3: "45 Gyöngy / db"
    }
  ]
};


