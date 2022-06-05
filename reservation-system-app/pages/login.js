import {useContext, useRef, useState} from "react";
import {ScrollView, Text} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "./styles";
import {Button, StyledText, TextInput} from "../components";
import api from "../services/api";

function LoggedIn() {
    const {loginInfo, setLoginInfo} = useContext(api.login.LoginContext);

    return(
        <>
            <Text style={styles.containerItem}>Logged in as {loginInfo.user.username}</Text>
            <Button variant="danger" onPress={async () => setLoginInfo(await api.login.logout())} style={styles.containerItem}>Logout</Button>
        </>
    )
}

export default function Login() {
    const {loginInfo, setLoginInfo} = useContext(api.login.LoginContext);

    const ref = useRef(null);
    useScrollToTop(ref);

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    async function doLogin() {
        setError("");

        const info = await api.login.login(username, password);
        console.log(info);
        if(info.isLoggedIn) {
            setLoginInfo(info);
        } else {
            setError(info.error ?? "Unknown error");
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            {loginInfo.isLoggedIn ? <LoggedIn/> :
                <>
                    <TextInput label="Username:" value={username} onChangeText={setUsername} style={styles.containerItem}/>
                    <TextInput label="Password:" value={password} onChangeText={setPassword} secureTextEntry={true}
                               style={styles.containerItem}/>
                    <Button variant="success" onPress={doLogin} style={styles.containerItem}>Submit</Button>
                    <StyledText variant="danger" style={styles.containerItem}>{error}</StyledText>
                </>
            }
        </ScrollView>
    );
}
