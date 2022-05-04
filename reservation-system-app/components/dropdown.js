import style from "./style";
import {Text, View} from "react-native";
import {Picker} from "@react-native-picker/picker";

export default function Dropdown(props) {
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
