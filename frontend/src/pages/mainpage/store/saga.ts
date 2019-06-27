import * as React from "react";
import axios from "axios";

import { call, put, takeEvery, all, fork } from "redux-saga/effects";
import {
  IActions,
  IActionMainpageRequest,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  MainpageActions
} from "./actions/get/actions";
import { CountryClient, CountryInfo, ICountryInfo } from "../../../api/Client";
import { async } from "q";
import { watchBuildingFetchRequest } from "../buildings/store/saga";

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
