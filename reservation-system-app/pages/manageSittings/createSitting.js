import { useRef, useState } from "react";
import { ScrollView, Text, View } from "react-native";
import { useScrollToTop } from "@react-navigation/native";
import styles from "../styles";
import { DatePicker, TextInput, Dropdown, Button } from "../../components";

export default function CreateSitting(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const { navigation } = props;
    const [startTime, setStartTime] = useState(new Date());
    const [endTime, setEndTime] = useState(new Date());
    const [capacity, setCapacity] = useState(0);
    const [sittingType, setSittingType] = useState(0);

    const sittingTypes = [
        { label: "-- Please Select --", value: 0 },
        { label: "Breakfast", value: 1 },
        { label: "Lunch", value: 2 },
        { label: "Dinner", value: 3 },
    ];

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <DatePicker label="Start Time: " style={styles.containerItem} value={startTime} setValue={setStartTime} />
            <DatePicker label="End Time: " style={styles.containerItem} value={endTime} setValue={setEndTime} />
            <TextInput label="Capacity: " value={capacity} onChangeText={setCapacity} keyboardType="numeric" />
            <Dropdown style={styles.containerItem} label="Sitting Type:" items={sittingTypes}
                selectedValue={sittingType} onValueChange={setSittingType} />
            <View style={{ flexDirection: "row" }}>
                <Button variant="success" >Confirm</Button>
                <Button variant="primary" onPress={() => navigation.goBack()}>Back</Button>
            </View>
        </ScrollView>
    );
}