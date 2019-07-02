export interface removeReport {
  id: number;
  isSeen: boolean;
}
//ACTIONTYPES
export interface IDeleteReportActionsTypes {
  REQUEST: "REPORT_REQUEST_REPORT_DELETE";
  SUCCES: "REPORT_SUCCES_REPORT_DELETE";
  ERROR: "REPORT_ERROR_REPORT_DELETE";
}

export const DeleteReportActions: IDeleteReportActionsTypes = {
  REQUEST: "REPORT_REQUEST_REPORT_DELETE",
  SUCCES: "REPORT_SUCCES_REPORT_DELETE",
  ERROR: "REPORT_ERROR_REPORT_DELETE"
};

//ACTIONHOZ
export interface IActionRequestDeleteReport {
  type: IDeleteReportActionsTypes["REQUEST"];
  params: removeReport;
}

export interface IActionSuccesDeleteReport {
  type: IDeleteReportActionsTypes["SUCCES"];
  data: removeReport;
}

export interface IActionErrorDeleteReport {
  type: IDeleteReportActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IDeleteReportActions =
  | IActionRequestDeleteReport
  | IActionSuccesDeleteReport
  | IActionErrorDeleteReport;

//ACTIONCREATORHOZ
export const DeleteReportActionCreator = (
  params: removeReport
): IActionRequestDeleteReport => ({
  type: DeleteReportActions.REQUEST,
  params
});
export const fetchError = (error?: string): IActionErrorDeleteReport => ({
  type: DeleteReportActions.ERROR,
  error
});
export const fetchSucces = (data: removeReport): IActionSuccesDeleteReport => ({
  type: DeleteReportActions.SUCCES,
  data
});
