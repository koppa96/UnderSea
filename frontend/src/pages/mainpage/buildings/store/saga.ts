import { call, put, takeEvery } from "redux-saga/effects";

import { BuildingsClient } from "../../../../api/Client";
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
    const caller: ISuccesParamState = yield call(beginFetchBuilding);
    console.log(caller);
    yield put(fetchSucces(caller));
  } catch (err) {
    if (err) {
      yield put(fetchError("Rossz jelszó, vagy felhasználó"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchBuildingFetchRequest() {
  yield takeEvery(GetBuildingActions.REQUEST, handleLogin);
}
