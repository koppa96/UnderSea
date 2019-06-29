import { ICreationInfo } from "./actions/BuildingAction.get";

export interface BuildingState {
  buildings: ICreationInfo[];
  isPostRequesting: boolean;
  isRequesting: boolean;
  isLoaded: boolean;
  error?: string;
}

export const buildingInitialState: BuildingState = {
  isPostRequesting: false,
  isRequesting: false,
  isLoaded: false,
  buildings: []
};

export interface RequestBuildingParams {
  id: number;
  cost: number;
}
