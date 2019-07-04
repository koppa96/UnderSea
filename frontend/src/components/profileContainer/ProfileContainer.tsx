import * as React from "react";
import { Link } from "react-router-dom";
import { ProfileProps } from "./Interface";
import { BasePortUrl } from "../..";
import { Modal } from "reactstrap";

var img: FileList;
export class ProfileContainer extends React.Component<ProfileProps> {
  componentDidMount() {
    document.title = "Profil";
    this.props.getUserInfo();
  }
  state = {
    showImg: false,
    fileName: "",
    file: img
  };
  toggleImgChange = () => {
    this.setState({
      showImg: !this.state.showImg
    });
  };

  handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    e.preventDefault();
    this.setState({ fileName: e.target.value, file: e.target.files });
  };
  render = () => {
    const { profile } = this.props.profile;
    const { event } = this.props;
    const { fileName } = this.state;
    return (
      <div className="profile-bg">
        {event && <p onClick={() => this.props.togglePopup()}>+</p>}

        <div className="rectangle" onClick={() => this.toggleImgChange()}>
          <img
            title="Profil kép módosítás"
            alt="profile"
            src={BasePortUrl + profile.profileImageUrl}
          />
        </div>
        <Link to="/">
          <span className="profil-margin">
            {profile.username ? profile.username : ""}
          </span>
        </Link>
        <Link className="profil-margin" to="/account/profile">
          Profil
        </Link>
        <span onClick={this.props.logout} className="logout">
          Kijelentkezés
        </span>

        <Modal
          className="main-component popup"
          contentClassName="main-component popup"
          isOpen={this.state.showImg}
          toggle={() => this.toggleImgChange()}
        >
          <div className="popup-content">
            <div className="">
              <h1>Profil</h1>
              <h3>Profil kép választása</h3>

              <input
                onChange={event => this.handleChange(event)}
                type="file"
                name="file"
                id="file"
                className="input-type"
              />
              <label htmlFor="file" className="input-container">
                <div className="browse-btn">
                  {fileName ? fileName : "Válassz képet"}
                </div>
              </label>
            </div>
            <button
              onClick={() =>
                this.props.uploadImage({
                  name: this.state.fileName,
                  file: this.state.file
                })
              }
            >
              Feltöltés
            </button>
          </div>
          <div className="rectangle popup-rectangle">
            <img alt="profile" src={BasePortUrl + profile.profileImageUrl} />
          </div>
        </Modal>
      </div>
    );
  };
}
