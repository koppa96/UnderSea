import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import {
  DeleteWarActions,
  IActionRequestDeleteWar,
  fetchSucces,
  fetchError
} from "./WarAction.delete";

const beginFetchWar = async (id: number) => {
  const instance = axios.create();
  registerAxiosConfig();

  try {
    const response = await axios.delete("/api/Commands/" + id);
    console.log("war deleted", response);

    return response.data;
  } catch (error) {
    console.log("war delete error", error);

    throw new Error(error);
  }
};

function* handleDelete(action: IActionRequestDeleteWar) {
  try {
    const data = yield call(beginFetchWar, action.params);
    yield put(fetchSucces(action.params));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchWarDeleteRequest() {
  yield takeEvery(DeleteWarActions.REQUEST, handleDelete);
}
