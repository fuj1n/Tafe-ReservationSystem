import { useRef, useState, useEffect, useContext } from "react";
import { ScrollView, Text, View } from "react-native";
import { useScrollToTop } from "@react-navigation/native";
import styles from "../styles";
import { DatePicker, TextInput, Dropdown, Button } from "../../components";
import login, { LoginContext } from "../../services";

export default function EditSitting(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { navigation, route } = props;
    const { sitting } = route.params;
    const [startTime, setStartTime] = useState(sitting.startTime);
    const [endTime, setEndTime] = useState(sitting.endTime);
    const [capacity, setCapacity] = useState(sitting.capacity);
    const [sittingType, setSittingType] = useState(sitting.sittingTypeId);

    const [sittingTypes, setSittingTypes] = useState([]);

    const { loginInfo } = useContext(LoginContext);

    useEffect(async () => {
        const response = await login.apiFetch("admin/sitting/sittingTypes", "GET", null, loginInfo.jwt)
            .catch(() => { });
        //console.log(response);
        if (response.ok) {
            setSittingTypes(await response.json());
        }
    }, []);

    const sittingTypesDropdown = [
        { label: "-- Please Select --", value: 0 },
        ...sittingTypes.map(st => ({ label: st.description, value: st.id }))
    ];

    function submit(){

        navigation.goBack();
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <DatePicker label="Start Time: " style={styles.containerItem} value={startTime} setValue={setStartTime} />
            <DatePicker label="End Time: " style={styles.containerItem} value={endTime} setValue={setEndTime} />
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