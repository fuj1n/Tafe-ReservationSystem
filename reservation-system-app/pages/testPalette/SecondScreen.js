import {View} from "react-native";
import styles from "../styles";
import {Button, StyledText} from "../../components";

export default function SecondScreen(props) {
    const {navigation} = props;

    return (
        <View style={styles.container}>
            <StyledText style={styles.containerItem} variant="danger">Second screen</StyledText>
            <Button style={styles.containerItem} variant="primary" onPress={() => navigation.goBack()}>Go back</Button>
        </View>
    )
}
