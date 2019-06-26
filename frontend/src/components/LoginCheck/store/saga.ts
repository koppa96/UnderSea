import * as React from "react";
import axios from "axios";

import qs from "qs";
import { call, put, takeEvery } from "redux-saga/effects";
import {
  IActions,
  IActionLoginRequest,
  ISuccesParamState,
  LoginActions
} from "./actions/post";

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
  const url = "https://localhost:44355/connect/token";

  const instance = axios.create();
  const resp = instance
    .post(url, requestBody, config)
    .then(response => {
      const access_token =
        response.data.token_type + " " + response.data.access_token;
      const refresh_token = response.data.refresh_token;
      localStorage.setItem("access_token", access_token);
      localStorage.setItem("refresh_token", refresh_token);

      return response;
    })
    .catch(error => {
      return error;
    });
  return resp;
};
/*
export const refreshLoginCheck = (
    name: string,
    password: string
  ): Promise<IActions> => {

    const localRefreshToken=localStorage.getItem("refresh_token")

    if(localRefreshToken){
        const config = {
            headers: {
              "Content-Type": "application/x-www-form-urlencoded",
              "Access-Control-Allow-Origin": "*",
              "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
            }
          };
        
          const requestBody = qs.stringify({
            refresh_token:localRefreshToken,
            client_id: "undersea_client",
            client_secret: "undersea_client_secret",
            scope: "offline_access undersea_api",
            grant_type: "refresh_token"
          });
          const url = "https://localhost:44355/connect/token";
        
          const resp = axios
            .post(url, requestBody, config)
            .then(response => {
              const access_token =
                response.data.token_type + " " + response.data.access_token;
              const refresh_token = response.data.refresh_token;
              localStorage.setItem("access_token", access_token);
              localStorage.setItem("refresh_token", refresh_token);
        
              return response;
            })
            .catch(error => {
              return error;
            });
          return resp;
        };


    }


  }

function* handleLogin(action: IActionLoginRequest) {
  try {
    const caller: ISuccesParamState = yield call(
      beginToLogin,
      action.params.name,
      action.params.password
    );
    console.log(caller);
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
*/
