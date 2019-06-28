import { DevelopmentState } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  totalDevelopment: DevelopmentState;
}
export interface DispachedProps {
  //addDevelopment: (params: number) => void;
}

export type DevelopmentProps = NativeProps & MappedProps & DispachedProps;
