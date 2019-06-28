import { IBriefCreationInfo } from "../../../../api/Client";

export interface DevelopmentState {
  isPostRequesting: boolean;
  development: IBriefCreationInfo[];
  loading: boolean;
  error?: string;
}

export const buildingInitialState: DevelopmentState = {
  isPostRequesting: false,
  development: [],
  loading: false
};
