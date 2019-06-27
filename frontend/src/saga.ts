import { all, fork, call } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/saga";
import { watchArmyUnits } from "./pages/mainpage/army/saga";

import { watchMainPageFetchRequest } from "./pages/mainpage/store/saga";
import { watchBuildingFetchRequest } from "./pages/mainpage/buildings/store/saga";
import { watchAddBuildingRequest } from "./pages/mainpage/store/actions/post/saga";

export function* rootSaga() {
  yield all([
    call(watchLoginFetchRequest),
    call(watchMainPageFetchRequest),
    call(watchArmyUnits),
    call(watchBuildingFetchRequest),
    call(watchAddBuildingRequest)
  ]);
}
