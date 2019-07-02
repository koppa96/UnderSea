//ACTIONTYPES
export interface IPostReportActionsTypes {
  REQUEST: "REPORT_REQUEST_REPORT_SEEN";
  SUCCES: "REPORT_SUCCES_REPORT_SEEN";
  ERROR: "REPORT_ERROR_REPORT_SEEN";
}

export const PostReportActions: IPostReportActionsTypes = {
  REQUEST: "REPORT_REQUEST_REPORT_SEEN",
  SUCCES: "REPORT_SUCCES_REPORT_SEEN",
  ERROR: "REPORT_ERROR_REPORT_SEEN"
};

//ACTIONHOZ
export interface IActionRequestPostReport {
  type: IPostReportActionsTypes["REQUEST"];
  params: number;
}

export interface IActionSuccesPostReport {
  type: IPostReportActionsTypes["SUCCES"];
  data: number;
}

export interface IActionErrorPostReport {
  type: IPostReportActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IPostReportActions =
  | IActionRequestPostReport
  | IActionSuccesPostReport
  | IActionErrorPostReport;

//ACTIONCREATORHOZ
export const PostReportActionCreator = (
  params: number
): IActionRequestPostReport => ({
  type: PostReportActions.REQUEST,
  params
});
export const fetchError = (error?: string): IActionErrorPostReport => ({
  type: PostReportActions.ERROR,
  error
});
export const fetchSucces = (data: number): IActionSuccesPostReport => ({
  type: PostReportActions.SUCCES,
  data
});
