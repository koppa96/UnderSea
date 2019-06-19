import * as React from "react";
import { Link } from "react-router-dom";

export class ProfileContainer extends React.Component {
  render = () => {
    return (
      <div className="profile-bg">
        <div className="rectangle">
          <div>asd</div>
        </div>
        <span>jakabjancsi</span>
        <Link to="/profil">Profil</Link>
        <span>Kijelentkezés</span>

      </div>
    );
  };
}
