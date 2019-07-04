export interface TargetState {
  targets: ITargetInfo[];
  isPostRequesting: boolean;
  isRequesting: boolean;
  isLoaded: boolean;
  error?: string;
}

export const targetInitialState: TargetState = {
  isPostRequesting: false,
  targets: [],
  isLoaded: false,
  isRequesting: false
};
export interface ITargetInfo {
  username?: string | undefined;
  countryId: number;
  countryName?: string | undefined;
}
