import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";
import {
  fetchSucces,
  IRequestActionGetBuilding,
  fetchError,
  GetBuildingActions
} from "./BuildingAction.get";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";

const beginFetchBuilding = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Buildings");

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
};

function* handleGetBuildings(action: IRequestActionGetBuilding) {
  try {
    const data = yield call(beginFetchBuilding);
    yield put(fetchSucces(data));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("Ismeretlen hiba történt"));
    }
  }
}

export function* watchBuildingFetchRequest() {
  yield takeEvery(GetBuildingActions.REQUEST, handleGetBuildings);
}
