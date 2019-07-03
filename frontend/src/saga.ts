import { all, call } from "redux-saga/effects";
import { watchLoginFetchRequest } from "./pages/account/login/store/saga";
import { watchArmyUnits } from "./pages/mainpage/army/store/saga";

import { watchMainPageFetchRequest } from "./pages/mainpage/store/saga";
import { watchRankFetchRequest } from "./pages/mainpage/rank/store/actions/saga.get";
import { watchProfileFetchRequest } from "./components/profileContainer/store/saga";
import { watchWarFetchRequest } from "./pages/mainpage/war/store/actions/saga.get";
import { watchDevelopmentFetchRequest } from "./pages/mainpage/development/store/actions/saga.get";
import { watchAddResearchRequest } from "./pages/mainpage/development/store/actions/saga.post";
import { watcAttackTargetRequest } from "./pages/mainpage/attack/store/actions/saga.post";
import { watchBuildingActions } from "./pages/mainpage/buildings/store/sagaCombiner";
import { watchTargetActions } from "./pages/mainpage/attack/store/sagaCombiner";
import { watchWarActions } from "./pages/mainpage/war/store/sagaCombiner";
import { watchReportActions } from "./pages/mainpage/reports/store/sagaCombiner";
import { watchTokenCheckRequest } from "./store/actions/saga.get";

export function* rootSaga() {
  yield all([
    call(watchLoginFetchRequest),
    call(watchMainPageFetchRequest),
    call(watchArmyUnits),
    call(watchBuildingActions),
    call(watchRankFetchRequest),
    call(watchProfileFetchRequest),
    call(watchWarActions),
    call(watchDevelopmentFetchRequest),
    call(watchAddResearchRequest),
    call(watchTargetActions),
    call(watchReportActions),
    call(watchTokenCheckRequest)
  ]);
}
