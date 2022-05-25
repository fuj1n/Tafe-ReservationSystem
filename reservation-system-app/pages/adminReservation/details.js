import {useCallback, useContext, useRef, useState} from "react";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {GestureHandlerRootView, ScrollView} from "react-native-gesture-handler";
import {Modal, Text, View} from "react-native";
import moment from "moment";
import api from "../../services/api";
import {LoginContext} from "../../services";
import {Badge, Button, Dropdown, DropdownItem, ErrorDisplay, StyledText} from "../../components";
import componentStyle from "../../components/style";

export default function Details(props) {
    const {navigation, route} = props;
    const {reservation, sitting, sittingType, action} = route.params;

    const {loginInfo} = useContext(LoginContext);

    const [origin, setOrigin] = useState("Loading...");

    const [statusId, setStatusId] = useState(reservation.reservationStatusId);
    const [pendingStatusId, setPendingStatusId] = useState(reservation.reservationStatusId);

    const [statuses, setStatuses] = useState({});

    const [visuals, setVisuals] = useState({});

    const [error, setError] = useState(null);

    const [showUpdateStatusModal, setShowUpdateStatusModal] = useState(false);

    useFocusEffect(useCallback(() => {
        async function getOrigin() {
            const response = await api.reservations.getOriginById(loginInfo.jwt, reservation.reservationOriginId);

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
            const response = await api.reservations.getStatusBadgeVisuals(loginInfo.jwt);

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
    }, [loginInfo]));

    const ref = useRef(null);
    useScrollToTop(ref);

    const title = {
        fontWeight: "bold"
    };

    async function updateStatus() {
        const response = await api.reservations.setStatus(loginInfo.jwt, reservation.id, pendingStatusId);
        setShowUpdateStatusModal(false);

        setError(null);

        if(response.error) {
            setError(response);
        } else {
            setStatusId(pendingStatusId);
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            {action && <StyledText variant="success">Successfully {action} a reservation</StyledText>}
            <View style={[styles.row, {alignSelf: 'stretch', justifyContent: "flex-start", marginBottom: 5}]}>
                <Button variant="info" style={{marginRight: 5}}>Edit Reservation</Button>
                <Button variant="primary" onPress={() => navigation.goBack()}>Back to List</Button>
            </View>
            <ErrorDisplay error={error}/>
            <Text style={[title, styles.containerItem]}>Sitting</Text>
            <Text style={styles.containerItem}>
                {sittingType} on {moment(sitting.startTime).format("DD/MM/YYYY")}{' '}
                from {moment(sitting.startTime).format("HH:mm")} to {moment(sitting.endTime).format("HH:mm")}
            </Text>

            <Text style={[title, styles.containerItem]}>Reservation Time</Text>
            <Text style={styles.containerItem}>{moment(reservation.startTime).format("hh:mm AA")}</Text>

            <Text style={[title, styles.containerItem]}>Covers</Text>
            <Text style={styles.containerItem}>{reservation.numberOfGuests}</Text>

            <Text style={[title, styles.containerItem]}>Reservation Status</Text>
            <View style={{flexDirection: 'row', alignItems: 'center'}}>
                <Badge variant={visuals[statusId]?.reactBadgeVariant} style={[styles.containerItem, {marginRight: 5}]}>
                    <Text style={{color: '#FFF'}}>{statuses[statusId] ?? "Unknown"}</Text>
                </Badge>
                <Button variant="info" onPress={() => setShowUpdateStatusModal(true)}>Update</Button>
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

            <Modal visible={showUpdateStatusModal} transparent={true} onRequestClose={() => setShowUpdateStatusModal(false)}
                   animationType="slide" statusBarTranslucent={true}>
                <GestureHandlerRootView style={componentStyle.modalHost}>
                    <View style={componentStyle.modalView}>
                        <Text>Please select new status</Text>

                        <Dropdown label="New Status:" selectedValue={pendingStatusId} onValueChange={setPendingStatusId}>
                            {Object.entries(statuses).map(([id, label], index) => (
                                <DropdownItem key={index} value={id} label={label}/>
                            ))}
                        </Dropdown>

                        <Button onPress={updateStatus} variant="success"
                                style={{alignSelf: "stretch"}}>Save</Button>
                    </View>
                </GestureHandlerRootView>
            </Modal>
        </ScrollView>
    );
}
