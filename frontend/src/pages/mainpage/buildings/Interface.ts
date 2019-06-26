import { BuildingState } from "./store/store";
import { IRequestParamState } from "./store/actions/buildingActions";
import {
  IBriefCreationInfo,
  BriefCreationInfo,
  ICreationInfo
} from "../../../api/Client";

interface NativeProps {}

export interface MappedProps {
  boughtBuildingState?: BriefCreationInfo[];
}

export interface DispachedProps {
  addBuilding: (params: IRequestParamState) => void;
  getAllBuilding: () => void;
}

export type BuildingProps = NativeProps & MappedProps & DispachedProps;
