import { call, put, takeEvery, all } from "redux-saga/effects";
import { ResearchesClient, ICreationInfo } from "../../../../../api/Client";
import {
  IRequestActionGetDevelopment,
  GetDevelopmentSuccessActionCreator,
  GetDevelopmentErrorActionCreator,
  ISuccesParamState,
  GetDevelopmentActions
} from "./DevelopmnetAction.get";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

export const beginFetchDevelopment = () => {
  const getResearch = new ResearchesClient();
  //registerAxiosConfig();
  const tempData = getResearch.getResearches();
  return tempData;
};

function* handleLogin(action: IRequestActionGetDevelopment) {
  try {
    const data: ICreationInfo[] = yield call(beginFetchDevelopment);
    const response: ISuccesParamState = { description: data };
    yield put(GetDevelopmentSuccessActionCreator(response));
  } catch (err) {
    if (err) {
      yield put(GetDevelopmentErrorActionCreator("Hiba történt"));
    } else {
      yield put(GetDevelopmentErrorActionCreator("An unknown error occured."));
    }
  }
}

export function* watchDevelopmentFetchRequest() {
  yield takeEvery(GetDevelopmentActions.REQUEST, handleLogin);
}
