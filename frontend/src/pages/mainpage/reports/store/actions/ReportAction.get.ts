export interface ICombatInfo {
  id: number;
  round: number;
  isAttack: boolean;
  isWon: boolean;
  enemyCountryId: number;
  enemyCountryName?: string | undefined;
  yourUnits?: IUnitInfo[] | undefined;
  enemyUnits?: IUnitInfo[] | undefined;
  lostUnits?: IUnitInfo[] | undefined;
  pealLoot: number;
  coralLoot: number;
  isSeen: boolean;
}

export interface IUnitInfo {
  id: number;
  name?: string | undefined;
  imageUrl?: string | undefined;
  attackPower: number;
  defensePower: number;
  maintenancePearl: number;
  maintenanceCoral: number;
  costPearl: number;
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
