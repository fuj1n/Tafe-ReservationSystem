import { useState, useRef, useContext, useCallback } from "react";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import { ScrollView, View, Text } from "react-native";
import { Button, SittingPicker, Toggle, Loader } from "../../components";
import styles from "../styles";
import { LoginContext } from "../../services/api/login"
import api from "../../services/api"
import ErrorDisplay from "../../components/errorDisplay"

function Sitting(props) {
    const { sitting, navigation, setChanged } = props;
    const { loginInfo, setLoginInfo } = useContext(LoginContext);

    async function close() {
        const response = await api.common.fetch(`admin/sitting/close?id=${sitting.id}`, "PUT", null, loginInfo.jwt)
            .catch(() => { });
        if (response.ok) {
            setChanged(true);
        }
    }

    return (
        <Button style={{ flexDirection: "row", alignItems: "center" }} onPress={() => navigation.navigate("SittingDetails", { sitting })}>
            {sitting.sittingType.description} from {sitting.startTime.format("hh:mm A")} to {sitting.endTime.format("hh:mm A")} {sitting.isClosed && "[CLOSED]"}
        </Button>


        /*             <Text style={{ flex: 1 }}>{sitting.sittingType.description}</Text>
                    <Text style={{ flex: 2 }}>{sitting.startTime}</Text>
                    <Text style={{ flex: 2 }}>{sitting.endTime}</Text>
                    <Text style={{ flex: 1 }}>{sitting.capacity}</Text>
                    <Text style={{ flex: 1.5 }}>{sitting.isClosed.toString()}</Text>
 */

    );
}

export default function SittingsList(props) {
    const { navigation } = props;
    const { loginInfo } = useContext(LoginContext);

    const [sittingTypes, setSittingTypes] = useState({});
    const [sittings, setSittings] = useState([]);
    const [changed, setChanged] = useState(false);

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    const [showPast, setShowPast] = useState(false);

    useFocusEffect(
        useCallback(() => {
            async function getSittingTypes() {
                const response = await api.sittings.getSittingTypes();
                if (response.error) {
                    setError(response);
                } else {
                    setSittingTypes(response);
                }
            }

            async function get() {
                setLoading(true);

                const response = await api.sittings.getSittingsAsAdmin(loginInfo.jwt, showPast, true);
                if (response.error) {
                    setError(response);
                } else {
                    setSittings(response);
                }

                setLoading(false);
                // const response = await login.apiFetch(`admin/sitting/sittings?pastSittings=${showPast}`, "GET", null, loginInfo.jwt)
                //     .catch(() => { });
                // if (response.ok) {
                //     const data = await response.json();
                //     data.map(st => {
                //         st.startTime = moment(st.startTime);
                //         st.endTime = moment(st.endTime);
                //         return st;
                //     })

                //     setSittings(data);
                // } else {
                //     setSittings([]);
                // }
            }

            if (changed) {
                setChanged(false);
            } else {
                setError(null);
                setSittings([]);

                getSittingTypes();
                get();
            }

        }, [changed, showPast, loginInfo])
    );

    const ref = useRef(null);
    useScrollToTop(ref);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <ErrorDisplay error={error}>
                <Loader loading={loading}>
                    <Button style={{}} variant="primary" onPress={() => navigation.navigate("CreateSitting")}>Create</Button>
                    {/*             <View style={{ flexDirection: "column", flex: 1 }}>
                <View style={{ flexDirection: "row", alignItems: "center" }}>
                    <Text style={{ flex: 1 }}>Type</Text>
                    <Text style={{ flex: 2 }}>Start Time</Text>
                    <Text style={{ flex: 2 }}>End Time</Text>
                    <Text style={{ flex: 1 }}>Capacity</Text>
                    <Text style={{ flex: 1.5 }}>Is Closed</Text>
                    <Text style={{ flex: 1 }}></Text>
                    <Text style={{ flex: 1 }}></Text>
                </View>

            </View> */}

                    {/* {sittings.map((s, index) => (
                    <Sitting key={index} sitting={s} navigation={navigation} setChanged={setChanged} />
                ))} */}
                    <View style={[styles.row, { alignSelf: 'stretch', justifyContent: "flex-end" }]}>
                        <Toggle mode="switch" label="Show past sittings" value={showPast} onChange={setShowPast}
                            style={{ paddingRight: 6 }} />
                    </View>
                    <SittingPicker sittings={sittings} onSelected={sitting => navigation.navigate("SittingDetails", { sitting })}
                        sittingTypeSelector={s => sittingTypes[s.sittingTypeId]} />
                </Loader>
            </ErrorDisplay>
        </ScrollView>
    )
}
