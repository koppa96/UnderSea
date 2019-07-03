import { call, put, takeEvery } from "redux-saga/effects";

import { BasePortUrl } from "../../../../..";
import axios from "axios";
import {
  IRequestActionGetReport,
  ICombatInfo,
  GetReportSuccessActionCreator,
  GetReportErrorActionCreator,
  GetReportActions
} from "./ReportAction.get";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

export const beginFetchReport = () => {
  const url = BasePortUrl + "api/Reports";
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .get(url)
    .then(response => {
      return response.data;
    })
    .catch(error => {});
};

function* handleFetch(action: IRequestActionGetReport) {
  try {
    const data: ICombatInfo[] = yield call(beginFetchReport);
    yield put(GetReportSuccessActionCreator(data));
  } catch (err) {
    if (err) {
      yield put(GetReportErrorActionCreator("Hiba történt"));
    } else {
      yield put(GetReportErrorActionCreator("Ismeretlen hiba történt"));
    }
  }
}

export function* watchReportFetchRequest() {
  yield takeEvery(GetReportActions.REQUEST, handleFetch);
}
