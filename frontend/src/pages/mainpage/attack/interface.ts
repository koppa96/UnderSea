import { TargetState } from "./store/store";
import { ICommandDetails } from "./store/actions/AddAttackAction.post";

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
  setTrop?: Function;
}

export interface IUnitDetails {
  unitId: number;
  amount: number;
}

export interface NativeProps {}

export interface MappedProps {
  targets: TargetState;
  units: defendingTrop[];
}

export interface DispatchedProps {
  getTargets: () => void;
  attackTarget: (params: ICommandDetails) => void;
}

export type TargetProps = NativeProps & MappedProps & DispatchedProps;
