import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";
import {
  GetRankActions,
  IRequestActionGetRank,
  ISuccesParamState,
  fetchError,
  fetchSucces,
  IRankInfo
} from "./RankAction.get";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";

const beginFetchRank = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Accounts/ranked");
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
