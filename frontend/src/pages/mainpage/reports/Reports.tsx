import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { ReportsProps } from "./Interface";

export class Reports extends React.Component<ReportsProps> {
  componentDidMount() {
    document.title = title;
    this.props.getAllReports();
  }

  render() {
    console.log("Összes report", this.props.reports.report);
    return (
      <div className="main-component reports-width">
        <ComponentHeader title={title} />
      </div>
    );
  }
}
const title: string = "Csatajelentés";
