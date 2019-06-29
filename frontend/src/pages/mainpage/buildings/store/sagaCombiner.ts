import { all } from "redux-saga/effects";
import { watchAddBuildingRequest } from "./actions/saga.post";
import { watchBuildingFetchRequest } from "./actions/saga.get";

export function* watchBuildingActions() {
  yield all([watchAddBuildingRequest(), watchBuildingFetchRequest()]);
}
