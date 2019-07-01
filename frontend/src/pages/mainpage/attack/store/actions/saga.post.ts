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
import { ICommandInfo } from "../../../war/store/actions/WarAction.get";
import { BasePortUrl } from "../../../../..";
export const asd = 0;

// TODO: create error handling (dont use any)
const beginAddUnits = (
  attackTarget: ICommandDetails
): Promise<IPostTargetActions> | any => {
  console.log("Army megvesz", attackTarget);
  const config = {
    headers: {
      Authorization: localStorage.getItem("access_token"),
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
    }
  };
  const url = BasePortUrl + "api/Commands";
  return axios
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
    console.log(err);
    const ErrorMEssage: string = "Sajnos valami hiba történt vásárlás közben";

    yield put(fetchError(ErrorMEssage));
  }
}

export function* watcAttackTargetRequest() {
  yield takeEvery(PostAttackActions.REQUEST, handleAttackTarget);
}
