import { IRankInfo } from "./actions/RankAction.get";

export interface RankState {
  rank: IRankInfo[];
  isRequesting: boolean;
  isLoaded: boolean;
  error?: string;
}

export const rankInitialState: RankState = {
  rank: [],
  isRequesting: false,
  isLoaded: false
};
