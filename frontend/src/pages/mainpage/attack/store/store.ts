import { ITargetInfo } from "../../../../api/Client";
export interface TargetState {
  isPostRequesting: boolean;
  targets: ITargetInfo[];
  loading: boolean;
  error?: string;
}

export const targetInitialState: TargetState = {
  isPostRequesting: false,
  targets: [],
  loading: false
};
