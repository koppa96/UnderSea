import { call, put, takeEvery } from "redux-saga/effects";
import {
  IRequestActionGetProfile,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  GetProfileActions
} from "./actions/profileContainer.get";
import { AccountsClient, IUserInfo } from "../../../api/Client";

export const beginFetchBuilding = () => {
  const getProfileedList = new AccountsClient();
  const tempData = getProfileedList.getAccount();
  return tempData;
};

function* handleFetch(action: IRequestActionGetProfile) {
  console.log("SAGA-Profile");
  try {
    const data: IUserInfo = yield call(beginFetchBuilding);
    const response: ISuccesParamState = { profile: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchProfileFetchRequest() {
  yield takeEvery(GetProfileActions.REQUEST, handleFetch);
}
