import axios from "axios";
import {
  PostAttackActions,
  IRequestActionPostTarget,
  IPostTargetActions,
  fetchError,
  fetchSucces,
  ICommandDetails
} from "./AddAttackAction.post";
import { call, put, takeEvery } from "redux-saga/effects";
import { BasePortUrl } from "../../../../..";
import { registerAxiosConfig } from "../../../../../config/axiosConfig";

// TODO: create error handling (dont use any)
const beginAddUnits = (
  attackTarget: ICommandDetails
): Promise<IPostTargetActions> | any => {
  const url = BasePortUrl + "api/Commands";
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .post(url, attackTarget)
    .then(response => {
      return response;
    })
    .catch(error => {
      throw new Error(error);
    });
};
function* handleAttackTarget(action: IRequestActionPostTarget) {
  try {
    const response = yield call(beginAddUnits, action.params);
    yield put(fetchSucces(response.data));
  } catch (err) {
    const ErrorMEssage: string = "Sajnos valami hiba történt vásárlás közben";

    yield put(fetchError(ErrorMEssage));
  }
}

export function* watcAttackTargetRequest() {
  yield takeEvery(PostAttackActions.REQUEST, handleAttackTarget);
}
