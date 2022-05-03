import {StatusBar} from 'expo-status-bar';
import {StyleSheet, Text, View} from 'react-native';
import login, {LoginContext, LoginInfo} from './services';
import {useEffect, useState} from "react";
import {ActivityIndicator} from "react-native";
import Button from "./components/button";
import TextInput from "./components/textInput";
import Dropdown from "./components/dropdown";

export default function App() {
    const [loginInfo, setLoginInfo] = useState(new LoginInfo());
    const [isLoading, setIsLoading] = useState(true);

    const [dropdownValue, setDropdownValue] = useState("");
    const dropdownItems = [
        {label: "Apple", value: "1"},
        {label: "Banana", value: "2"},
        {label: "Orange", value: "3"},
    ]

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
                <Text style={styles.containerItem}>Open up App.js to start working on your app!</Text>
                <TextInput label="This is a text box:" style={styles.containerItem} placeholder="Enter text here..."/>

                <Dropdown label="This is a dropdown:" items={dropdownItems} selectedValue={dropdownValue} onValueChange={setDropdownValue}/>
                <Dropdown label="This is a dropdown 2:" items={dropdownItems} selectedValue={dropdownValue} onValueChange={setDropdownValue}/>

                <Button style={styles.containerItem} variant="primary">Primary</Button>
                <Button style={styles.containerItem} variant="secondary">Secondary</Button>
                <Button style={styles.containerItem} variant="success">Success</Button>
                <Button style={styles.containerItem} variant="danger">Danger</Button>
                <Button style={styles.containerItem} variant="warning">Warning</Button>
                <Button style={styles.containerItem} variant="info">Info</Button>
                <Button style={styles.containerItem} variant="dark">Dark</Button>
                <Button style={styles.containerItem} variant="light">Light</Button>
            </View>
            <StatusBar style="auto"/>
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
    containerItem: {
        marginBottom: 5
    }
});
