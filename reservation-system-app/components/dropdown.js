import style from "./style";
import {Platform, Text, View} from "react-native";
import {Picker} from "@react-native-picker/picker";
import RNPickerSelect from 'react-native-picker-select';


function DropdownIOS(props) {
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

/**
 * @param props {{label : string, items : [{label : string, value, color?: ColorValue}], children, style,
 * onValueChange : function(string), selectedValue, enabled : boolean, forceIosMode : boolean}}
 * @remarks props.forceIosMode is used to force iOS mode even if the platform is not iOS, and is intended for testing purposes only
 */
export default function Dropdown(props) {
    if(Platform.OS === "ios" || props.forceIosMode) {
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

export const DropdownItem = Picker.Item;
