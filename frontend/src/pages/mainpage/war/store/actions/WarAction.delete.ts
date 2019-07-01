//ACTIONTYPES
export interface IDeleteWarActionsTypes {
  REQUEST: "WAR_REQUEST_WAR_DELETE";
  SUCCES: "WAR_SUCCES_WAR_DELETE";
  ERROR: "WAR_ERROR_WAR_DELETE";
}

export const DeleteWarActions: IDeleteWarActionsTypes = {
  REQUEST: "WAR_REQUEST_WAR_DELETE",
  SUCCES: "WAR_SUCCES_WAR_DELETE",
  ERROR: "WAR_ERROR_WAR_DELETE"
};

//ACTIONHOZ
export interface IActionRequestDeleteWar {
  type: IDeleteWarActionsTypes["REQUEST"];
  params: number;
}

export interface IActionSuccesDeleteWar {
  type: IDeleteWarActionsTypes["SUCCES"];
  data: number;
}

export interface IActionErrorDeleteWar {
  type: IDeleteWarActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IDeleteWarActions =
  | IActionRequestDeleteWar
  | IActionSuccesDeleteWar
  | IActionErrorDeleteWar;

//ACTIONCREATORHOZ
export const DeleteWarActionCreator = (
  params: number
): IActionRequestDeleteWar => ({
  type: DeleteWarActions.REQUEST,
  params
});
export const fetchError = (error?: string): IActionErrorDeleteWar => ({
  type: DeleteWarActions.ERROR,
  error
});
export const fetchSucces = (data: number): IActionSuccesDeleteWar => ({
  type: DeleteWarActions.SUCCES,
  data
});
