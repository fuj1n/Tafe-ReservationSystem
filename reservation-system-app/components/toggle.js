import {useState} from "react";
import {Text} from "react-native";
import MaterialIcons from "@expo/vector-icons/MaterialIcons";
import MaterialCommunityIcons from "@expo/vector-icons/MaterialCommunityIcons";
import style, {getVariant} from "./style";
import {RectButton} from "react-native-gesture-handler";

function Checkbox(props) {
    const {checked} = props;
    const variant = props.variant ?? "dark"

    const icon = checked ? "check-box" : "check-box-outline-blank";
    const fg = checked ? getVariant("fg", variant, false) : {};

    return <MaterialIcons name={icon} size={28} style={[style.icon28, fg]}/>
}

function Switch(props) {
    const {checked} = props;
    const variant = props.variant ?? "primary"

    const icon = checked ? "toggle-switch" : "toggle-switch-off-outline";
    const fg = checked ? getVariant("fg", variant, false) : {};

    return <MaterialCommunityIcons name={icon} size={28} style={[style.icon28, fg]}/>
}

/**
 * @typedef toggleMode {"checkbox"|"switch"}
 * @param props {{mode : toggleMode, value : boolean, onChange : function(boolean), variant? : string}}
 */
export default function Toggle(props) {
    const {mode, label} = props;

    const [checked, setChecked] = useState(props.value ?? false);

    function checkChange() {
        props.onChange?.(!checked);
        setChecked(!checked);
    }

    let ToggleComponent;
    switch (mode ?? "checkbox") {
        case "checkbox":
            ToggleComponent = Checkbox;
            break;
        case "switch":
            ToggleComponent = Switch;
            break;
        default:
            throw new Error("Unknown toggle mode");
    }

    return (
        <RectButton style={[props.style, style.inputContainer, {flexDirection: 'row', alignItems: 'center', paddingVertical: 12}]} onPress={checkChange} rippleColor='#DDDDDD'>
            <ToggleComponent checked={checked} {...props} />
            {label && <Text style={{paddingLeft: 6}}>{label}</Text>}
        </RectButton>
    );
}
