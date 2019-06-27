import { ICommandInfo } from "../../../../api/Client";

export interface WarState {
  war: ICommandInfo[];
  loading: boolean;
  error?: string;
}

export const WarInitialState: WarState = {
  war: [],
  loading: false
};
