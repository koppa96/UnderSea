import { ICommandInfo } from "./actions/WarAction.get";

export interface WarState {
  war: ICommandInfo[];
  isPostRequesting: boolean;
  isRequesting: boolean;
  isLoaded: boolean;
  error?: string;
}

export const WarInitialState: WarState = {
  war: [],
  isPostRequesting: false,
  isRequesting: false,
  isLoaded: false
};
