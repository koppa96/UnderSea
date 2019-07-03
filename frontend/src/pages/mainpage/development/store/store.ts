import { ICreationInfo } from "../../../../api/Client";

export interface DevelopmentState {
  isPostRequesting: boolean;
  development: ICreationInfo[];
  loading: boolean;
  error?: string;
}

export const developmentInitialState: DevelopmentState = {
  isPostRequesting: false,
  development: [],
  loading: false
};
