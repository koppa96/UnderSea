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

function* handleArmyGetUnits(action: IActionGetArmyUnitRequest) {
  try {
    // const response = yield call(API, nincs param);
    const response: IGetArmyRespone = [
      {
        id: 1,
        imageUrl: "asdasd",
        name: "Lézercápa",
        count: 10,
        attackPower: 5,
        defensePower: 5,
        maintenancePearl: 1,
        maintenanceCoral: 10,
        costPearl: 3
      },
      {
        id: 2,
        imageUrl: "asdasd",
        name: "Foka",
        count: 15,
        attackPower: 1,
        defensePower: 6,
        maintenancePearl: 1,
        maintenanceCoral: 10,
        costPearl: 3
      },
      {
        id: 3,
        imageUrl: "asdasd",
        name: "Csatacsikó",
        count: 20,
        attackPower: 5,
        defensePower: 5,
        maintenancePearl: 1,
        maintenanceCoral: 10,
        costPearl: 3
      }
    ];
    // TODO: Delete
    yield delay(2000);
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
