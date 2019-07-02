import { all } from "redux-saga/effects";
import { watchReportFetchRequest } from "./actions/saga.get";

export function* watchReportActions() {
  yield all([watchReportFetchRequest()]);
}
