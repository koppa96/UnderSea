import {
  IGetReportActions,
  GetReportActions
} from "./actions/ReportAction.get";
import { reportInitialState, ReportState } from "./store";

export const ReportReducer = (
  state = reportInitialState,
  action: IGetReportActions
): ReportState => {
  switch (action.type) {
    case GetReportActions.REQUEST:
      return {
        ...state,
        isRequesting: true
      };
    case GetReportActions.SUCCES:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        report: action.data
      };
    case GetReportActions.ERROR:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        error: action.error ? action.error : "Beállítási hiba"
      };
    default:
      const check: never = action;
      return state;
  }
};
