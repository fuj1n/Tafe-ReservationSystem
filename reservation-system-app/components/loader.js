import {View, ActivityIndicator} from "react-native";
import styles from "../pages/styles";
import {variants} from "./style";

export default function Loader(props) {
    if(props.loading) {
        return (
            <View style={[styles.container, {paddingTop: 12}]}>
                <ActivityIndicator size="large" color={variants.Primary.color} />
            </View>
        )
    }

    return props.children;
}
