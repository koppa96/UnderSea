export interface ITestActionsTypes {
  REQUEST: "TEST_REQUEST_MAN_CREATE";
  DELETE: "TEST_REQUEST_MAN_DELETE";
}

export const TestActions: ITestActionsTypes = {
  REQUEST: "TEST_REQUEST_MAN_CREATE",
  DELETE: "TEST_REQUEST_MAN_DELETE"
};

export interface IRequestParamState {
  name: string;
  age: number;
  gender: string;
}
//ACtionshoz
export interface IActionAddMan {
  type: ITestActionsTypes["REQUEST"];
  params: IRequestParamState;
}

export interface IActionDeleteMan {
  type: ITestActionsTypes["DELETE"];
}

//Reducerhez
export type IActions = IActionAddMan | IActionDeleteMan;

//ActionCreators

export const TestAddActionCreator = (
  params: IRequestParamState
): IActionAddMan => ({
  type: TestActions.REQUEST,
  params
});

export const TestDeleteActionCreator = (): IActionDeleteMan => ({
  type: TestActions.DELETE
});
