import { ArmyInfoWoCount } from "../Interface";
export interface ArmyItemResponse {
  id: number;
  name: string;
  imageUrl: string;
  attackPower: number;
  defensePower: number;
  count: number;
  maintenancePearl: number;
  maintenanceCoral: number;
  costPearl: number;
}
export interface ArmyProps {
  unit: ArmyInfoWoCount;
  currentTroops: (id: number, troop: number, price: number) => void;
  count: number;
  reset: boolean;
}
