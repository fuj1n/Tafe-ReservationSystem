import {useCallback, useContext, useRef, useState} from "react";
import {View} from "react-native";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import {Loader, SittingPicker, StyledText, Toggle} from "../../components";
import login, {LoginContext} from "../../services/login";

export default function Sittings(props) {
    const {navigation} = props;

    const ref = useRef(null);
    useScrollToTop(ref);

    const {loginInfo} = useContext(LoginContext);

    const [sittings, setSittings] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    const [showPast, setShowPast] = useState(false);
    const [showClosed, setShowClosed] = useState(true);

    useFocusEffect(useCallback(() => {
        async function getSittings() {
            setLoading(true);
            setError(null);

            const response = await login.apiFetch(`admin/reservation?includePast=${showPast}&includeClosed=${showClosed}`, "GET", null, loginInfo.jwt);

            if (response.ok) {
                setSittings(await response.json());
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
        getSittings();
    }, [showPast, showClosed]));

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Loader loading={loading}>
                {error ? <StyledText variant="danger" style={styles.error}>{error}</StyledText> :
                    <>
                        <View style={[styles.row, {alignSelf: 'stretch', justifyContent: "flex-end"}]}>
                            <Toggle mode="switch" label="Show past sittings" value={showPast} onChange={setShowPast} style={{paddingRight: 6}}/>
                            <Toggle mode="switch" label="Show closed sittings" value={showClosed} onChange={setShowClosed}/>
                        </View>
                        <SittingPicker sittings={sittings} onSelected={s => navigation.navigate("Reservations", s)}/>
                    </>}
            </Loader>
        </ScrollView>
    );
}
