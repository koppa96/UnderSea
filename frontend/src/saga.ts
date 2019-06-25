import { all, fork } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/saga";

export function* rootSaga() {
  yield all([fork(watchLoginFetchRequest)]);
}
