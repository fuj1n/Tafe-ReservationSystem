import {View} from "react-native";
import style, {getVariant} from "./style";

/**
 * @param props {{children: string, variant: string, style: object?}}
 */
export default function Badge(props) {
    const {children} = props;

    const variant = props.variant ?? "primary";

    return (
        <View style={[style.badge, getVariant('badge', variant, false), props.style]}>
            {children}
        </View>
    );
}
