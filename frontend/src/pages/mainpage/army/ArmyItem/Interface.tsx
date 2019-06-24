export interface ArmyItemProps {
  solider: {
    id: number;
    imageUrl: string;
    title: string;
    amount: number;
    stat: string;
    price: string;
    price2: string;
    price3: string;
  };

  currentTrops: Function;
}
