import { call, put, takeEvery } from "redux-saga/effects";
import {
  IRequestActionGetProfile,
  ISuccesParamState,
  fetchSucces,
  fetchError,
  GetProfileActions
} from "./actions/profileContainer.get";
import { AccountsClient, IUserInfo } from "../../../api/Client";
import { registerAxiosConfig } from "../../../config/axiosConfig";
import axios from "axios";
import { BasePortUrl } from "../../..";

export const beginFetchBuilding = () => {
  const getProfileedList = new AccountsClient();
  const tempData = getProfileedList.getAccount();
  return tempData;
};

const beginFetchUser = async () => {
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  try {
    const response = await configured.get(BasePortUrl + "api/Accounts/me");
    console.log("profil fetched", response.data);

    return response.data;
  } catch (error) {
    console.log("profil fetch error", error);

    throw new Error(error);
  }
};

function* handleFetch(action: IRequestActionGetProfile) {
  console.log("SAGA-Profile");
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
