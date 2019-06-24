import { BuildingProps } from "../buildingItem/Interface";

export interface BuildingState {
  buildings: BuildingProps[];
}

export const buildingInitialState: BuildingState = {
  buildings: [
    {
      id: 1,
      imageUrl: "asdasd",
      title: "Zátorvány",
      description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
      amount: 1,
      price: "45 Gyöngy / db"
    },
    {
      id: 2,
      imageUrl: "Áramlásirányító",
      title: "Áramlásirányító",
      description: "200 egység nyújt szállást",
      amount: 2,
      price: "35 Gyöngy / db"
    },
    {
      id: 3,
      imageUrl: "asdasd",
      title: "Áramlásirányító",
      description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
      amount: 3,
      price: "45 Gyöngy / db"
    }
  ]
};

export interface alma {
  asd: {
    ka: string;
  };
}
const mockData = [
  {
    id: 1,
    imageUrl: "asdasd",
    title: "Zátorvány",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    amount: 1,
    price: "45 Gyöngy / db"
  },
  {
    id: 2,
    imageUrl: "Áramlásirányító",
    title: "Áramlásirányító",
    description: "200 egység nyújt szállást",
    amount: 2,
    price: "35 Gyöngy / db"
  },
  {
    id: 3,
    imageUrl: "asdasd",
    title: "Áramlásirányító",
    description: "50 ember-t ad a népességhez 200 krumplit termel körönként",
    amount: 3,
    price: "45 Gyöngy / db"
  }
];
