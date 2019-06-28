import { DevelopmentState } from "./store/store";
import { ICreationInfo, IBriefCreationInfo } from "../../../api/Client";
import { IRequestActionGetDevelopment } from "./store/actions/DevelopmnetAction.get";

interface NativeProps {}

export interface MappedProps {
  totalDevelopment: DevelopmentState;
  totalResourcesDesc: IBriefCreationInfo[];
}
export interface DispachedProps {
  //addDevelopment: (params: number) => void;
  getAllDevelopment: () => void;
}

export type DevelopmentProps = NativeProps & MappedProps & DispachedProps;

export interface DevelpomentDescription {
  id: number;
  name: string;
  description: string;
  cost: number;
}
export interface CombinedDevelopmnet {
  textLogic: IBriefCreationInfo;
  textUI: ICreationInfo;
}
