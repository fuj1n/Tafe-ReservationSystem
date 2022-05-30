import {useCallback, useContext, useRef, useState} from "react";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import api from "../../services/api";
import {LoginContext} from "../../services";
import {ErrorDisplay, Loader, ReservationPicker} from "../../components";

export default function List({navigation}) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const {loginInfo} = useContext(LoginContext);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [reservations, setReservations] = useState([]);

    useFocusEffect(useCallback(() => {
        async function getReservations() {
            setLoading(true);

            const response = await api.reservations.getReservationsAsMember(loginInfo.jwt);

            if (response.error) {
                setError(response);
            } else {
                setReservations(response);
            }

            setLoading(false);
        }

        // noinspection JSIgnoredPromiseFromCall
        getReservations();
    }, [loginInfo]));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Loader loading={loading}>
                <ErrorDisplay error={error}>
                    <ReservationPicker reservations={reservations} onSelected={reservation => navigation.navigate("Details", {reservation})}/>
                </ErrorDisplay>
            </Loader>
        </ScrollView>
    );
}