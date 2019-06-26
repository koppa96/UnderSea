import "bootstrap/dist/css/bootstrap.min.css";
import React from "react";
import ReactDOM from "react-dom";
import { App } from "./App";
import axios from "axios";
import * as serviceWorker from "./serviceWorker";
import { Provider } from "react-redux";
import { configureStore } from "./config/ConfigureStore";
import { createBrowserHistory } from "history";
import { ConnectedRouter } from "connected-react-router";
import { codegen } from "swagger-axios-codegen";
import qs from "qs";

export const BasePortUrl = "https://localhost:44355/";

axios.defaults.baseURL = BasePortUrl;

axios.defaults.headers.common["Authorization"] = localStorage.getItem(
  "access_token"
);
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
        grant_type: "password"
      });
      const url = "connect/token";

      const { data } = await axios.post(url, requestBody, config);
      window.localStorage.setItem("token", data.token);
      window.localStorage.setItem("refreshToken", data.refreshToken);
      axios.defaults.headers.common["Authorization"] = "Bearer " + data.token;
      originalRequest.headers["Authorization"] = "Bearer " + data.token;
      return axios(originalRequest);
    }

    return Promise.reject(error);
  }
);
const history = createBrowserHistory({ basename: "/" });
const { store } = configureStore(history);

// codegen({

//   methodNameMode: 'operationId',
//   source:require('./config/swagger.json'),
//   outputDir: '.',
//   useStaticMethod:true
// });

ReactDOM.render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <React.StrictMode>
        <App />
      </React.StrictMode>
    </ConnectedRouter>
  </Provider>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
