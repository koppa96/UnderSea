import { IBriefCreationInfo, ICreationInfo } from "../../../../api/Client";
import { DevelpomentDescription } from "../Interface";

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
