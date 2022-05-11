import styles from "../styles";
import {Button, StyledText, Toggle} from "../../components";
import Radio, {RadioGroup} from "../../components/radio";
import {ScrollView} from "react-native-gesture-handler";
import {useScrollToTop} from "@react-navigation/native";
import {useRef} from "react";

export default function SecondScreen(props) {
    const {navigation} = props;

    const variants = [
        "no variant",
        "primary",
        "secondary",
        "success",
        "danger",
        "warning",
        "info",
        "light",
        "dark"
    ];

    const ref = useRef(null);
    useScrollToTop(ref);

    return (
        <ScrollView ref={ref} contentContainerStyle={styles.container}>
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

            <RadioGroup style={styles.containerItem} label="Vertical Button Radio Group:" direction="column"
                        mode="button">
                <Radio label="Option One" value={1}/>
                <Radio label="Option Two" value={2}/>
            </RadioGroup>

            <RadioGroup style={styles.containerItem} label="Horizontal Button Radio Group:" direction="row"
                        mode="button">
                <Radio label="Option One" value={1}/>
                <Radio label="Option Two" value={2}/>
            </RadioGroup>

            {variants.map((variant, index) => (
                <Toggle style={styles.containerItem} label={`Checkbox Toggle (${variant})`} mode="checkbox" variant={variant} key={index} value={true}/>
            ))}

            {variants.map((variant, index) => (
                <Toggle style={styles.containerItem} label={`Switch Toggle (${variant})`} mode="switch" variant={variant} key={index} value={true}/>
            ))}
        </ScrollView>
    );
}
