import { useRef, useState, useEffect, useContext } from "react";
import { Text, ScrollView, View, ActivityIndicator, StyleSheet } from "react-native";
import { useScrollToTop } from "@react-navigation/native";
import styles from "../styles";
import { Button, TimeSlotPicker, Dropdown, StyledText, TextInput } from "../../components";
import login, { LoginContext } from "../../services";
import moment from "moment";


export default function ConfirmReservation(props) {
    const {route} = props;
    const {returnedBody} = route.params;
    const ref = useRef(null);
    useScrollToTop(ref);

    console.log(route.params);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Text style={styles.containerItem}>Your reservation has been confirmed</Text>
            <Text style={styles.containerItem}>Sitting Type: {returnedBody.sittingType}</Text>
            <Text style={styles.containerItem}>Start Time: {moment(returnedBody.startTime).format("DD/MM/YYYY hh:mm A")}</Text>
            <Text style={styles.containerItem}>Duration: {returnedBody.duration}</Text>
            <Text style={styles.containerItem}>Number of Guests: {returnedBody.noOfPeople}</Text>
            <Text style={styles.containerItem}>Phone: {returnedBody.phone}</Text>
            <Text style={styles.containerItem}>Email: {returnedBody.email}</Text>
            <Text style={styles.containerItem}>Notes: {returnedBody.notes}</Text>
            
            <Text>Thank you very much for your booking</Text>

       
            
            
        </ScrollView>
    );
}