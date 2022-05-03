import DropDownPicker from 'react-native-dropdown-picker';
import style from "./style";
import {Text, View} from "react-native";
import {useState} from "react";

export default function Dropdown(props) {
    const {label, zIndex = 100} = props;
    const [open, setOpen] = useState(false);

    return (
        <View style={[style.inputContainer, {zIndex: zIndex}]}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <DropDownPicker open={open} setOpen={setOpen} {...props}
                            style={style.dropdownInput} placeholderStyle={style.dropdownPlaceholder}
                            containerStyle={style.dropdownContainer} dropDownContainerStyle={style.dropdownDropdown}
                            listItemContainerStyle={style.dropdownItem}
                            selectedItemContainerStyle={style.dropdownSelectedItem}
                            itemSeparatorStyle={style.dropdownItemSeparator} itemSeparator={true}/>
        </View>
    );
}
