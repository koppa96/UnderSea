import { call, put, takeEvery } from "redux-saga/effects";
import { ResearchesClient } from "../../../../../api/Client";
import {
  AddDevelopmentErrorActionCreator,
  IActionRequestAddDevelopment,
  AddDevelopmentSuccessActionCreator,
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
    yield put(AddDevelopmentSuccessActionCreator());
  } catch (err) {
    if (err) {
      yield put(AddDevelopmentErrorActionCreator(err));
    } else {
      yield put(AddDevelopmentErrorActionCreator("An unknown error occured."));
    }
  }
}

export function* watchAddResearchRequest() {
  yield takeEvery(AddDevelopmentActions.REQUEST, handleAddResearch);
}
