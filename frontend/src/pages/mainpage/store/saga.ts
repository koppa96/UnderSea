import { call, put, takeEvery } from "redux-saga/effects";
import {
  IActionMainpageRequest,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  MainpageActions
} from "./actions/MainpageAction.get";
import { registerAxiosConfig } from "../../../config/axiosConfig";

import axios from "axios";
import { BasePortUrl } from "../../..";
import { ICountryInfo } from "../Interface";
const beginToFetchMainpage = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Country");

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
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
