import style, {getVariant} from './style';
import {Text} from "react-native";

export default function StyledText(props) {
    const {variant, style : userStyle} = props;

    return (
        <Text {...props} style={[style.text, getVariant('text', variant), userStyle]}/>
    );
}
