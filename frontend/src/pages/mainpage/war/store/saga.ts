import { call, put, takeEvery } from "redux-saga/effects";
import {
  CommandsClient,
  ICombatInfo,
  ICommandInfo
} from "../../../../api/Client";
import {
  IRequestActionGetWar,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  GetWarActions
} from "./actions/WarAction.get";

export const beginFetchBuilding = () => {
  const getWaredList = new CommandsClient();
  const tempData = getWaredList.getCommands();
  return tempData;
};

function* handleFetch(action: IRequestActionGetWar) {
  console.log("SAGA-War");
  try {
    const data: ICommandInfo[] = yield call(beginFetchBuilding);
    const response: ISuccesParamState = { wars: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchWarFetchRequest() {
  yield takeEvery(GetWarActions.REQUEST, handleFetch);
}
