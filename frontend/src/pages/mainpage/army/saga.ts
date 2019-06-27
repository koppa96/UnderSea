import {
  IActionAddArmyUnitRequest,
  ArmyActions,
  IActions,
  fetchSuccess,
  fetchError
} from "./store/actions/ArmyActions.post";
import {
  ArmyActions as GetArmyActions,
  IActionGetArmyUnitRequest,
  ArmyUnitGetSuccessActionCreator,
  IGetArmyRespone
} from "./store/actions/ArmyActions.get";
import { call, put, takeEvery, all, delay } from "redux-saga/effects";
import { ArmyUnit } from "./store/store";
import axios from "axios";
import { UnitsClient, PurchaseDetails } from "./../../../api/Client";
export const asd = 0;

// TODO: create error handling (dont use any)
const beginAddUnits = (unitsToAdd: ArmyUnit[]): Promise<IActions> | any => {
  const config = {
    headers: {
      Authorization: localStorage.getItem("access_token"),
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
    }
  };
  const url = "api/Units";
  return axios
    .post(url, unitsToAdd)
    .then(response => {
      console.log(response);
      return response;
    })
    .catch(error => {
      throw new Error(error);
    });

  //         const axiosClient = new UnitsClient
  // const data = new PurchaseDetails(unit)
  // return axiosClient.create(unitsToAdd)
};

const getUnits = () => {
  return axios
    .get("/api/Units")
    .then(response => {
      return response.data;
    })
    .catch(error => {
      throw new Error(error);
    });
};

function* handleArmyGetUnits(action: IActionGetArmyUnitRequest) {
  try {
    const response = yield call(getUnits);
    yield put(ArmyUnitGetSuccessActionCreator(response));
  } catch (err) {
    const ErrorMEssage: string = "Sajnos valami hiba történt betöltés közben";

    yield put(fetchError(ErrorMEssage));
  }
}

function* handleArmyAddUnits(action: IActionAddArmyUnitRequest) {
  const params = action.params;
  try {
    const response = yield call(beginAddUnits, action.params.unitsToAdd);
    yield put(fetchSuccess(params));
  } catch (err) {
    const ErrorMEssage: string = "Sajnos valami hiba történt vásárlás közben";

    yield put(fetchError(ErrorMEssage));
  }
}

export function* watchArmyAddUnitsRequest() {
  yield takeEvery(ArmyActions.REQUEST, handleArmyAddUnits);
}

export function* watchArmyGetUnitsRequest() {
  yield takeEvery(GetArmyActions.REQUEST, handleArmyGetUnits);
}

export function* watchArmyUnits() {
  yield all([watchArmyAddUnitsRequest(), watchArmyGetUnitsRequest()]);
}
