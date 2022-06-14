import {TextInput as NativeTextInput} from "react-native";
import style from "./style";
import {Text, View} from "react-native";

/**
 * @param props {{label: string, value: string, placeholder: string, onChangeText: function, editable: boolean,
 * placeholder: string, keyboardType: string, autoComplete: string, secureTextEntry: boolean, multiline: boolean}}
 */
export default function TextInput(props) {
    const {label} = props;

    let multilineStyle = null;
    if(props.multiline) {
        multilineStyle = {
            textAlignVertical: "top",
            height: 100,
        };
    }

    return (
        <View style={style.inputContainer}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <NativeTextInput placeholderTextColor="#6c757d" {...props} style={[style.textInput, multilineStyle, props.style]}/>
        </View>
    );
}
