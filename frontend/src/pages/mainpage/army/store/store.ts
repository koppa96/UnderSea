
export interface ArmyUnit{
    id: number;
    amount: number;
}

export interface ArmyState {
    units:ArmyUnit[]
}

export const initialItems: ArmyState = {
  units: [
    {
        id:1,
        amount: 10
    },
    {
        id:2,
        amount: 25
    },
    {
        id:3,
        amount: 20
    }
]
  };