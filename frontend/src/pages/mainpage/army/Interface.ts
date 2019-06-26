import { ArmyState } from "./store/store";
import { IRequestParamState } from "./store/actions/ArmyActions";

interface NativeProps {
  isNative: boolean
}

export interface MappedProps {
  ownedUnitState: ArmyState;
}

export interface DispachedProps {
  addUnits: (params: IRequestParamState) => void;
}

export type ArmyProps = NativeProps & MappedProps & DispachedProps;

