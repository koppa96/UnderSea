import { all } from "redux-saga/effects";
import { watchWarFetchRequest } from "./actions/saga.get";
import { watchWarDeleteRequest } from "./actions/saga.delete";

export function* watchWarActions() {
  yield all([watchWarFetchRequest(), watchWarDeleteRequest()]);
}
