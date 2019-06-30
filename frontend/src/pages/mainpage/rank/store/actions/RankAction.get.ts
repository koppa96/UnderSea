export interface IRankInfo {
  name?: string | undefined;
  rank: number;
  score?: string | undefined;
}
//ACTIONTYPES
export interface IGetRankActionsTypes {
  REQUEST: "RANK_REQUEST_GET_RANK";
  SUCCES: "RANK_SUCCES_GET_RANK";
  ERROR: "RANK_ERROR_GET_RANK";
}

export const GetRankActions: IGetRankActionsTypes = {
  REQUEST: "RANK_REQUEST_GET_RANK",
  SUCCES: "RANK_SUCCES_GET_RANK",
  ERROR: "RANK_ERROR_GET_RANK"
};

export interface ISuccesParamState {
  ranks: IRankInfo[];
}

//ACTIONHOZ
export interface IRequestActionGetRank {
  type: IGetRankActionsTypes["REQUEST"];
}
export interface ISuccesActionGetRank {
  type: IGetRankActionsTypes["SUCCES"];
  params: ISuccesParamState;
}
export interface IErrorActionGetRank {
  type: IGetRankActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IRankActions =
  | IRequestActionGetRank
  | ISuccesActionGetRank
  | IErrorActionGetRank;

//ACTIONCREATORHOZ
export const GetRankActionCreator = (): IRequestActionGetRank => ({
  type: GetRankActions.REQUEST
});

export const fetchError = (params?: string): IErrorActionGetRank => ({
  type: GetRankActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): ISuccesActionGetRank => ({
  type: GetRankActions.SUCCES,
  params
});
