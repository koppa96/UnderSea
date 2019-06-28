import { ITargetInfo, IBriefUnitInfo } from "../../../api/Client";

export interface Attack {
  id: number;
  name: string;
  checked: boolean;
}

export interface NativeProps {}

export interface MappedProps {
  targets: ITargetInfo[];
  unit: IBriefUnitInfo[];
}

export interface DispatchedProps {
  getTargets: () => void;
}

export type TargetProps = NativeProps & MappedProps & DispatchedProps;
