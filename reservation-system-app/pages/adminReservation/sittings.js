import {useCallback, useContext, useRef, useState} from "react";
import {View} from "react-native";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import {Loader, SittingPicker, Toggle} from "../../components";
import {LoginContext} from "../../services";
import api from "../../services/api";
import ErrorDisplay from "../../components/errorDisplay";

export default function Sittings(props) {
    const {navigation} = props;

    const ref = useRef(null);
    useScrollToTop(ref);

    const {loginInfo} = useContext(LoginContext);

    const [sittings, setSittings] = useState([]);
    const [sittingTypes, setSittingTypes] = useState({});

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    const [showPast, setShowPast] = useState(false);
    const [showClosed, setShowClosed] = useState(true);

    useFocusEffect(useCallback(() => {
        async function getSittingTypes() {
            const response = await api.sittings.getSittingTypes();
            if (response.error) {
                setError(response);
            } else {
                setSittingTypes(response);
            }
        }

        async function getSittings() {
            setLoading(true);

            const response = await api.sittings.getSittingsAsAdmin(loginInfo.jwt, showPast, showClosed);

            if (response.error) {
                setError(response);
            } else {
                setSittings(response);
            }

            setLoading(false);
        }

        setError(null);
        // noinspection JSIgnoredPromiseFromCall
        getSittingTypes();
        // noinspection JSIgnoredPromiseFromCall
        getSittings();
    }, [showPast, showClosed]));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Loader loading={loading}>
                <ErrorDisplay error={error}>
                    <View style={[styles.row, {alignSelf: 'stretch', justifyContent: "flex-end"}]}>
                        <Toggle mode="switch" label="Show past sittings" value={showPast} onChange={setShowPast}
                                style={{paddingRight: 6}}/>
                        <Toggle mode="switch" label="Show closed sittings" value={showClosed} onChange={setShowClosed}/>
                    </View>
                    <SittingPicker sittings={sittings} onSelected={s => navigation.navigate("Reservations", s)}
                                   sittingTypeSelector={s => sittingTypes[s.sittingTypeId]}/>
                </ErrorDisplay>
            </Loader>
        </ScrollView>
    );
}
