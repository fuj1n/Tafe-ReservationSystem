import { useRef, useContext, useCallback, useState } from "react";
import { ScrollView, Text, View } from "react-native";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import styles from "../styles";
import { Button, StyledText } from "../../components";
import login, { LoginContext } from "../../services/api/login"
import moment from "moment";
import api from "../../services/api"

export default function SittingDetails(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { navigation, route } = props;

    const [sittingTypes, setSittingTypes] = useState({});

    const { sitting, operation } = route.params;
    const [isClosed, setIsClosed] = useState(sitting.isClosed);

    const { loginInfo } = useContext(LoginContext);

    useFocusEffect(
        useCallback(() => {
            async function getSittingTypes() {
                const response = await api.sittings.getSittingTypes();
                if(response.error) {
                    setError(response);
                } else {
                    setSittingTypes(response);
                }
            }

            getSittingTypes();
        }, [])
    );

    async function close() {
        const response = await login.apiFetch(`admin/sitting/close?id=${sitting.id}`, "PUT", null, loginInfo.jwt)
            .catch(() => { });
        if (response.ok) {
            setIsClosed(true);
            navigation.navigate("SittingDetails", { sitting: sitting, operation: "closed" });
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            {operation && <StyledText variant="success">Sitting {operation} successfully</StyledText>}
            <Text>Type: {sittingTypes[sitting.sittingTypeId]}</Text>
            <Text>Start Time: {moment(sitting.startTime).format("hh:mm A")}</Text>
            <Text>End Time: {moment(sitting.endTime).format("hh:mm A")}</Text>
            <Text>Default Duration: {moment.duration(sitting.defaultDuration).humanize()}</Text>
            <Text>Capacity: {sitting.capacity}</Text>
            <Text>Status: {isClosed ? "Closed" : "Open"}</Text>
            <View style={{ flexDirection: 'row' }}>
                <Button style={{ flex: 1 }} variant="primary"
                    onPress={() => navigation.navigate("EditSitting", { sitting })}>Edit</Button>
                <Button style={{ flex: 1 }} variant="danger" onPress={close}>Close</Button>
            </View>
        </ScrollView>
    );
}
