import { all, fork, call } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/saga";
import { watchArmyAddUnitsRequest } from "./pages/mainpage/army/saga";

import {
  watchMainPageFetchRequest,
  mainpageSaga
} from "./pages/mainpage/store/saga";

export function* rootSaga() {
  yield all([call(watchLoginFetchRequest), call(mainpageSaga),call(watchArmyAddUnitsRequest)]);
}
