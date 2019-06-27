import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { GetRankActionCreator } from "./store/actions/RankAction.get";
import { DispachedProps, MappedProps } from "./Interface";
import { Rank } from "./Rank";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  return {
    totalRank: state.app.pages.rank.rank
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getAllBuilding: GetRankActionCreator
    },

    dispatch
  );

export const RankConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Rank);
