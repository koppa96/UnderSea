import { ArmyInfoWoCount } from "./actions/ArmyActions.get";

export interface ArmyUnit {
  unitId: number;
  count: number;
  price: number;
}

export interface ArmyState {
  isPostRequesting: boolean;
  isRequesting: boolean;
  isPostSuccessFull: boolean;
  isLoaded: boolean;
  error: string | null;
  units: ArmyInfoWoCount[];
}
export const ArmyInitialState: ArmyState = {
  isPostRequesting: false,
  isPostSuccessFull: false,
  isRequesting: false,
  isLoaded: false,
  error: null,
  units: []
};
