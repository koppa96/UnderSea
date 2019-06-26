import { ArmyItemProps } from "../ArmyItem/Interface";

export interface ArmyUnit{
    id: number;
    amount: number;
}

export interface ArmyState {
    units: ArmyItemProps[];
}
export const ArmyInitialState: ArmyState = {
    units: [
      { 
        id: 1,
        imageUrl: "asdasd",
        name: "Lézercápa",
        count: 10,
        attackPower: 5,
        defensePower: 5,
        maintenancePearl: 1,
        maintenanceCoral: 10,
        costPearl: 3,
      },
      { 
        id: 2,
        imageUrl: "asdasd",
        name: "Foka",
        count: 15,
        attackPower: 1,
        defensePower: 6,
        maintenancePearl: 1,
        maintenanceCoral: 10,
        costPearl: 3,
      },
    {
      id: 3,
      imageUrl: "asdasd",
      name: "Csatacsikó",
      count: 20,
      attackPower: 5,
      defensePower: 5,
      maintenancePearl: 1,
      maintenanceCoral: 10,
      costPearl: 3
    }
  ]
};


