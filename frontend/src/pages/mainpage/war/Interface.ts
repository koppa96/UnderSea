import { WarState } from "./store/store";

export interface DevelopmentProps {
  development: {
    id: string;
    country: string;
    trops: [];
  };
}
interface NativeProps {}

export interface MappedProps {
  totalWar: WarState;
}
export interface DispachedProps {
  getAllWar: () => void;
}

export type WarProps = NativeProps & MappedProps & DispachedProps;
