export interface NativeProps {}

export interface MappedProps {
  error?: string;
  loading: boolean;
}

export interface DispatchedProps {
  beginFetchMainpage: () => void;
}

export type MainPageProps = NativeProps & MappedProps & DispatchedProps;
