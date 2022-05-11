/**
 * This file acts as a template for creating pages, and acts as minimum required code to have a working page with
 * scrollable content.
 */
import {useContext, useRef, useState} from "react";
import {ScrollView, Text} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "./styles";
import {Button, StyledText, TextInput} from "../components";
import login, {LoginContext} from "../services";

function LoggedIn() {
    const {loginInfo, setLoginInfo} = useContext(LoginContext);

    return(
        <>
            <Text style={styles.containerItem}>Logged in as {loginInfo.username}</Text>
            <Button variant="danger" onPress={async () => setLoginInfo(await login.logout())} style={styles.containerItem}>Logout</Button>
        </>
    )
}

export default function Login() {
    const {loginInfo, setLoginInfo} = useContext(LoginContext);

    const ref = useRef(null);
    useScrollToTop(ref);

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    async function doLogin() {
        setError("");

        const info = await login.login(username, password);
        if((await info).isLoggedIn) {
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
