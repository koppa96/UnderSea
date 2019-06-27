import { RankState } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  totalRank: RankState;
}
export interface DispachedProps {
  getAllBuilding: () => void;
}

export type RankProps = NativeProps & MappedProps & DispachedProps;
