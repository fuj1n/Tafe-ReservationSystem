import 'react-native-gesture-handler';

import {StatusBar} from 'expo-status-bar';
import {StyleSheet, View, ActivityIndicator} from 'react-native';
import {useEffect, useState} from "react";
import {createDrawerNavigator} from "@react-navigation/drawer";
import login, {LoginContext, LoginInfo} from './services';
import {TestPalette} from "./pages";
import {NavigationContainer} from "@react-navigation/native";

export default function App() {
    const [loginInfo, setLoginInfo] = useState(new LoginInfo());
    const [isLoading, setIsLoading] = useState(true);
    const Drawer = createDrawerNavigator();

    useEffect(async () => {
        const loginInfo = await login.getLogin();

        if (loginInfo.isLoggedIn) {
            setLoginInfo(loginInfo);
        }

        setIsLoading(false);
    }, []);

    if (isLoading) {
        return (
            <View style={[styles.root, styles.loadingContainer]}>
                <ActivityIndicator size="large" color="#0000ff"/>
            </View>
        );
    }

    return (
        <NavigationContainer>
            <LoginContext.Provider value={{loginInfo, setLoginInfo}}>
                <View style={styles.root}>
                    <Drawer.Navigator initialRouteName="Home">
                        <Drawer.Screen name="Test" component={TestPalette}/>
                    </Drawer.Navigator>
                </View>
            </LoginContext.Provider>
            <StatusBar style="auto"/>
        </NavigationContainer>
    );
}

const styles = StyleSheet.create({
    root: {
        flex: 1,
        backgroundColor: '#fff'
    },
    loadingContainer: {
        alignItems: 'center',
        justifyContent: 'center'
    },
});
