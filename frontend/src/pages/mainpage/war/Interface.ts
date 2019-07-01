import { WarState } from "./store/store";
import { ICommandInfo } from "./store/actions/WarAction.get";

interface NativeProps {}

export interface MappedProps {
  totalWar: WarState;
}
export interface DispachedProps {
  getAllWar: () => void;
  deleteById: (params: ICommandInfo) => void;
}

export type WarProps = NativeProps & MappedProps & DispachedProps;
