import { call, put, takeEvery, all } from "redux-saga/effects";
import { ICreationInfo } from "../../../../../api/Client";
import {
  IRequestActionGetDevelopment,
  GetDevelopmentSuccessActionCreator,
  GetDevelopmentErrorActionCreator,
  ISuccesParamState,
  GetDevelopmentActions
} from "./DevelopmnetAction.get";
import { BasePortUrl } from "../../../../..";
import axios from "axios";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

export const beginFetchDevelopment = () => {
  const url = BasePortUrl + "api/Researches";
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .get(url)
    .then(response => {
      return response.data;
    })
    .catch(error => {
      throw new Error(error);
    });
};

function* handleLogin(action: IRequestActionGetDevelopment) {
  try {
    const data: ICreationInfo[] = yield call(beginFetchDevelopment);
    const response: ISuccesParamState = { description: data };
    yield put(GetDevelopmentSuccessActionCreator(response));
  } catch (err) {
    if (err) {
      yield put(GetDevelopmentErrorActionCreator("Hiba történt"));
    } else {
      yield put(GetDevelopmentErrorActionCreator("An unknown error occured."));
    }
  }
}

export function* watchDevelopmentFetchRequest() {
  yield takeEvery(GetDevelopmentActions.REQUEST, handleLogin);
}
