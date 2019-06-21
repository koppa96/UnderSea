import { BuildingState } from "./store/store";
import { IRequestParamState } from "./store/actions/buildingActions";

interface NativeProps {}

export interface MappedProps {
  buildingState: BuildingState;
}

export interface DispachedProps {
  addBuilding: (params: IRequestParamState) => void;
}

export type BuildingProps = NativeProps & MappedProps & DispachedProps;
