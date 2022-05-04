import {Text, View, StyleSheet} from "react-native";
import style, {getVariant} from "./style";
import {useState} from "react";
import {RectButton} from "react-native-gesture-handler";

/**
 * @param props {{value: string, children: string, variant: string, onPress: () => void,
 * disabled: boolean, style: object?, btnStyle: object?, textStyle: object}}
 */
export default function Button(props) {
    const {value, children} = props;
    const [active, setActive] = useState(false);

    const variant = props.variant ?? "primary";

    let text = value;
    if(typeof(children) === 'string') {
        text = children;
    }

    // noinspection JSUnresolvedVariable - definitely resolved
    const ripple = StyleSheet.flatten(getVariant('button', variant, true))?.backgroundColor;

    return (
        <RectButton onActiveStateChange={setActive}
            {...props} style={props.style} rippleColor={ripple}>
            <View style={[style.button, getVariant('button', variant, active), props.btnStyle]}>
                <Text style={[style.buttonText, getVariant('buttonText', variant, active), props.textStyle]}>{text}</Text>
            </View>
        </RectButton>
    );
}
