import {View} from "react-native";
import styles from "../styles";
import {Button, StyledText} from "../../components";
import Radio, {RadioGroup} from "../../components/radio";
import Toggle from "../../components/toggle";

export default function SecondScreen(props) {
    const {navigation} = props;

    return (
        <View style={styles.container}>
            <StyledText style={styles.containerItem} variant="danger">Second screen</StyledText>
            <Button style={styles.containerItem} variant="primary" onPress={() => navigation.goBack()}>Go back</Button>

            <RadioGroup style={styles.containerItem} label="Vertical Radio Group:" direction="column">
                <Radio label="Option One" value={1}/>
                <Radio label="Option Two" value={2}/>
            </RadioGroup>

            <RadioGroup style={styles.containerItem} label="Horizontal Radio Group:" direction="row">
                <Radio label="Option One" value={1}/>
                <Radio label="Option Two" value={2}/>
            </RadioGroup>

            <RadioGroup style={styles.containerItem} label="Vertical Button Radio Group:" direction="column" mode="button">
                <Radio label="Option One" value={1}/>
                <Radio label="Option Two" value={2}/>
            </RadioGroup>

            <RadioGroup style={styles.containerItem} label="Horizontal Button Radio Group:" direction="row" mode="button">
                <Radio label="Option One" value={1}/>
                <Radio label="Option Two" value={2}/>
            </RadioGroup>

            <Toggle style={styles.containerItem} label="Checkbox Toggle" mode="checkbox" />
        </View>
    )
}
