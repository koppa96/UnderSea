import * as React from "react";
import { Route, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";

import axios from "axios";
import "./app.scss";
import { NotFound } from "./pages/notFound/index";
import { LoginConnected } from "./pages/account/login/connect";
import { LoginCheckConnected } from "./components/LoginCheck/connect";
import { MainPageConnected } from "./pages/mainpage/connect";
import { BasePortUrl } from ".";
import qs from "qs";

export const App = () => {
  const loggedin = true;

  //TODO: Router kiszervezés
  return (
    <div className="App">
      <div className="bg-image">
        <Switch>
          <Route exact path="/">
            <LoginCheckConnected login={loggedin}>
              <MainPageConnected />
            </LoginCheckConnected>
          </Route>
          <Route path="/account">
            <LoginCheckConnected login={loggedin}>
              <MainPageConnected />
            </LoginCheckConnected>
          </Route>
          <Route path="/register" component={Register} />

          <Route path="/login">
            <LoginCheckConnected login={!loggedin}>
              <LoginConnected />
            </LoginCheckConnected>
          </Route>
          <Route component={NotFound} />
        </Switch>
      </div>
    </div>
  );
};
export const registeraxios = () => {
  axios.defaults.baseURL = BasePortUrl;
  axios.interceptors.request.use(request => {
    console.log("Starting Request", request);
    return request;
  });
  axios.interceptors.response.use(
    function(response) {
      return response;
    },
    async function(error) {
      const originalRequest = error.config;

      if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;

        const refreshToken = window.localStorage.getItem("refresh_token");

        const config = {
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
          }
        };

        const requestBody = qs.stringify({
          refresh_token: refreshToken,
          client_id: "undersea_client",
          client_secret: "undersea_client_secret",
          scope: "offline_access undersea_api",
          grant_type: "refresh_token"
        });
        const url = BasePortUrl + "connect/token";
        console.log(url, "axios url");
        const instance = axios.create();
        const { data } = await instance.post(url, requestBody, config);
        console.log(data, "axios data");
        window.localStorage.setItem("access_token", data.token);
        window.localStorage.setItem("refresh_token", data.refreshToken);
        axios.defaults.headers.common["Authorization"] = "Bearer " + data.token;
        originalRequest.headers["Authorization"] = "Bearer " + data.token;
        return axios(originalRequest);
      } else {
        localStorage.removeItem("access_token");
        localStorage.removeItem("refresh_token");
      }
      return Promise.reject(error);
    }
  );
};
