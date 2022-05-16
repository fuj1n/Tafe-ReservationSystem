import {useCallback, useContext, useRef, useState} from "react";
import {View} from "react-native";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import {Loader, StyledText} from "../../components";
import login, {LoginContext} from "../../services/login";

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
            const response = await login.apiFetch(`admin/reservation/statuses`, "GET", null, loginInfo.jwt);

            if(response.ok) {
                const statuses = await response.json();
                // Turn origins into an object with id as key
                setStatuses(statuses.reduce((acc, status) => {
                    acc[status.id] = status.description;
                    return acc;
                }, {}));
            } else {
                if (response.internalError) {
                    setError(response.statusText);
                } else {
                    const errorObject = await response.json();
                    setError(errorObject.errorMessage ?? `${response.status} ${response.statusText}`);
                }
            }
        }

        async function getReservations() {
            setLoading(true);
            setError(null);

            const response = await login.apiFetch(`admin/reservation/list/${sitting.id}`, "GET", null, loginInfo.jwt);

            if (response.ok) {
                setReservations(await response.json());
            } else {
                if (response.internalError) {
                    setError(response.statusText);
                } else {
                    const errorObject = await response.json();
                    setError(errorObject.errorMessage ?? `${response.status} ${response.statusText}`);
                }
            }

            setLoading(false);
        }

        // noinspection JSIgnoredPromiseFromCall
        getStatuses();
        // noinspection JSIgnoredPromiseFromCall
        getReservations();
    }, []));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Loader loading={loading}>
                {error ? <StyledText variant="danger" style={styles.error}>{error}</StyledText> :
                    <View>
                        {reservations.length.toString()}
                    </View>}
            </Loader>
        </ScrollView>
    );
}
