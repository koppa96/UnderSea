import { ICountryInfo, BriefCreationInfo, EventInfo } from "../../api/Client";
import { IEventInfo } from "../../components/profileContainer/Interface";

export interface NativeProps {
  mounted: Function;
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
