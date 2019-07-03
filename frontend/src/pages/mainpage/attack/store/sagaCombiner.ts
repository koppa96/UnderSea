import { all } from "redux-saga/effects";
import { watcAttackTargetRequest } from "./actions/saga.post";
import { watchTargetFetchRequest } from "./actions/saga.get";

export function* watchTargetActions() {
  yield all([watchTargetFetchRequest(), watcAttackTargetRequest()]);
}
