import { call, put, takeEvery } from "redux-saga/effects";

import axios, { AxiosRequestConfig } from "axios";
import {
  PostProfileImgActions,
  IRequestActionPostProfileImg,
  fetchSucces,
  fetchError
} from "./uploadImage.post";
import { BasePortUrl } from "../../../..";
import { registerAxiosConfig } from "../../../../config/axiosConfig";

const beginToAddImage = (img: FileList): Promise<void> | any => {
  const url = BasePortUrl + "api/Accounts/me/image";
  const instance = axios.create();
  let data = new FormData();
  data.append("file", img[0], img[0].name);
  instance.defaults.headers.common = {
    Authorization: localStorage.getItem("access_token"),
    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token"
  };
  console.log("kép");
  console.log(img);
  return instance
    .put(url, data)
    .then(response => {
      var parse: string = JSON.stringify(response.data);
      parse = parse.split('"').join("");
      console.log("parse", parse);
      return parse;
    })
    .catch(error => {
      console.log("parse", error);
      throw new Error(error);
    });
};
function* handleAddImage(action: IRequestActionPostProfileImg) {
  try {
    const temp = yield call(beginToAddImage, action.params.file);

    yield put(fetchSucces(temp));
  } catch (err) {
    if (err) {
      yield put(fetchError("Sajnos valami hiba történt vásárlás közben"));
    } else {
      yield put(fetchError("Ismeretlen hiba"));
    }
  }
}

export function* watchPostImageRequest() {
  yield takeEvery(PostProfileImgActions.REQUEST, handleAddImage);
}
