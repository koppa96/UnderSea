import { all, call } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/store/saga";
import { watchArmyUnits } from "./pages/mainpage/army/store/saga";

import { watchMainPageFetchRequest } from "./pages/mainpage/store/saga";
import { watchAddBuildingRequest } from "./pages/mainpage/buildings/store/actions/saga.post";
import { watchBuildingFetchRequest } from "./pages/mainpage/buildings/store/actions/saga.get";
import { watchRankFetchRequest } from "./pages/mainpage/rank/store/actions/saga.get";

export function* rootSaga() {
  yield all([
    call(watchLoginFetchRequest),
    call(watchMainPageFetchRequest),
    call(watchArmyUnits),
    call(watchBuildingFetchRequest),
    call(watchAddBuildingRequest),
    call(watchRankFetchRequest)
  ]);
}
