import axios from "axios";

import qs from "qs";
import { call, put, takeEvery } from "redux-saga/effects";
import {
  LoginActions,
  fetchError,
  fetchSucces,
  ISuccesParamState,
  IActions,
  IActionLoginRequest
} from "./actions/LoginAction.post";
import { BasePortUrl } from "../../../..";

export const beginToLogin = (
  name: string,
  password: string
): Promise<IActions> => {
  const config = {
    headers: {
      "Content-Type": "application/x-www-form-urlencoded",
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
    }
  };

  const requestBody = qs.stringify({
    username: name,
    password: password,
    client_id: "undersea_client",
    client_secret: "undersea_client_secret",
    scope: "offline_access undersea_api",
    grant_type: "password"
  });
  const url = BasePortUrl + "connect/token";

  const resp = axios
    .post(url, requestBody, config)
    .then(response => {
      const access_token =
        response.data.token_type + " " + response.data.access_token;
      const refresh_token = response.data.refresh_token;
      localStorage.setItem("access_token", access_token);
      localStorage.setItem("refresh_token", refresh_token);

      axios.defaults.headers.common["Authorization"] = localStorage.getItem(
        "access_token"
      );
      return response;
    })
    .catch(error => {
      return error;
    });
  return resp;
};

function* handleLogin(action: IActionLoginRequest) {
  try {
    const caller: ISuccesParamState = yield call(
      beginToLogin,
      action.params.name,
      action.params.password
    );
    yield put(fetchSucces(caller));
  } catch (err) {
    if (err) {
      yield put(fetchError("Rossz jelszó, vagy felhasználó"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchLoginFetchRequest() {
  yield takeEvery(LoginActions.REQUEST, handleLogin);
}
