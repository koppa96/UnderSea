import { ICreationInfo } from "../../../../api/Client";

export interface BuildingState {
  isPostRequesting: boolean;
  buildings: ICreationInfo[];
  loading: boolean;
  error?: string;
}

export const buildingInitialState: BuildingState = {
  isPostRequesting: false,
  buildings: [],
  loading: false
};
