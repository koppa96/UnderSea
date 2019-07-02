import { IBriefUnitInfo } from "../../../war/store/actions/WarAction.get";

export interface ICombatInfo {
  id: number;
  round: number;
  isAttack: boolean;
  isWon: boolean;
  enemyCountryId: number;
  enemyCountryName?: string | undefined;
  yourUnits?: IBriefUnitInfo[] | undefined;
  enemyUnits?: IBriefUnitInfo[] | undefined;
  lostUnits?: IBriefUnitInfo[] | undefined;
  pealLoot: number;
  coralLoot: number;
  isSeen: boolean;
}



//ACTIONTYPES
export interface IGetReportActionsTypes {
  REQUEST: "RERPORT_REQUEST_GET_RERPORT";
  SUCCES: "RERPORT_SUCCES_GET_RERPORT";
  ERROR: "RERPORT_ERROR_GET_RERPORT";
}

export const GetReportActions: IGetReportActionsTypes = {
  REQUEST: "RERPORT_REQUEST_GET_RERPORT",
  SUCCES: "RERPORT_SUCCES_GET_RERPORT",
  ERROR: "RERPORT_ERROR_GET_RERPORT"
};

//ACTIONHOZ
export interface IRequestActionGetReport {
  type: IGetReportActionsTypes["REQUEST"];
}
export interface ISuccesActionGetReport {
  type: IGetReportActionsTypes["SUCCES"];
  data: ICombatInfo[];
}
export interface IErrorActionGetReport {
  type: IGetReportActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IGetReportActions =
  | IRequestActionGetReport
  | ISuccesActionGetReport
  | IErrorActionGetReport;

//ACTIONCREATORHOZ
export const GetReportRequestActionCreator = (): IRequestActionGetReport => ({
  type: GetReportActions.REQUEST
});

export const GetReportErrorActionCreator = (
  error?: string
): IErrorActionGetReport => ({
  type: GetReportActions.ERROR,
  error
});
export const GetReportSuccessActionCreator = (
  data: ICombatInfo[]
): ISuccesActionGetReport => ({
  type: GetReportActions.SUCCES,
  data
});
