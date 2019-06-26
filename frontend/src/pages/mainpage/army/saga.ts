import {IActionAddArmyUnitRequest, ArmyActions, IActionAddArmyUnitSucces, IRequestParamState, IActions, fetchSuccess, fetchError } from './store/actions/ArmyActions'
import { call, put, takeEvery } from "redux-saga/effects";
import { ArmyUnit } from './store/store';
import  axios  from 'axios'
import {UnitsClient} from './../../../api/Client'
export const asd=0;

/*const beginAddUnits = (unitsToAdd:ArmyUnit[]):Promise<IActions> => {
    const config = {
        headers: {
            "Authorization": localStorage.getItem("access_token"),
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
        }
      };
    const url = "api/Units"
    const axiosClient:UnitsClient = new UnitsClient
    const resp = axiosClient.create()
    return resp;
}

function* handleArmyAddUnits(action: IActionAddArmyUnitRequest) {
    try{
        const caller: string = yield call(
            beginAddUnits,
            action.params.unitsToAdd
        )
        yield put(fetchSuccess(caller))
    } catch(err) {
        yield put(fetchError(err))
    }
}

export function* watchArmyAddUnitsRequest(){
    yield takeEvery(ArmyActions.REQUEST, handleArmyAddUnits)
}*/