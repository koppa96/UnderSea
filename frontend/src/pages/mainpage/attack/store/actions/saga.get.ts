import { call, put, takeEvery } from "redux-saga/effects";
import { AccountsClient, ITargetInfo } from "../../../../../api/Client";
import {
  fetchSucces,
  IRequestActionGetTarget,
  GetTargetActions,
  fetchError
} from "./GetAttackAction.get";

import axios from "axios";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

const beginFetchTargets = async () => {
  const instance = axios.create();
  registerAxiosConfig();

  try {
    const response = await axios.get("/api/Accounts");
    console.log("targets fetched", response.data);

    return response.data;
  } catch (error) {
    console.log("targets fetch error", error);

    throw new Error(error);
  }
};

function* handleGetTargets(action: IRequestActionGetTarget) {
  console.log("SAGA-Target");
  try {
    const data: ITargetInfo[] = yield call(beginFetchTargets);
    console.log("fetched targets", data);
    yield put(fetchSucces(data));
  } catch (err) {
    if (err) {
      yield put(fetchError("Sajnos valami hiba történt betöltés közben"));
    } else {
      yield put(fetchError("Ismeretlen hiba történt"));
    }
  }
}

export function* watchTargetFetchRequest() {
  yield takeEvery(GetTargetActions.REQUEST, handleGetTargets);
}
