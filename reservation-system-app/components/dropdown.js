import style from "./style";
import {Platform, Text, View} from "react-native";

// Has to be done conditionally, otherwise crashes on mobile due to conflict between Picker and PickerSelect
let Picker;
let DropdownIOS = (_) => (<></>);
if(Platform.OS === "ios") {
    let RNPickerSelect;
    import("react-native-picker-select").then(module => {
        RNPickerSelect = module.default;
    });

    DropdownIOS = function DropdownIOS(props) {
        const {label, items, children, style: userStyle} = props;

        let itemsList;
        if(children) {
            itemsList = children.map(item => {
                return {label: item.props.label, value: item.props.value};
            });
        } else {
            itemsList = items;
        }

        return (
            <View style={[style.inputContainer, userStyle]}>
                {label && <Text style={style.inputLabel}>{label}</Text>}
                <View style={style.dropdownInput}>
                    <RNPickerSelect {...props} mode="dropdown"
                                    items={itemsList} placeholder={{}} value={props.selectedValue}
                                    style={{inputWeb: style.dropdownWebInputStyle}}/>
                </View>
            </View>
        );
    }
} else {
    import("@react-native-picker/picker").then(module => {
        Picker = module.Picker;
    });
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
    if(Platform.OS === "ios") {
        return <DropdownIOS {...props} />;
    }

    const {label, items, children, style: userStyle} = props;

    let itemsDom;
    if(children) {
        itemsDom = children;
    } else {
        itemsDom = items.map((item, index) => {
            return <Picker.Item key={index} {...item}/>
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
