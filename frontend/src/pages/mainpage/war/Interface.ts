import { WarState } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  totalWar: WarState;
}
export interface DispachedProps {
  getAllWar: () => void;
  deleteById: (params: number) => void;
}

export type WarProps = NativeProps & MappedProps & DispachedProps;
