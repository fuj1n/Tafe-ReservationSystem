import style from "./style";
import {Platform, Text, View} from "react-native";
import MaterialIcons from "@expo/vector-icons/MaterialIcons";

// Has to be done conditionally, otherwise crashes on mobile due to conflict between Picker and PickerSelect
let Picker;
let DropdownIOS = (_) => (<></>);
if (Platform.OS === "ios") {
    let RNPickerSelect;
    RNPickerSelect = require("react-native-picker-select").default;

    DropdownIOS = function DropdownIOS(props) {
        const {label, items, children, style: userStyle} = props;

        let itemsList;
        if (children) {
            itemsList = children.map(item => {
                return {label: item.props.label, value: item.props.value};
            });
        } else {
            itemsList = items;
        }
        return (
            <View style={[style.inputContainer, userStyle]}>
                {label && <Text style={style.inputLabel}>{label}</Text>}
                <View style={[style.dropdownInput, style.dropdownIosInput]}>
                    <RNPickerSelect {...props}
                                    items={itemsList} placeholder={{}} value={props.selectedValue}
                                    style={{
                                        inputWeb: style.dropdownWebInputStyle,
                                        viewContainer: style.dropdownIosViewContainer
                                    }}/>
                    <MaterialIcons name="arrow-drop-down" size={24}/>
                </View>
            </View>
        );
    };
} else {
    Picker = require("@react-native-picker/picker").Picker;
}

/**
 * @param props {label : string, value : any}
 */
// Done due to the inability to re-export a dynamic import
export function DropdownItem(props) {
    return <Picker.Item {...props}/>;
}

/**
 * @param props {{label : string, items : [{label : string, value : any, color?: ColorValue}], children, style,
 * onValueChange : function(string), selectedValue, enabled : boolean}}
 */
export default function Dropdown(props) {
    if (Platform.OS === "ios") {
        return <DropdownIOS {...props} />;
    }

    const {label, items, children, style: userStyle} = props;

    let itemsDom;
    if (children) {
        itemsDom = children;
    } else {
        itemsDom = items.map((item, index) => {
            return <Picker.Item key={index} {...item}/>;
        });
    }

    return (
        <View style={[style.inputContainer, userStyle]}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <View style={style.dropdownInput}>
                <Picker {...props} style={style.dropdownWebInputStyle} mode="dropdown">
                    {itemsDom}
                </Picker>
            </View>
        </View>
    );
}
