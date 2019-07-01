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

const beginFetchWar = async () => {
  const instance = axios.create();
  registerAxiosConfig();

  try {
    const response = await axios.get("/api/Commands");
    console.log("war fetched", response.data);

    return response.data;
  } catch (error) {
    console.log("war fetch error", error);

    throw new Error(error);
  }
};

function* handleFetch(action: IRequestActionGetWar) {
  console.log("SAGA-War");
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
