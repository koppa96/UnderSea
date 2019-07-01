import { ArmyState } from "./store/store";
import { IRequestParamState } from "./store/actions/ArmyActions.post";

interface NativeProps {
  isNative: boolean;
}

export interface MappedProps {
  ownedUnitState: ArmyState;
  count: Array<{ id: number; count: number }>;
  pearls: number;
}

export interface DispachedProps {
  addUnits: (params: IRequestParamState) => void;
  getArmy: () => void;
  resetUnits: () => void;
}

export type ArmyProps = NativeProps & MappedProps & DispachedProps;
