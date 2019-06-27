import { call, put, takeEvery } from "redux-saga/effects";

import { async } from "q";
import {
  AddBuildingActions,
  fetchError,
  fetchSucces,
  IActionRequestAddBuilding
} from "./BuildingAction.post";
import { BuildingsClient } from "../../../../../api/Client";

export const beginToAddBuilding = (id: number): Promise<void> => {
  const startBuilding = new BuildingsClient();
  return startBuilding.startBuilding(id);
};

function* handleAddBuilding(action: IActionRequestAddBuilding) {
  try {
    yield call(beginToAddBuilding, action.params);
    yield put(fetchSucces());
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchAddBuildingRequest() {
  yield takeEvery(AddBuildingActions.REQUEST, handleAddBuilding);
}
