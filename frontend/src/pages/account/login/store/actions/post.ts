export interface ILoginActionsTypes {
  REQUEST: "LOGIN_REQUEST_LOGIN_LOGIN";
  SUCCES: "LOGIN_REQUEST_LOGIN_LOGIN";
  ERROR: "LOGIN_REQUEST_LOGIN_LOGIN";
}

export const LoginActions: ILoginActionsTypes = {
  REQUEST: "LOGIN_REQUEST_LOGIN_LOGIN",
  SUCCES: "LOGIN_REQUEST_LOGIN_LOGIN",
  ERROR: "LOGIN_REQUEST_LOGIN_LOGIN"
};

export interface IRequestParamState {
  name: string;
  password: string;
}
//ACtionshoz
export interface IActionLoginRequest {
  type: ILoginActionsTypes["REQUEST"];
  params: IRequestParamState;
}
export interface IActionLoginSucces {
  type: ILoginActionsTypes["SUCCES"];
  params: IRequestParamState;
}

export interface IActionLoginError {
  type: ILoginActionsTypes["ERROR"];
}
//Reducerhez
export type IActions =
  | IActionLoginRequest
  | IActionLoginSucces
  | IActionLoginError;

//ActionCreators

export const TestAddActionCreator = (
  params: IRequestParamState
): IActionLoginRequest => ({
  type: LoginActions.REQUEST,
  params
});
