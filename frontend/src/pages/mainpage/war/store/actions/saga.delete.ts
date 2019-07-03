import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import {
  DeleteWarActions,
  IActionRequestDeleteWar,
  fetchSucces,
  fetchError
} from "./WarAction.delete";
import { BasePortUrl } from "../../../../..";

const beginFetchWar = async (id: number) => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.delete(
      BasePortUrl + "api/Commands/" + id
    );

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
};

function* handleDelete(action: IActionRequestDeleteWar) {
  try {
    yield call(beginFetchWar, action.params.id);
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
