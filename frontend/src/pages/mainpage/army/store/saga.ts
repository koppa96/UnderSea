import {
  IActionAddArmyUnitRequest,
  ArmyActions,
  IArmyActions,
  fetchSuccess,
  fetchError
} from "./actions/ArmyActions.post";
import {
  ArmyActions as GetArmyActions,
  IActionGetArmyUnitRequest,
  ArmyUnitGetSuccessActionCreator
} from "./actions/ArmyActions.get";
import { call, put, takeEvery, all } from "redux-saga/effects";
import { ArmyUnit } from "./store";
import axios from "axios";
import { BasePortUrl } from "../../../..";
import { registerAxiosConfig } from "../../../../config/axiosConfig";
export const asd = 0;

// TODO: create error handling (dont use any)
const beginAddUnits = (unitsToAdd: ArmyUnit[]): Promise<IArmyActions> | any => {
  const url = BasePortUrl + "api/Units";
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);

  return configured
    .post(url, unitsToAdd)
    .then(response => {
      return response.data;
    })
    .catch(error => {
      throw new Error(error);
    });
};

const getUnits = () => {
  const url = BasePortUrl + "api/Units";
  const instance = axios.create();
  const configured = registerAxiosConfig(instance);
  return configured
    .get(url)
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
    console.log(err);
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
