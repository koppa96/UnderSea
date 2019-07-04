import { all } from "redux-saga/effects";
import { watchPostImageRequest } from "./actions/saga.post";
import { watchProfileFetchRequest } from "./actions/saga.get";

export function* watchProfileActions() {
  yield all([watchPostImageRequest(), watchProfileFetchRequest()]);
}
