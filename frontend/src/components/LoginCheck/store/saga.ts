import * as React from "react";
import axios from "axios";

import qs from "qs";
import { call, put, takeEvery } from "redux-saga/effects";
import {
  IActions,
  IActionLoginRequest,
  ISuccesParamState,
  LoginActions
} from "./actions/LoginCheckAction.post";

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
