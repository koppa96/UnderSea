import { call, put, takeEvery } from "redux-saga/effects";
import { AccountsClient, ITargetInfo } from "../../../../../api/Client";
import {
  fetchSucces,
  IRequestActionGetTarget,
  ISuccesParamState,
  fetchError,
  GetTargetActions
} from "./GetAttackAction.get";

export const beginFetchTargets = () => {
  const getTargetList = new AccountsClient();
  const tempData = getTargetList.getUsernames();
  return tempData;
};

function* handleFetch(action: IRequestActionGetTarget) {
  console.log("SAGA-Target");
  try {
    const data: ITargetInfo[] = yield call(beginFetchTargets);
    const response: ISuccesParamState = { targets: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchTargetFetchRequest() {
  yield takeEvery(GetTargetActions.REQUEST, handleFetch);
}
