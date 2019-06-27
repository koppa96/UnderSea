import { BuildingState } from "./store/store";
import {
  IBriefCreationInfo,
  BriefCreationInfo,
  ICreationInfo
} from "../../../api/Client";

interface NativeProps {}

export interface MappedProps {
  boughtBuildingState?: BuildingState;
  buildingCount?: countProp[];
  totalpearl: number;
  totalcoral: number;
}
export interface countProp {
  id: number;
  count: number;
}

export interface DispachedProps {
  addBuilding: (params: number) => void;
  getAllBuilding: () => void;
}

export type BuildingProps = NativeProps & MappedProps & DispachedProps;
