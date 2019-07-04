import { call, put, takeEvery } from "redux-saga/effects";
import {
  IRequestActionGetProfile,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  GetProfileActions
} from "./profileContainer.get";
import { registerAxiosConfig } from "../../../../config/axiosConfig";
import axios from "axios";
import { BasePortUrl } from "../../../..";
import { IUserInfo } from "../../Interface";

const beginFetchUser = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Accounts/me");

    return response.data;
  } catch (error) {
    throw new Error(error);
  }
};

function* handleFetch(action: IRequestActionGetProfile) {
  try {
    const data: IUserInfo = yield call(beginFetchUser);
    const response: ISuccesParamState = { profile: data };
    yield put(fetchSucces(response));
  } catch (err) {
    if (err) {
      yield put(fetchError("Hiba történt"));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchProfileFetchRequest() {
  yield takeEvery(GetProfileActions.REQUEST, handleFetch);
}
