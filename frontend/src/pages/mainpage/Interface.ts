import {
  ICountryInfo,
  IBriefCreationInfo,
  BriefCreationInfo
} from "../../api/Client";

export interface NativeProps {
  mounted: Function;
}

export interface MappedProps {
  error?: string;
  loading: boolean;
  building: BriefCreationInfo[];
}

export interface DispatchedProps {
  beginFetchMainpage: () => void;
  refreshCountryInfo: (params: ICountryInfo) => void;
}

export type MainPageProps = NativeProps & MappedProps & DispatchedProps;
