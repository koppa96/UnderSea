import { all, call } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/store/saga";
import { watchArmyUnits } from "./pages/mainpage/army/store/saga";

import { watchMainPageFetchRequest } from "./pages/mainpage/store/saga";
import { watchAddBuildingRequest } from "./pages/mainpage/buildings/store/actions/saga.post";
import { watchBuildingFetchRequest } from "./pages/mainpage/buildings/store/actions/saga.get";
import { watchRankFetchRequest } from "./pages/mainpage/rank/store/actions/saga.get";
import { watchProfileFetchRequest } from "./components/profileContainer/store/saga";
import { watchWarFetchRequest } from "./pages/mainpage/war/store/saga";
import { watchDevelopmentFetchRequest } from "./pages/mainpage/development/store/actions/saga.get";
import { watchAddResearchRequest } from "./pages/mainpage/development/store/actions/saga.post";
import { watcAttackTargetRequest } from "./pages/mainpage/attack/store/actions/saga.post";

export function* rootSaga() {
  yield all([
    call(watchLoginFetchRequest),
    call(watchMainPageFetchRequest),
    call(watchArmyUnits),
    call(watchBuildingFetchRequest),
    call(watchAddBuildingRequest),
    call(watchRankFetchRequest),
    call(watchProfileFetchRequest),
    call(watchWarFetchRequest),
    call(watchDevelopmentFetchRequest),
    call(watchAddResearchRequest),
    call(watcAttackTargetRequest)
  ]);
}
