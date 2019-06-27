import { ProfileContainer } from "./ProfileContainer";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { IApllicationState, resetEverything } from "../../store";
import { GetProfileActionCreator } from "./store/actions/profileContainer.get";
import { DispachedProps, MappedProps } from "./Interface";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  return {
    profile: state.app.pages.profile
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getUserInfo: GetProfileActionCreator,
      logout: resetEverything
    },

    dispatch
  );

export const ProfileContainerConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(ProfileContainer);
