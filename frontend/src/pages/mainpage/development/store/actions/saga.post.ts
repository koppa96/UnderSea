import { call, put, takeEvery } from "redux-saga/effects";

import {
  AddDevelopmentErrorActionCreator,
  IActionRequestAddDevelopment,
  AddDevelopmentSuccessActionCreator,
  AddDevelopmentActions
} from "./DevelopmentAction.post";
import axios from "axios";
import { BasePortUrl } from "../../../../..";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

export const beginToAddResearch = (id: number): Promise<void> => {
  console.log(id, "elkezdtem, a fejlesztést");
  const url = BasePortUrl + "api/Researches/" + id;
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .post(url)
    .then(response => {
      return response.data;
    })
    .catch(err => {
      throw new Error(err);
    });
};

function* handleAddResearch(action: IActionRequestAddDevelopment) {
  try {
    yield call(beginToAddResearch, action.params);
    yield put(AddDevelopmentSuccessActionCreator(action.params));
  } catch (err) {
    if (err) {
      yield put(AddDevelopmentErrorActionCreator(err));
    } else {
      yield put(AddDevelopmentErrorActionCreator("An unknown error occured."));
    }
  }
}

export function* watchAddResearchRequest() {
  yield takeEvery(AddDevelopmentActions.REQUEST, handleAddResearch);
}
