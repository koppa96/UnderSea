interface NativeProps {}

export interface MappedProps {
  unseenReports: number;
}
export interface DispachedProps {}

export type MenuProps = NativeProps & MappedProps & DispachedProps;
