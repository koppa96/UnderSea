import { IRankInfo } from "../../../api/Client";

interface NativeProps {}

export interface MappedProps {
  totalRank: IRankInfo[];
}
export interface DispachedProps {
  getAllBuilding: () => void;
}

export type RankProps = NativeProps & MappedProps & DispachedProps;
