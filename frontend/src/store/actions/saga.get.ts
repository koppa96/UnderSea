import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";

import { BasePortUrl } from "../..";
import { registerAxiosConfig } from "../../config/axiosConfig";
import { AccountsClient } from "../../api/Client";
import {
  IRequestActionCheckToken,
  fetchSucces,
  fetchError,
  CheckTokenActions
} from "./CheckToken.get";

export const beginFetchBuilding = () => {
  const CheckTokenedList = new AccountsClient();
  const tempData = CheckTokenedList.getAccount();
  return tempData;
};

const beginFetchUser = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Accounts/me");
    console.log("profil fetched", response.data);

    return response.data;
  } catch (error) {
    console.log("profil fetch error", error);

    throw new Error(error);
  }
};

function* handleFetch(action: IRequestActionCheckToken) {
  console.log("SAGA-Profile");
  try {
    const data = yield call(beginFetchUser);
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
