import {Platform} from "react-native";
import {Text, View, StyleSheet} from "react-native";
import style, {getVariant} from "./style";
import {useRef, useState} from "react";
import {useHover} from "@react-native-aria/interactions";
import {RectButton} from "react-native-gesture-handler";

/**
 * @param props {{value: string, children: string, variant: string, onPress: () => void,
 * onLongPress: () => void, disabled: boolean, style: object?, textStyle: object}}
 */
export default function Button(props) {
    const {value, children} = props;
    const [pressed, setPressed] = useState(false);

    let hovered = pressed;
    let hoverRef = undefined;

    // Simulate press in browser
    if(Platform.OS === 'web') {
        hoverRef = useRef();
        const {isHovered} = useHover({}, hoverRef);
        hovered = hovered || isHovered;
    }

    const variant = props.variant ?? "primary";

    let text = value;
    if(typeof(children) === 'string') {
        text = children;
    }

    // noinspection JSUnresolvedVariable - definitely resolved
    const ripple = StyleSheet.flatten(getVariant('button', variant, true))?.backgroundColor;

    return (
        <RectButton ref={hoverRef} onActiveStateChange={setPressed}
            {...props} style={props.style} rippleColor={ripple}>
            <View style={[style.button, getVariant('button', variant, hovered)]}>
                <Text style={[style.buttonText, getVariant('buttonText', variant, hovered), props.textStyle]}>{text}</Text>
            </View>
        </RectButton>
    );
}
