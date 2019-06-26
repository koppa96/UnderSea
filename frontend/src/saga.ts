import { all, fork } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/saga";
import { watchMainPageFetchRequest } from "./pages/mainpage/store/saga";

export function* rootSaga() {
  yield all([fork(watchLoginFetchRequest), fork(watchMainPageFetchRequest)]);
}
