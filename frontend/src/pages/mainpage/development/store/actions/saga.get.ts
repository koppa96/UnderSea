import { call, put, takeEvery, all } from "redux-saga/effects";
import {
  AccountsClient,
  ResearchesClient,
  ICreationInfo
} from "../../../../../api/Client";
import {
  IRequestActionGetDevelopment,
  fetchSucces,
  fetchError,
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
  console.log("SAGA-Development");
  try {
    const data: ICreationInfo[] = yield call(beginFetchDevelopment);
    const response: ISuccesParamState = { description: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError("Hiba történt"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchDevelopmentFetchRequest() {
  yield takeEvery(GetDevelopmentActions.REQUEST, handleLogin);
}
