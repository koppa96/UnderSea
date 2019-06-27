import { ArmyItemResponse } from "../ArmyItem/Interface";

export interface ArmyUnit {
  unitId: number;
  count: number;
}

export interface ArmyState {
  isPostRequesting: boolean;
  isRequesting: boolean;
  isLoaded: boolean;
  error: string | null;
  units: ArmyItemResponse[];
}
export const ArmyInitialState: ArmyState = {
  isPostRequesting: false,
  isRequesting: false,
  isLoaded: false,
  error: null,
  units: []
};
