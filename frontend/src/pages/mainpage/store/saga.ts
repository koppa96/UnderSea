import { call, put, takeEvery, all, fork } from "redux-saga/effects";
import {
  IActionMainpageRequest,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  MainpageActions
} from "./actions/MainpageAction.get";
import { CountryClient, CountryInfo, ICountryInfo } from "../../../api/Client";

export const beginToFetchMainpage = (): Promise<CountryInfo> => {
  const getCountry = new CountryClient();
  const tempData = getCountry.getCurrentState();

  return tempData;
};

function* handleLogin(action: IActionMainpageRequest) {
  try {
    const data: ICountryInfo = yield call(beginToFetchMainpage);
    const response: ISuccesParamState = { country: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError("hiba történt"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchMainPageFetchRequest() {
  yield takeEvery(MainpageActions.REQUEST, handleLogin);
}
