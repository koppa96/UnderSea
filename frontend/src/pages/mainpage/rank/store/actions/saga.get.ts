import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";
import {
  GetRankActions,
  IRequestActionGetRank,
  ISuccesParamState,
  fetchError,
  fetchSucces
} from "./RankAction.get";
import { IRankInfo, AccountsClient } from "../../../../../api/Client";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

const beginFetchRank = async () => {
  const instance = axios.create();
  registerAxiosConfig();

  try {
    const response = await axios.get("/api/Accounts/ranked");
    console.log("rank fetched", response.data);

    return response.data;
  } catch (error) {
    console.log("rank fetch error", error);

    throw new Error(error);
  }
};

function* handleFetch(action: IRequestActionGetRank) {
  console.log("SAGA-RANK");
  try {
    const data: IRankInfo[] = yield call(beginFetchRank);
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
