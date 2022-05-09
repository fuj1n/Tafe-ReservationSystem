import {useScrollToTop} from "@react-navigation/native";
import {ScrollView} from "react-native";
import styles from "../styles";
import {Button, DatePicker, Dropdown, StyledText, TextInput, TimeSlotPicker} from "../../components";
import {useRef, useState} from "react";

export default function FirstScreen(props) {
    const {navigation} = props;

    const [dropdownValue, setDropdownValue] = useState("");
    const [dropdownTwoValue, setDropdownTwoValue] = useState("");

    const dropdownOneItems = [
        {label: "-- Please Select --", value: "0"},
        {label: "Apple", value: "1"},
        {label: "Banana", value: "2"},
        {label: "Orange", value: "3"},
    ];
    const dropdownTwoItems = [
        {label: "-- Please Select --", value: "0"},
        {label: "Arthur", value: "1"},
        {label: "Matthew", value: "2"},
        {label: "Wencong", value: "3"},
    ];

    const timeSlots = [
        new Date(2022, 5, 4, 9),
        new Date(2022, 5, 4, 9, 30),
        new Date(2022, 5, 4, 10),
        new Date(2022, 5, 4, 10, 30),
        new Date(2022, 5, 4, 11),
        new Date(2022, 5, 4, 11, 30),
        new Date(2022, 5, 4, 12),
    ];
    const [timeSlot, setTimeSlot] = useState(timeSlots[0]);

    const [date, setDate] = useState(new Date(2022, 5, 4, 9));

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
        <ScrollView style={styles.scrollView} contentContainerStyle={styles.container} ref={ref}>
            <StyledText style={styles.containerItem} variant="primary">Open up App.js to start working on your
                app!</StyledText>
            <Button variant="primary" onPress={() => navigation.navigate("SecondScreen")}>Go to second screen</Button>
            <TextInput label="This is a text box:" style={styles.containerItem}
                       placeholder="Enter text here..."/>

            <Dropdown label="This is a dropdown:" items={dropdownOneItems} selectedValue={dropdownValue}
                      onValueChange={setDropdownValue}/>
            <Dropdown style={styles.containerItem} label="This is a dropdown #2:" items={dropdownTwoItems}
                      selectedValue={dropdownTwoValue} onValueChange={setDropdownTwoValue}/>

            <TimeSlotPicker label="This is a time slot picker:" style={styles.containerItem} value={timeSlot}
                            setValue={setTimeSlot} timeSlots={timeSlots}/>

            <DatePicker label="This is a date picker:" style={styles.containerItem} value={date} setValue={setDate}/>

            {variants.map((variant, index) => (
                <Button key={index} style={styles.containerItem} variant={variant}>{variant}</Button>))}
            {variants.map((variant, index) => (
                <StyledText key={index} style={styles.containerItem} variant={variant}>{variant}</StyledText>))}
        </ScrollView>
    );
}
