import { ITargetInfo, IBriefUnitInfo, ICommandInfo } from "../../../api/Client";
import { TargetState } from "./store/store";

export interface Attack {
  id: number;
  name: string;
  checked: boolean;
}
export interface defendingTrop {
  imageUrl: string | null;
  id: number;
  defendingCount: number;
  name: string;
}

export interface NativeProps {}

export interface MappedProps {
  targets: TargetState;
  units: defendingTrop[];
}

export interface DispatchedProps {
  getTargets: () => void;
  attackTarget: (params: ICommandInfo) => void;
}

export type TargetProps = NativeProps & MappedProps & DispatchedProps;
