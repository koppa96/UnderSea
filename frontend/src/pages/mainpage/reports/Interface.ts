import { ICombatInfo } from "./store/actions/ReportAction.get";
import { ReportState } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  reports: ReportState;
}
export interface DispachedProps {
  getAllReports: () => void;
  // deleteReport:(id:number)=>void
}

export type ReportsProps = NativeProps & MappedProps & DispachedProps;
