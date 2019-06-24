import { ManState } from "./store/store";
import { IRequestParamState } from "./store/actions/testAction";

interface NativeProps {
  isNative: boolean;
}

export interface MappedProps {
  humanState: ManState;
}

export interface DispachedProps {
  addHuman: (params: IRequestParamState) => void;
  removeHUman: () => void;
}

export type TestProps = NativeProps & MappedProps & DispachedProps;
