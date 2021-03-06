import { useRef, useState, useContext, useCallback } from "react";
import { ScrollView, Text, View } from "react-native";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import styles from "../styles";
import api from "../../services/api";
import { DatePicker, TextInput, Dropdown, Button, StyledText, ErrorDisplay } from "../../components";
import moment from "moment";

export default function CreateSitting(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { loginInfo } = useContext(api.login.LoginContext);

    const { navigation } = props;
    const [startTime, setStartTime] = useState(moment().startOf('hour').toISOString(true));
    const [endTime, setEndTime] = useState(moment().startOf('hour').toISOString(true));
    const [defaultDuration, setDefaultDuration] = useState("00:30:00");
    const [capacity, setCapacity] = useState(0);
    const [sittingType, setSittingType] = useState(0);
    const [sittingTypes, setSittingTypes] = useState([]);
    const [error, setError] = useState(null);

    useFocusEffect(
        useCallback(() => {
            async function getTypes() {
                const response = await api.common.fetch("admin/sitting/sittingTypes", "GET", null, loginInfo.jwt)
                    .catch(() => { });
                //console.log(response);
                if (response.ok) {
                    setSittingTypes(await response.json());
                }
            }

            getTypes();
        }, [])
    );

    const sittingTypesDropdown = [
        { label: "-- Please Select --", value: 0 },
        ...sittingTypes.map(st => ({ label: st.description, value: st.id }))
    ];

    async function submit() {
        const body = {
            "startTime": startTime,
            "endTime": endTime,
            "defaultDuration": defaultDuration,
            "capacity": capacity,
            "sittingTypeId": sittingType,
        };
        setError(null);
        const response = await api.common.fetch("admin/sitting/create", "POST", body, loginInfo.jwt);
        if (!response.ok) {
            setError(await api.common.processError(response));
        }
        else {
            navigation.pop();
            navigation.navigate("SittingDetails", { sitting: await response.json(), operation: "created" });
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <ErrorDisplay error={error} />
            <DatePicker label="Start Time: " style={styles.containerItem} value={startTime} onChange={setStartTime} />
            <DatePicker label="End Time: " style={styles.containerItem} value={endTime} onChange={setEndTime} />
            <TextInput label="Default Duration: " value={defaultDuration} onChangeText={setDefaultDuration} />
            <TextInput label="Capacity: " value={capacity} onChangeText={setCapacity} keyboardType="numeric" />
            <Dropdown style={styles.containerItem} label="Sitting Type:" items={sittingTypesDropdown}
                selectedValue={sittingType} onValueChange={setSittingType} />
            <View style={{ flexDirection: "row" }}>
                <Button variant="success" onPress={submit}>Confirm</Button>
                <Button variant="primary" onPress={() => navigation.goBack()}>Back</Button>
            </View>
        </ScrollView>
    );
}
