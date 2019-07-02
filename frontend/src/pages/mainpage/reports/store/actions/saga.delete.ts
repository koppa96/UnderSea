import { call, put, takeEvery } from "redux-saga/effects";

import axios from "axios";

import { registerAxiosConfig } from "../../../../../config/axiosConfig";
import { BasePortUrl } from "../../../../..";
import {
  DeleteReportActions,
  IActionRequestDeleteReport,
  fetchSucces,
  fetchError
} from "./ReportAction.delete";

const beginToDeleteReport = (id: number): Promise<void> | any => {
  console.log("Beginig buy building", id);

  const url = BasePortUrl + "api/Reports/" + id;
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .delete(url)
    .then(response => {
      return response;
    })
    .catch(error => {
      throw new Error(error);
    });
};
function* handleDeleteReport(action: IActionRequestDeleteReport) {
  try {
    yield call(beginToDeleteReport, action.params.id);
    yield put(fetchSucces(action.params));
  } catch (err) {
    if (err) {
      yield put(fetchError("Sajnos valami hiba történt törlés közben"));
    } else {
      yield put(fetchError("Ismeretlen hiba"));
    }
  }
}

export function* watchDeleteReportsRequest() {
  yield takeEvery(DeleteReportActions.REQUEST, handleDeleteReport);
}
