//ACTIONTYPES
export interface IAddDevelopmentActionsTypes {
  REQUEST: "DEVELOPMENT_REQUEST_DEVELOPMENT_CREATE";
  SUCCES: "DEVELOPMENT_SUCCES_DEVELOPMENT_CREATE";
  ERROR: "DEVELOPMENT_ERROR_DEVELOPMENT_CREATE";
}

export const AddDevelopmentActions: IAddDevelopmentActionsTypes = {
  REQUEST: "DEVELOPMENT_REQUEST_DEVELOPMENT_CREATE",
  SUCCES: "DEVELOPMENT_SUCCES_DEVELOPMENT_CREATE",
  ERROR: "DEVELOPMENT_ERROR_DEVELOPMENT_CREATE"
};

//ACTIONHOZ
export interface IActionRequestAddDevelopment {
  type: IAddDevelopmentActionsTypes["REQUEST"];
  params: number;
}

export interface IActionSuccesAddDevelopment {
  type: IAddDevelopmentActionsTypes["SUCCES"];
}

export interface IActionErrorAddDevelopment {
  type: IAddDevelopmentActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IAddDevelopmentActions =
  | IActionRequestAddDevelopment
  | IActionSuccesAddDevelopment
  | IActionErrorAddDevelopment;

//ACTIONCREATORHOZ
export const AddDevelopmentRequestActionCreator = (
  params: number
): IActionRequestAddDevelopment => ({
  type: AddDevelopmentActions.REQUEST,
  params
});
export const AddDevelopmentErrorActionCreator = (
  params?: string
): IActionErrorAddDevelopment => ({
  type: AddDevelopmentActions.ERROR,
  params
});
export const AddDevelopmentSuccessActionCreator = (): IActionSuccesAddDevelopment => ({
  type: AddDevelopmentActions.SUCCES
});
