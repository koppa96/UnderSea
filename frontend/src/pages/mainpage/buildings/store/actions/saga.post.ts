import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";
import {
  AddBuildingActions,
  fetchError,
  fetchSucces,
  IActionRequestAddBuilding
} from "./BuildingAction.post";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";

const beginToAddBuilding = (id: number): Promise<void> | any => {
  const url = BasePortUrl + "api/Buildings/" + id;
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .post(url)
    .then(response => {
      return response;
    })
    .catch(error => {
      throw new Error(error);
    });
};
function* handleAddBuilding(action: IActionRequestAddBuilding) {
  try {
    yield call(beginToAddBuilding, action.params.id);
    yield put(fetchSucces(action.params));
  } catch (err) {
    if (err) {
      yield put(fetchError("Sajnos valami hiba történt vásárlás közben"));
    } else {
      yield put(fetchError("Ismeretlen hiba"));
    }
  }
}

export function* watchAddBuildingRequest() {
  yield takeEvery(AddBuildingActions.REQUEST, handleAddBuilding);
}
