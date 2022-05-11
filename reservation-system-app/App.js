import 'react-native-gesture-handler';

import {StatusBar} from 'expo-status-bar';
import {StyleSheet, View, ActivityIndicator} from 'react-native';
import {useEffect, useState} from "react";
import {createDrawerNavigator} from "@react-navigation/drawer";
import login, {LoginContext, LoginInfo} from './services';
import {LoginPage, TestPalette, ReservationPage, SittingsPage} from "./pages";
import {getFocusedRouteNameFromRoute, NavigationContainer, DefaultTheme} from "@react-navigation/native";
import moment from "moment";
import * as Localization from "expo-localization";

const navTheme = {
    ...DefaultTheme,
    colors: {
        ...DefaultTheme.colors,
        background: 'transparent'
    }
};

export default function App() {
    const [loginInfo, setLoginInfo] = useState(new LoginInfo());
    const [isLoading, setIsLoading] = useState(true);
    const Drawer = createDrawerNavigator();

    moment.locale(Localization.locale);

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

    // Disables the root navigator's header if the child navigator is not on the first page
    function showHeader({route}) {
        const child = route[Object.getOwnPropertySymbols(route)[0]];
        const routeName = getFocusedRouteNameFromRoute(route);

        if(!child || !routeName || child.routeNames[0] === routeName) {
            return {};
        }

        return {headerShown: false};
    }

    return (
        <NavigationContainer theme={navTheme}>
            <LoginContext.Provider value={{loginInfo, setLoginInfo}}>
                <View style={styles.root}>
                    <Drawer.Navigator initialRouteName="Home" screenOptions={showHeader}>
                        <Drawer.Screen name="TestPalette" options={{title: "Test Palette"}} component={TestPalette}/>
                        <Drawer.Screen name="Reservation" options={{title: "Sittings"}} component={ReservationPage}/>
                        <Drawer.Screen name="Sittings" options={{title: "Sittings"}} component={SittingsPage}/>
                        <Drawer.Screen name="Login" options={{title: "Login"}} component={LoginPage}/>
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
