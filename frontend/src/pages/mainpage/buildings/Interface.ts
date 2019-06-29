import { BuildingState, RequestBuildingParams } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  ownedBuildingState: BuildingState;
  count: Array<{ id: number; count: number; inProgress: boolean }>;
  totalpearl: number;
  totalcoral: number;
}

export interface DispachedProps {
  addBuilding: (params: RequestBuildingParams) => void;
  getAllBuilding: () => void;
}

export type BuildingProps = NativeProps & MappedProps & DispachedProps;
