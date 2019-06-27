import { ICreationInfo } from "../../../../api/Client";

export interface BuildingState {
  buildings: ICreationInfo[];
  loading: boolean;
  error?: string;
}

export const buildingInitialState: BuildingState = {
  buildings: [],
  loading: false
};
