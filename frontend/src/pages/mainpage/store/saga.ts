import * as React from "react";
import axios from "axios";

import { call, put, takeEvery } from "redux-saga/effects";
import {
  IActions,
  IActionMainpageRequest,
  ISuccesParamState,
  fetchSucces,
  fetchError
} from "./actions";
import { CountryClient } from "../../../api/Client";

export const beginToFetchMainpage = (): Promise<IActions> => {
  const config = {
    headers: {
      "Content-Type": "application/json",
      Authorization: localStorage.getItem("access_token")
    }
  };

  const url = "api/country";

  const getCountry = new CountryClient();

  const resp = axios
    .get(url, config)
    .then(response => {
      return response;
    })
    .catch(error => {
      return error;
    });
  return resp;
};

function* handleLogin(action: IActionMainpageRequest) {
  try {
    const caller: ISuccesParamState = yield call(beginToFetchMainpage);
    console.log(caller, "mainpage req érkezik");
    yield put(fetchSucces(caller));
  } catch (err) {
    if (err) {
      yield put(fetchError("hiba történt"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchLoginFetchRequest() {
  yield takeEvery(LoginActions.REQUEST, handleLogin);
}
