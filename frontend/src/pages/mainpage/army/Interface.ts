import { ArmyState } from "./store/store";
import { IRequestParamState } from "./store/actions/ArmyActions.post";

interface NativeProps {
  isNative: boolean;
}

export interface MappedProps {
  ownedUnitState: ArmyState;
}

export interface DispachedProps {
  addUnits: (params: IRequestParamState) => void;
  getArmy: () => void;
}

export type ArmyProps = NativeProps & MappedProps & DispachedProps;
