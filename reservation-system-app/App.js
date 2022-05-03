import {StatusBar} from 'expo-status-bar';
import {StyleSheet, Text, View} from 'react-native';
import login, {LoginContext, LoginInfo} from './services';
import {useEffect, useState} from "react";
import {ActivityIndicator} from "react-native";
import Button from "./components/button";

export default function App() {
    const [loginInfo, setLoginInfo] = useState(new LoginInfo());
    const [isLoading, setIsLoading] = useState(true);

    useEffect(async () => {
        const loginInfo = await login.getLogin();

        if (loginInfo.isLoggedIn) {
            setLoginInfo(loginInfo);
        }

        setIsLoading(false);
    }, []);

    if (isLoading) {
        return (
            <View style={styles.container}>
                <ActivityIndicator size="large" color="#0000ff"/>
            </View>
        );
    }

    return (
        <LoginContext.Provider value={{loginInfo, setLoginInfo}}>
            <View style={styles.container}>
                <Text>Open up App.js to start working on your app!</Text>
                <StatusBar style="auto"/>
                <Button variant="primary">Primary</Button>
                <Button variant="secondary">Secondary</Button>
                <Button variant="success">Success</Button>
                <Button variant="danger">Danger</Button>
                <Button variant="warning">Warning</Button>
                <Button variant="info">Info</Button>
                <Button variant="dark">Dark</Button>
                <Button variant="light">Light</Button>
            </View>
        </LoginContext.Provider>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
