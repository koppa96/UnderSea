import {
  IGetReportActions,
  GetReportActions,
  ICombatInfo
} from "./actions/ReportAction.get";
import { reportInitialState, ReportState } from "./store";
import {
  IPostReportActions,
  PostReportActions
} from "./actions/ReportAction.post";
import {
  IDeleteReportActions,
  DeleteReportActions
} from "./actions/ReportAction.delete";

export const ReportReducer = (
  state = reportInitialState,
  action: IGetReportActions | IPostReportActions | IDeleteReportActions
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
    case PostReportActions.REQUEST:
      return {
        ...state
      };
    case PostReportActions.SUCCES:
      var tempReport: ICombatInfo[] = [];
      state.report.forEach(item => {
        if (item.id === action.data) {
          item.isSeen = true;
        }
        tempReport.push(item);
      });

      return {
        ...state,
        report: [...tempReport]
      };
    case PostReportActions.ERROR:
      return {
        ...state,
        error: action.error ? action.error : "Beállítási hiba"
      };
    case DeleteReportActions.REQUEST:
      return {
        ...state
      };
    case DeleteReportActions.SUCCES:
      var deleteReport: ICombatInfo[] = state.report.filter(
        item => item.id !== action.data.id
      );

      return {
        ...state,
        report: [...deleteReport]
      };
    case DeleteReportActions.ERROR:
      return {
        ...state,
        error: action.error ? action.error : "Beállítási hiba"
      };
    default:
      const _check: never = action;
      return state;
  }
};
