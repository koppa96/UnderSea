import { IUserInfo } from "../../../api/Client";

export interface ProfileState {
  profile: IUserInfo;
  loading: boolean;
  error?: string;
}

export const profileInitialState: ProfileState = {
  profile: {},
  loading: false
};
