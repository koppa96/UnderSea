import { IUserInfo } from "../Interface";

export interface ProfileState {
  profile: IUserInfo;
  loading: boolean;
  error?: string;
}

export const profileInitialState: ProfileState = {
  profile: {},
  loading: false
};
