import {useCallback, useRef, useState} from "react";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import api from "../../services/api";
import {Text, View} from "react-native";
import {Badge, Button, ErrorDisplay, Loader, StyledText} from "../../components";
import moment from "moment";

export default function Details({navigation, route}) {
    const {reservation} = route.params;

    const ref = useRef(null);
    useScrollToTop(ref);

    const [statuses, setStatuses] = useState({});
    const [origin, setOrigin] = useState("Loading");
    const [visuals, setVisuals] = useState({});

    useFocusEffect(useCallback(() => {
        async function getOrigin() {
            const response = await api.reservations.getOriginById(reservation.reservationOriginId);

            if (response.error) {
                return "Cannot determine origin";
            } else {
                return response;
            }
        }

        async function getStatuses() {
            const response = await api.reservations.getStatuses();

            if (response.error) {
                return {};
            } else {
                return response;
            }
        }

        async function getVisuals() {
            const response = await api.reservations.getStatusBadgeVisuals();

            if (response.error) {
                return {};
            } else {
                setVisuals(response);
                return response;
            }
        }

        getOrigin().then(o => setOrigin(o));
        getStatuses().then(s => setStatuses(s));
        getVisuals().then(v => setVisuals(v));
    }, []));

    const title = {
        fontWeight: "bold"
    };

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <View style={[styles.row, {alignSelf: 'stretch', justifyContent: "flex-start", marginBottom: 5}]}>
                <Button variant="primary" onPress={() => navigation.goBack()}>Back to List</Button>
            </View>
            <Text style={[title, styles.containerItem]}>Reservation Time</Text>
            <Text style={styles.containerItem}>{moment(reservation.startTime).format("hh:mm A")}</Text>

            <Text style={[title, styles.containerItem]}>Reservation Duration</Text>
            <Text style={styles.containerItem}>{moment.duration(reservation.duration).humanize()}</Text>

            <Text style={[title, styles.containerItem]}>Covers</Text>
            <Text style={styles.containerItem}>{reservation.numberOfGuests}</Text>

            <Text style={[title, styles.containerItem]}>Reservation Status</Text>
            <View style={{flexDirection: 'row', alignItems: 'center'}}>
                <Badge variant={visuals[reservation.reservationStatusId]?.reactBadgeVariant} style={[styles.containerItem, {marginRight: 5}]}>
                    <Text style={{color: '#FFF'}}>{statuses[reservation.reservationStatusId] ?? "Unknown"}</Text>
                </Badge>
            </View>

            <Text style={[title, styles.containerItem]}>Reservation Origin</Text>
            <Text style={styles.containerItem}>{origin}</Text>

            <Text style={[title, styles.containerItem]}>Customer</Text>
            <Text style={styles.containerItem}>{reservation.customer.firstName} {reservation.customer.lastName}</Text>
            {reservation.customer.phoneNumber &&
                <Text style={styles.containerItem}>Ph: {reservation.customer.phoneNumber}</Text>}
            {reservation.customer.email &&
                <Text style={styles.containerItem}>Email: {reservation.customer.email}</Text>}

            <Text style={[title, styles.containerItem]}>Notes</Text>
            {reservation.notes ?
                <Text style={styles.containerItem}>{reservation.notes}</Text> :
                <StyledText variant="danger" style={styles.containerItem}>No notes</StyledText>}
        </ScrollView>
    );
}