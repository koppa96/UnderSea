import { call, put, takeEvery } from "redux-saga/effects";

import {
  GetRankActions,
  IRequestActionGetRank,
  ISuccesParamState,
  fetchError,
  fetchSucces
} from "./RankAction.get";
import { IRankInfo, AccountsClient } from "../../../../../api/Client";

export const beginFetchBuilding = () => {
  const getRankedList = new AccountsClient();
  const tempData = getRankedList.getRankedList();
  return tempData;
};

function* handleFetch(action: IRequestActionGetRank) {
  console.log("SAGA-RANK");
  try {
    const data: IRankInfo[] = yield call(beginFetchBuilding);
    const response: ISuccesParamState = { ranks: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchRankFetchRequest() {
  yield takeEvery(GetRankActions.REQUEST, handleFetch);
}
