import {View} from "react-native";
import StyledText from "./styledText";

/**
 * @remarks If used as a parent component, will block rendering of children components in the event of error.
 * @param props {{error: ErrorDesc, children: React.Node}}
 */
export default function ErrorDisplay(props) {
    const {error} = props;

    if (!error || !error.error) {
        return props.children ?? (<></>);
    }

    return (
        <View>
            <StyledText variant="danger">{error.message}</StyledText>
            {error.errors ? error.errors.map((error, index) => (
                <StyledText key={index} variant="danger">- {error.message}</StyledText>
            )) : null}
        </View>
    );
}
