import { useRef, useState, useContext, useCallback } from "react";
import { ScrollView, Text, View } from "react-native";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import styles from "../styles";
import login, { LoginContext } from "../../services";
import { DatePicker, TextInput, Dropdown, Button, StyledText } from "../../components";
import moment from "moment";

export default function CreateSitting(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { loginInfo } = useContext(LoginContext);

    const { navigation } = props;
    const [startTime, setStartTime] = useState(moment().startOf('hour').toISOString(true));
    const [endTime, setEndTime] = useState(moment().startOf('hour').toISOString(true));
    const [capacity, setCapacity] = useState(0);
    const [sittingType, setSittingType] = useState(0);
    const [sittingTypes, setSittingTypes] = useState([]);
    const [error, setError] = useState("");

    useFocusEffect(
        useCallback(() => {
            async function getTypes() {
                const response = await login.apiFetch("admin/sitting/sittingTypes", "GET", null, loginInfo.jwt)
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

    //console.log(sittingTypesDropdown);

    async function submit() {
        const body = {
            "startTime": startTime,
            "endTime": endTime,
            "capacity": capacity,
            "sittingTypeId": sittingType,
        };
        const response = await login.apiFetch("admin/sitting/create", "POST", body, loginInfo.jwt)
            .catch(() => { });
        setError(null);
        if (!response.ok) {
            if (response.status === 400) {
                setError(await response.json());
            }
            else {
                setError(`${response.status} - ${response.statusText}`);
            }
        }
        else {
            navigation.navigate("SittingDetails", { sitting: await response.json(), operation: "created" });
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <DatePicker label="Start Time: " style={styles.containerItem} value={startTime} onChange={setStartTime} />
            <DatePicker label="End Time: " style={styles.containerItem} value={endTime} onChange={setEndTime} />
            <TextInput label="Capacity: " value={capacity} onChangeText={setCapacity} keyboardType="numeric" />
            <Dropdown style={styles.containerItem} label="Sitting Type:" items={sittingTypesDropdown}
                selectedValue={sittingType} onValueChange={setSittingType} />
            <View style={{ flexDirection: "row" }}>
                <Button variant="success" onPress={submit}>Confirm</Button>
                <Button variant="primary" onPress={() => navigation.goBack()}>Back</Button>
            </View>
            <StyledText variant="danger">{error && JSON.stringify(error)}</StyledText>
        </ScrollView>
    );
}