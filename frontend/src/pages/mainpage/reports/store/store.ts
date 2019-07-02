import { ICombatInfo } from "./actions/ReportAction.get";

export interface ReportState {
  report: ICombatInfo[];
  isPostRequesting: boolean;
  isRequesting: boolean;
  isLoaded: boolean;
  error?: string;
}

export const reportInitialState: ReportState = {
  report: [],
  isPostRequesting: false,
  isRequesting: false,
  isLoaded: false
};
