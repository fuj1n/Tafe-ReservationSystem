import { useState, useRef, useContext, useEffect } from "react";
import { useScrollToTop } from "@react-navigation/native";
import { ScrollView, View, Text } from "react-native";
import { Button } from "../../components";
import styles from "../styles";
import login, { LoginContext } from "../../services/login"

function Sitting(props) {
    const { sitting, navigation } = props;

    return (
        <View style={{ flexDirection: "row", alignItems: "center" }}>
            <Text style={{ flex: 1 }}>{sitting.sittingType.description}</Text>
            <Text style={{ flex: 2 }}>{sitting.startTime}</Text>
            <Text style={{ flex: 2 }}>{sitting.endTime}</Text>
            <Text style={{ flex: 1 }}>{sitting.capacity}</Text>
            <Text style={{ flex: 1.5 }}>{sitting.isClosed}</Text>
            <Button style={{ flex: 1 }} variant="primary" onPress={() => navigation.navigate("EditSitting")}>Edit</Button>
            <Button style={{ flex: 1 }} variant="danger" >Close</Button>
        </View>
    );
}

export default function SittingsList(props) {
    const { navigation } = props;
    const { loginInfo, setLoginInfo } = useContext(LoginContext);

    const [sittings, setSittings] = useState([]);

    useEffect(async () => {
        const response = await login.apiFetch("admin/sitting/sittings", "GET", null, loginInfo.jwt)
        .catch(() => {});
        //console.log(response);
        if (response.ok) {
            setSittings(await response.json());
        }
    }, []);

    const ref = useRef(null);
    useScrollToTop(ref);
    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Button style={{}} variant="primary" onPress={() => navigation.navigate("CreateSitting")}>Create</Button>
            <View style={{ flexDirection: "column", flex: 1 }}>
                {/* Header of the data */}
                <View style={{ flexDirection: "row", alignItems: "center" }}>
                    <Text style={{ flex: 1 }}>Type</Text>
                    <Text style={{ flex: 2 }}>Start Time</Text>
                    <Text style={{ flex: 2 }}>End Time</Text>
                    <Text style={{ flex: 1 }}>Capacity</Text>
                    <Text style={{ flex: 1.5 }}>Is Closed</Text>
                    <Text style={{ flex: 1 }}></Text>
                    <Text style={{ flex: 1 }}></Text>
                </View>
                {sittings.map(s => (
                    <Sitting sitting={s} navigation={navigation} />
                ))}
            </View>
        </ScrollView>
    )
}