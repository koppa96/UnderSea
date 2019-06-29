import axios, { AxiosStatic } from "axios";
import { BasePortUrl } from "..";
import qs from "qs";

export const registerAxiosConfig = () => {
  axios.interceptors.request.use(request => {
    console.log("Starting Request", request);
    return request;
  });

  axios.defaults.headers.common = {
    Authorization: localStorage.getItem("access_token"),
    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token",
    "Content-Type": "application/json"
  };

  axios.interceptors.response.use(
    function(response) {
      return response;
    },
    async function(error) {
      const originalRequest = error.config;

      if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        console.log("Denied request, begin refresh token connect");
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

        const instance = axios.create();
        const { data } = await instance.post(url, requestBody, config);

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
