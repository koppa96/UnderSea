import { call, put, takeEvery } from "redux-saga/effects";

import { BuildingsClient, ICreationInfo } from "../../../../api/Client";
import {
  IRequestActionGetBuilding,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  GetBuildingActions
} from "./actions/get";

export const beginFetchBuilding = () => {
  const getCountry = new BuildingsClient();
  const tempData = getCountry.getBuildings();
  return tempData;
};

function* handleLogin(action: IRequestActionGetBuilding) {
  console.log("SAGA-BUILDING");
  try {
    const data: ICreationInfo[] = yield call(beginFetchBuilding);
    const response: ISuccesParamState = { buildings: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError("Hiba történt"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchBuildingFetchRequest() {
  yield takeEvery(GetBuildingActions.REQUEST, handleLogin);
}
