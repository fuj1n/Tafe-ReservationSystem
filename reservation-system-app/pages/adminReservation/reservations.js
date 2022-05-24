import {useCallback, useContext, useRef, useState} from "react";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import {Loader, ReservationPicker, StyledText} from "../../components";
import {LoginContext} from "../../services";
import api from "../../services/api";
import ErrorDisplay from "../../components/errorDisplay";

export default function Reservations(props) {
    const {route} = props;
    const sitting = route.params;

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
    }, []));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Loader loading={loading}>
                <ErrorDisplay error={error}>
                    <StyledText style={{fontWeight: '700'}}>
                        <ReservationPicker reservations={reservations}/>
                    </StyledText>
                </ErrorDisplay>
            </Loader>
        </ScrollView>
    );
}
