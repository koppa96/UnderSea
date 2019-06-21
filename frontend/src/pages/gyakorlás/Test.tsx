import * as React from "react";
import { TestProps } from "./Interface";

export class Test extends React.Component<TestProps> {
  render() {
    const { addHuman, isNative, humanState, removeHUman } = this.props;

    return (
      <div>
        <h1>{humanState.name}</h1>
        <p>{humanState.gender}</p>
        <p>{humanState.isOld ? "Öreg" : "Fiatal"}</p>
        <p>{humanState.age}</p>

        <button
          onClick={() => addHuman({ name: "Sanyi", age: 24, gender: "male" })}
        >
          Add
        </button>
        <button onClick={removeHUman}>Remove</button>
      </div>
    );
  }
}
