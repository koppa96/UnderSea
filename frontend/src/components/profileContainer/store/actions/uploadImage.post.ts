//ACTIONTYPES
export interface IPostProfileImgActionsTypes {
  REQUEST: "IMAGE_REQUEST_POST_PROFILE_IMAGE";
  SUCCES: "IMAGE_SUCCES_POST_PROFILE__IMAGE";
  ERROR: "IMAGE_ERROR_POST_PROFILE__IMAGE";
}

export const PostProfileImgActions: IPostProfileImgActionsTypes = {
  REQUEST: "IMAGE_REQUEST_POST_PROFILE_IMAGE",
  SUCCES: "IMAGE_SUCCES_POST_PROFILE__IMAGE",
  ERROR: "IMAGE_ERROR_POST_PROFILE__IMAGE"
};

export interface PostProfileInput {
  name: string;
  file: FileList;
}

//ACTIONHOZ
export interface IRequestActionPostProfileImg {
  type: IPostProfileImgActionsTypes["REQUEST"];
  params: PostProfileInput;
}
export interface ISuccesActionPostProfileImg {
  type: IPostProfileImgActionsTypes["SUCCES"];
  data: string;
}
export interface IErrorActionPostProfileImg {
  type: IPostProfileImgActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IPostProfileActions =
  | IRequestActionPostProfileImg
  | ISuccesActionPostProfileImg
  | IErrorActionPostProfileImg;

//ACTIONCREATORHOZ
export const PostProfileImgActionCreator = (
  params: PostProfileInput
): IRequestActionPostProfileImg => ({
  type: PostProfileImgActions.REQUEST,
  params
});

export const fetchError = (error?: string): IErrorActionPostProfileImg => ({
  type: PostProfileImgActions.ERROR,
  error
});
export const fetchSucces = (data: string): ISuccesActionPostProfileImg => ({
  type: PostProfileImgActions.SUCCES,
  data
});
