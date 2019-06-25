import { ArmyState } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  ownedUnitState: ArmyState;
}

export interface DispachedProps {
  //addTroops: (params: IRequestParamState) => void;
}

export type ArmyProps = NativeProps & MappedProps & DispachedProps;

