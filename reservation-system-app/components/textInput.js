import {TextInput as NativeTextInput} from "react-native";
import style from "./style";
import {Text, View} from "react-native";

/**
 * @param props {{label: string, value: string, placeholder: string, onChangeText: function, editable: boolean,
 * placeholder: string, keyboardType: string, keyboardType: string, autoComplete: string, secureTextEntry: boolean}}
 */
export default function TextInput(props) {
    const {label} = props;

    return (
        <View style={style.textInputContainer}>
            {label && <Text style={style.textInputLabel}>{label}</Text>}
            <NativeTextInput placeholderTextColor="#6c757d" {...props} style={[style.textInput, props.style]}/>
        </View>
    );
}
