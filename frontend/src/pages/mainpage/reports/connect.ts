import { IApllicationState } from "../../../store";
import { bindActionCreators, Dispatch } from "redux";
import { Reports } from "./Reports";
import { connect } from "react-redux";
import { DispachedProps, MappedProps } from "./Interface";
import { GetReportRequestActionCreator } from "./store/actions/ReportAction.get";
import { PostReportActionCreator } from "./store/actions/ReportAction.post";
import { DeleteReportActionCreator } from "./store/actions/ReportAction.delete";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  console.log("reducerben", state.app.pages.reports);
  return {
    reports: state.app.pages.reports
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getAllReports: GetReportRequestActionCreator,
      seenReport: PostReportActionCreator,
      deleteReport:DeleteReportActionCreator
    },

    dispatch
  );

export const ReportsConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Reports);
