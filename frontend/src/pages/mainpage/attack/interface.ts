import { ITargetInfo, IBriefUnitInfo, ICommandInfo } from "../../../api/Client";

export interface Attack {
  id: number;
  name: string;
  checked: boolean;
}
interface OutProp {
  unitId: number;
  count: number;
}

export interface NativeProps {}

export interface MappedProps {
  targets: ITargetInfo[];
  unit: IBriefUnitInfo[];
}

export interface DispatchedProps {
  getTargets: () => void;
  attackTarget: (params: ICommandInfo) => void;
}

export type TargetProps = NativeProps & MappedProps & DispatchedProps;
