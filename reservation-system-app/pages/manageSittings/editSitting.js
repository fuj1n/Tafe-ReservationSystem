import { useRef, useState, useContext, useCallback } from "react";
import { ScrollView, View } from "react-native";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import styles from "../styles";
import { DatePicker, TextInput, Dropdown, Button, StyledText, ErrorDisplay } from "../../components";
import login, { LoginContext } from "../../services";
import api from "../../services/api";

export default function EditSitting(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { navigation, route } = props;
    const { sitting } = route.params;
    const [startTime, setStartTime] = useState(sitting.startTime);
    const [endTime, setEndTime] = useState(sitting.endTime);
    const [capacity, setCapacity] = useState(sitting.capacity);
    const [sittingType, setSittingType] = useState(sitting.sittingTypeId);

    const [error, setError] = useState(null);

    const [sittingTypes, setSittingTypes] = useState([]);

    const { loginInfo } = useContext(LoginContext);

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
        }, [sitting.id])
    );

    const sittingTypesDropdown = [
        { label: "-- Please Select --", value: 0 },
        ...sittingTypes.map(st => ({ label: st.description, value: st.id }))
    ];

    async function submit() {
        const body = {
            "id": sitting.id,
            "startTime": startTime,
            "endTime": endTime,
            "capacity": capacity,
            "sittingTypeId": sittingType,
        };
        setError(null);
        const response = await login.apiFetch("admin/sitting/edit", "PUT", body, loginInfo.jwt);
        if (!response.ok) {
            setError(await api.common.processError(response));
        }
        else {
            navigation.navigate("SittingDetails", { sitting: await response.json(), operation: "edited" });
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <ErrorDisplay error={error} />  
            <DatePicker label="Start Time: " style={styles.containerItem} value={startTime} onChange={setStartTime} />
            <DatePicker label="End Time: " style={styles.containerItem} value={endTime} onChange={setEndTime} />
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