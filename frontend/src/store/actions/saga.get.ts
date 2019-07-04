import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";

import { BasePortUrl } from "../..";
import { registerAxiosConfig } from "../../config/axiosConfig";
import { fetchSucces, fetchError, CheckTokenActions } from "./CheckToken.get";

const beginFetchUser = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Accounts/me");

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
};

function* handleFetch() {
  try {
    yield call(beginFetchUser);
    yield put(fetchSucces());
  } catch (err) {
    if (err) {
      yield put(fetchError("Rossz token"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchTokenCheckRequest() {
  yield takeEvery(CheckTokenActions.REQUEST, handleFetch);
}
