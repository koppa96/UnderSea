import { BriefCreationInfo, EventInfo, BriefUnitInfo } from "../../api/Client";

export interface NativeProps {
  mounted: Function;
}

export interface ICountryInfo {
  round: number;
  barrackSpace: number;
  rank: number;
  armyInfo?: BriefUnitInfo[] | undefined;
  pearls: number;
  corals: number;
  pearlsPerRound: number;
  coralsPerRound: number;
  event?: EventInfo | undefined;
  unseenReports: number;
  buildings?: BriefCreationInfo[] | undefined;
  researches?: BriefCreationInfo[] | undefined;
}

export interface MappedProps {
  error?: string;
  loading: boolean;
  building: BriefCreationInfo[];
  researches: BriefCreationInfo[];
  event?: EventInfo;
}

export interface DispatchedProps {
  beginFetchMainpage: () => void;
  refreshCountryInfo: (params: ICountryInfo) => void;
}

export type MainPageProps = NativeProps & MappedProps & DispatchedProps;
