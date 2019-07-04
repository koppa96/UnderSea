import { call, put, takeEvery } from "redux-saga/effects";
import {
  fetchSucces,
  IRequestActionGetTarget,
  GetTargetActions,
  fetchError,
  ITargetInfo
} from "./GetAttackAction.get";

import axios from "axios";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";

const beginFetchTargets = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Accounts");

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
};

function* handleGetTargets(action: IRequestActionGetTarget) {
  try {
    const data: ITargetInfo[] = yield call(beginFetchTargets);
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
