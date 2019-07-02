import { all } from "redux-saga/effects";
import { watchReportFetchRequest } from "./actions/saga.get";
import { watchSeenReportsRequest } from "./actions/saga.post";
import { watchDeleteReportsRequest } from "./actions/saga.delete";

export function* watchReportActions() {
  yield all([
    watchReportFetchRequest(),
    watchSeenReportsRequest(),
    watchDeleteReportsRequest()
  ]);
}
