import {useCallback, useContext, useRef, useState} from "react";
import {View} from "react-native";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import {ErrorDisplay, Loader, SittingPicker, Toggle} from "../../components";
import api from "../../services/api";

export default function Sittings(props) {
    const {navigation} = props;

    const ref = useRef(null);
    useScrollToTop(ref);

    const {loginInfo} = useContext(api.login.LoginContext);

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
    }, [showPast, showClosed, loginInfo]));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <ErrorDisplay error={error}>
                <Loader loading={loading}>
                    <View style={[styles.row, {alignSelf: 'stretch', justifyContent: "flex-end"}]}>
                        <Toggle mode="switch" label="Show past sittings" value={showPast} onChange={setShowPast}
                                style={{paddingRight: 6}}/>
                        <Toggle mode="switch" label="Show closed sittings" value={showClosed} onChange={setShowClosed}/>
                    </View>
                    <SittingPicker sittings={sittings} onSelected={sitting => navigation.navigate("Reservations", {
                        sitting, sittingType: sittingTypes[sitting.sittingTypeId]
                    })} sittingTypeSelector={s => sittingTypes[s.sittingTypeId]}/>
                </Loader>
            </ErrorDisplay>
        </ScrollView>
    );
}
