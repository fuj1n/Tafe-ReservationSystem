import {Platform, Pressable} from "react-native";
import {Text, View} from "react-native";
import style, {getVariant} from "./style";
import {useRef, useState} from "react";
import {useHover} from "@react-native-aria/interactions";

/**
 * @param props {{value: string, children: string, variant: string, onPress: () => void,
 * onLongPress: () => void, disabled: boolean, style: object?, textStyle: object}}
 */
export default function Button(props) {
    const {value, children, onPress, onLongPress, disabled} = props;
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

    return (
        <Pressable ref={hoverRef} onPressIn={() => setPressed(true)} onPressOut={() => setPressed(false)}
                   onPress={onPress} onLongPress={onLongPress} disabled={disabled}>
            <View style={[style.button, getVariant('button', variant, hovered), props.style]}>
                <Text style={[style.buttonText, getVariant('buttonText', variant, hovered), props.textStyle]}>{text}</Text>
            </View>
        </Pressable>
    );
}
