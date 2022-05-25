import {useCallback, useContext, useRef, useState} from "react";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import {Button, Loader, ReservationPicker, StyledText} from "../../components";
import {LoginContext} from "../../services";
import api from "../../services/api";
import ErrorDisplay from "../../components/errorDisplay";
import {View} from "react-native";

export default function Reservations(props) {
    const {route, navigation} = props;
    const {sitting, sittingType} = route.params;

    const ref = useRef(null);
    useScrollToTop(ref);

    const {loginInfo} = useContext(LoginContext);

    const [statuses, setStatuses] = useState({});

    const [reservations, setReservations] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useFocusEffect(useCallback(() => {
        async function getStatuses() {
            const response = await api.reservations.getStatuses();

            if(response.error) {
                setError(response)
            } else {
                setStatuses(response);
            }
        }

        async function getReservations() {
            setLoading(true);

            const response = await api.reservations.getReservationsAsAdmin(loginInfo.jwt, sitting.id);

            if (response.error) {
                setError(response);
            } else {
                setReservations(response);
            }

            setLoading(false);
        }

        setError(null);
        // noinspection JSIgnoredPromiseFromCall
        getStatuses();
        // noinspection JSIgnoredPromiseFromCall
        getReservations();
    }, [loginInfo]));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Loader loading={loading}>
                <ErrorDisplay error={error}>
                    <View style={[styles.row, {alignSelf: 'stretch', justifyContent: "flex-start", marginBottom: 5}]}>
                        <Button variant="success" style={{marginRight: 5}} onPress={() => navigation.navigate("Create", {sitting, sittingType})}>Create Reservation</Button>
                        <Button variant="primary" onPress={() => navigation.goBack()}>Back to Sittings</Button>
                    </View>
                    <ReservationPicker reservations={reservations} onSelected={reservation => navigation.navigate("Details", {reservation, sitting, sittingType, status: statuses[reservation.reservationStatusId]})}/>
                </ErrorDisplay>
            </Loader>
        </ScrollView>
    );
}
