import { call, put, takeEvery } from "redux-saga/effects";
import { ResearchesClient } from "../../../../../api/Client";
import {
  fetchError,
  IActionRequestAddDevelopment,
  fetchSucces,
  AddDevelopmentActions
} from "./DevelopmentAction.post";

export const beginToAddResearch = (id: number): Promise<void> => {
  const startResearch = new ResearchesClient();
  console.log(id, "elkezdtem, a fejlesztést");
  return startResearch.startResearch(id);
};

function* handleAddResearch(action: IActionRequestAddDevelopment) {
  try {
    yield call(beginToAddResearch, action.params);
    yield put(fetchSucces());
  } catch (err) {
    if (err) {
      yield put(fetchError(err));
    } else {
      yield put(fetchError("An unknown error occured."));
    }
  }
}

export function* watchAddResearchRequest() {
  yield takeEvery(AddDevelopmentActions.REQUEST, handleAddResearch);
}
