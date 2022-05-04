import {useRef, useState} from "react";
import {ScrollView} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import {Button, Dropdown, StyledText, TextInput} from "../components";
import styles from "./styles";

export default function TestPalette() {
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
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <StyledText style={styles.containerItem} variant="primary">Open up App.js to start working on your
                app!</StyledText>
            <TextInput label="This is a text box:" style={styles.containerItem}
                       placeholder="Enter text here..."/>

            <Dropdown label="This is a dropdown:" items={dropdownOneItems} selectedValue={dropdownValue}
                      onValueChange={setDropdownValue}/>
            <Dropdown style={styles.containerItem} label="This is a dropdown 2:" items={dropdownTwoItems}
                      selectedValue={dropdownTwoValue} onValueChange={setDropdownTwoValue}/>

            {variants.map((variant, index) => (
                <Button key={index} style={styles.containerItem} variant={variant}>{variant}</Button>))}
            {variants.map((variant, index) => (
                <StyledText key={index} style={styles.containerItem} variant={variant}>{variant}</StyledText>))}
        </ScrollView>
    );
}
