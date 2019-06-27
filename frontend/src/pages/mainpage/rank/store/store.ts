import { IRankInfo } from "../../../../api/Client";

export interface RankState {
  rank: IRankInfo[];
  loading: boolean;
  error?: string;
}


export const rankInitialState: RankState = {
    rank: [],
    loading: false
  };
  