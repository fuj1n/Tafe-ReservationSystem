import { useState, useRef, useContext, useCallback } from "react";
import { useScrollToTop, useFocusEffect } from "@react-navigation/native";
import { ScrollView, View, Text } from "react-native";
import { Button } from "../../components";
import styles from "../styles";
import login, { LoginContext } from "../../services/login"
import moment from "moment";

function Sitting(props) {
    const { sitting, navigation, setChanged } = props;
    const { loginInfo, setLoginInfo } = useContext(LoginContext);

    async function close() {
        const response = await login.apiFetch(`admin/sitting/close?id=${sitting.id}`, "PUT", null, loginInfo.jwt)
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
    const { loginInfo, setLoginInfo } = useContext(LoginContext);

    const [sittings, setSittings] = useState([]);
    const [changed, setChanged] = useState(false);

    useFocusEffect(
        useCallback(() => {
            async function get() {
                const response = await login.apiFetch("admin/sitting/sittings", "GET", null, loginInfo.jwt)
                    .catch(() => { });
                if (response.ok) {
                    const data = await response.json();
                    data.map(st => {
                        st.startTime = moment(st.startTime);
                        st.endTime = moment(st.endTime);
                        return st;
                    })

                    setSittings(data);
                }
            }

            if (changed) {
                setChanged(false);
            } else {
                get();
            }
        }, [changed])
    );

    const ref = useRef(null);
    useScrollToTop(ref);
    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
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

            <View>
                {sittings.map((s, index) => (
                    <Sitting key={index} sitting={s} navigation={navigation} setChanged={setChanged} />
                ))}
            </View>
        </ScrollView>
    )
}
