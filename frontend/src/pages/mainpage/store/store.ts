import { ICountryInfo } from "../../../api/Client";

//STATE
export interface MainpageResponseState {
  model?: ICountryInfo;
  error?: string;
  loading: boolean;
}

export const initialMainpageResponseState: MainpageResponseState = {
  error: undefined,
  loading: false,
  model: {
    round: 0,
    rank: 0,
    armyInfo: [],
    pearls: 0,
    corals: 0,
    pearlsPerRound: 0,
    coralsPerRound: 0,
    event: undefined,
    unseenReports: 0,
    buildings: [],
    researches: []
  }
};
