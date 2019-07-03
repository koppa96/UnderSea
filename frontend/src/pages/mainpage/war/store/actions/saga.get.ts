import { call, put, takeEvery } from "redux-saga/effects";

import {
  IRequestActionGetWar,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  GetWarActions,
  ICommandInfo
} from "./WarAction.get";

import axios from "axios";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";

const beginFetchWar = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Commands");

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
};

function* handleFetch(action: IRequestActionGetWar) {
  try {
    const data: ICommandInfo[] = yield call(beginFetchWar);
    const response: ISuccesParamState = { wars: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchWarFetchRequest() {
  yield takeEvery(GetWarActions.REQUEST, handleFetch);
}
