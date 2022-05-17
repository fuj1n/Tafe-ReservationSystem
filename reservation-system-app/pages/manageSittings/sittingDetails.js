import { useRef, useContext, useCallback, useState } from "react";
import { ScrollView, Text, View } from "react-native";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import styles from "../styles";
import { Button, StyledText } from "../../components";
import login, { LoginContext } from "../../services/login"
import moment from "moment";

export default function SittingDetails(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { navigation, route } = props;

    const { sitting }= route.params;
    const [operation, setOperation] = useState(route.params.operation);
    const [isClosed, setIsClosed] = useState(sitting.isClosed);

    const { loginInfo } = useContext(LoginContext);

    async function close() {
        const response = await login.apiFetch(`admin/sitting/close?id=${sitting.id}`, "PUT", null, loginInfo.jwt)
            .catch(() => { });
        if (response.ok) {
            setIsClosed(true);
            setOperation("closed")
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            {operation && <StyledText variant="success">Sitting {operation} successfully</StyledText>}
            <Text>Type: {sitting.sittingType.description}</Text>
            <Text>Start Time: {moment(sitting.startTime).format("hh:mm A")}</Text>
            <Text>End Time: {moment(sitting.endTime).format("hh:mm A")}</Text>
            <Text>Capacity: {sitting.capacity}</Text>
            <Text>Is Closed: {isClosed.toString()}</Text>
            <View style={{ flexDirection: 'row' }}>
                <Button style={{ flex: 1 }} variant="primary"
                    onPress={() => navigation.navigate("EditSitting", { sitting })}>Edit</Button>
                <Button style={{ flex: 1 }} variant="danger" onPress={close}>Close</Button>
            </View>
        </ScrollView>
    );
}
