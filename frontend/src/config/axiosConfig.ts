import axios, { AxiosStatic, AxiosInstance } from "axios";
import { BasePortUrl } from "..";
import qs from "qs";

export const registerAxiosConfig = (instance: AxiosInstance) => {
  instance.interceptors.request.use(request => {
    console.log("Starting Request", request);
    return request;
  });

  instance.defaults.headers.common = {
    Authorization: localStorage.getItem("access_token"),
    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token",
    "Content-Type": "application/json"
  };

  instance.interceptors.response.use(
    function(response) {
      return response;
    },
    async function(error) {
      const originalRequest = error.config;
      console.log("oo 1");
      if (error.response.status === 401 && !originalRequest._retry) {
        console.log("oo 2");
        originalRequest._retry = true;
        console.log("Denied request, begin refresh token connect");
        const refreshToken = localStorage.getItem("refresh_token");

        const config = {
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
          }
        };

        console.log(localStorage.getItem("refresh_token"), "ez itt");

        const requestBody = qs.stringify({
          refresh_token: localStorage.getItem("refresh_token"),
          client_id: "undersea_client",
          client_secret: "undersea_client_secret",
          scope: "offline_access undersea_api",
          grant_type: "refresh_token"
        });
        const url = BasePortUrl + "connect/token";
        console.log("oo 3");
        const tokeninstance = axios.create();
        console.log("oo 4");

        tokeninstance
          .post(url, requestBody, config)
          .then(res => {
            console.log(res);
            console.log("datatatatatata", res.data);

            localStorage.setItem("access_token", "Bearer " + res.data.token);
            localStorage.setItem("refresh_token", res.data.refreshToken);
            tokeninstance.defaults.headers.common["Authorization"] =
              "Bearer " + res.data.token;
            originalRequest.headers["Authorization"] =
              "Bearer " + res.data.token;
            return axios(originalRequest);
          })
          .catch(err => {
            console.log(err);
          });
      }
      console.log("oo utolso");
      return Promise.reject(error);
    }
  );
  return instance;
};
