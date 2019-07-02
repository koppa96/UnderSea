import { ICombatInfo } from "./store/actions/ReportAction.get";
import { ReportState } from "./store/store";
import { removeReport } from "./store/actions/ReportAction.delete";

interface NativeProps {}

export interface MappedProps {
  reports: ReportState;
}
export interface DispachedProps {
  getAllReports: () => void;
  seenReport: (id: number) => void;
  deleteReport: (item: removeReport) => void;
}

export type ReportsProps = NativeProps & MappedProps & DispachedProps;
