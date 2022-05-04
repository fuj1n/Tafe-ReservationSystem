import {TextInput as NativeTextInput} from "react-native";
import style from "./style";
import {Text, View} from "react-native";

/**
 * @param props {{label: string, value: Date, setValue: function(Date)}}
 */
export default function TimeSlotPicker(props) {
    //TODO: proper date picker

    const {label} = props;

    function onChange(value) {
        props.setValue?.(new Date(value));
    }

    return (
        <View style={style.inputContainer}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <NativeTextInput placeholderTextColor="#6c757d" {...props} style={[style.textInput]} onChangeText={onChange}/>
        </View>
    );
}
