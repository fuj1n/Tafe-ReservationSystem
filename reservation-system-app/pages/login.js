/**
 * This file acts as a template for creating pages, and acts as minimum required code to have a working page with
 * scrollable content.
 */
import {useContext, useRef, useState} from "react";
import {ScrollView} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "./styles";
import {Button, StyledText, TextInput} from "../components";
import login, {LoginInfo} from "../services";

export default function Login() {
    const {loginInfo, setLoginInfo} = useContext(LoginInfo);

    const ref = useRef(null);
    useScrollToTop(ref);

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    async function doLogin() {
        const info = await login.login(username, password);
        if((await info).isLoggedIn) {
            setLoginInfo(info);
        } else {
            setError("Incorrect username/password");
        }
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <TextInput label="Username:" value={username} onChangeText={setUsername}/>
            <TextInput label="Password:" value={password} onChangeText={setPassword}/>
            <Button variant="success">Submit</Button>
            <StyledText variant="danger">{error}</StyledText>
        </ScrollView>
    );
}
