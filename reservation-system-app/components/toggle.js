import {useState} from "react";
import {Text} from "react-native";
import MaterialIcons from "@expo/vector-icons/MaterialIcons";

function Checkbox(props) {
    const {checked, onChange} = props;
    const icon = checked ? "check-box" : "check-box-outline-blank";
    return <><MaterialIcons name={icon}></MaterialIcons></>
}

function Switch(props) {
    return <></>;
}

/**
 * @typedef toggleMode {"checkbox"|"switch"}
 * @param props
 */
export default function Toggle(props) {
    const {mode} = props;

    const [checked, setChecked] = useState(false);

    let ToggleComponent;
    switch (mode ?? "checkbox") {
        case "checkbox":
            ToggleComponent = Checkbox;
            break;
        case "switch":
            ToggleComponent = Switch;
            break;
    }

    return (
        <ToggleComponent checked={checked} onChange={setChecked} {...props} />
    );
}
