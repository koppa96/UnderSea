import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";

import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";
import {
  PostReportActions,
  IActionRequestPostReport,
  fetchSucces,
  fetchError
} from "./ReportAction.post";

const beginToSeenReport = (id: number): Promise<void> | any => {
  console.log("Beginig buy building", id);

  const url = BasePortUrl + "api/Reports/seen/" + id;
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .post(url)
    .then(response => {
      return response;
    })
    .catch(error => {
      throw new Error(error);
    });
};
function* handleSeenReport(action: IActionRequestPostReport) {
  try {
    yield call(beginToSeenReport, action.params);
    yield put(fetchSucces(action.params));
  } catch (err) {
    if (err) {
      yield put(fetchError("Sajnos valami hiba történt nézés közben"));
    } else {
      yield put(fetchError("Ismeretlen hiba"));
    }
  }
}

export function* watchSeenReportsRequest() {
  yield takeEvery(PostReportActions.REQUEST, handleSeenReport);
}
