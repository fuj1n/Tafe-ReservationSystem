import {StatusBar} from 'expo-status-bar';
import {StyleSheet, View, ActivityIndicator, ScrollView} from 'react-native';
import login, {LoginContext, LoginInfo} from './services';
import {useEffect, useState} from "react";
import {Button, TextInput, Dropdown, StyledText} from "./components";

export default function App() {
    const [loginInfo, setLoginInfo] = useState(new LoginInfo());
    const [isLoading, setIsLoading] = useState(true);

    const [dropdownValue, setDropdownValue] = useState("");
    const dropdownItems = [
        {label: "Apple", value: "1"},
        {label: "Banana", value: "2"},
        {label: "Orange", value: "3"},
    ];

    const variants = [
        "no variant",
        "primary",
        "secondary",
        "success",
        "danger",
        "warning",
        "info",
        "light",
        "dark"
    ];

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
            <View style={styles.root}>
                <ScrollView contentContainerStyle={styles.container}>
                    <StyledText style={styles.containerItem} variant="primary">Open up App.js to start working on your
                        app!</StyledText>
                    <TextInput label="This is a text box:" style={styles.containerItem}
                               placeholder="Enter text here..."/>

                    <Dropdown label="This is a dropdown:" items={dropdownItems} selectedValue={dropdownValue}
                              onValueChange={setDropdownValue}/>
                    <Dropdown style={styles.containerItem} label="This is a dropdown 2:" items={dropdownItems}
                              selectedValue={dropdownValue} onValueChange={setDropdownValue}/>

                    {variants.map((variant, index) => (
                        <Button key={index} style={styles.containerItem} variant={variant}>{variant}</Button>))}
                    {variants.map((variant, index) => (
                        <StyledText key={index} style={styles.containerItem} variant={variant}>{variant}</StyledText>))}
                </ScrollView>
            </View>
            <StatusBar style="auto"/>
        </LoginContext.Provider>
    );
}

const styles = StyleSheet.create({
    root: {
        flex: 1,
        marginTop: 50
    },
    container: {
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
    containerItem: {
        marginBottom: 5
    }
});
