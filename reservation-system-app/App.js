import 'react-native-gesture-handler';

import {StatusBar} from 'expo-status-bar';
import {StyleSheet, View, ActivityIndicator, Image, Text, Alert, Platform} from 'react-native';
import {useContext, useEffect, useState} from "react";
import {createDrawerNavigator, DrawerContentScrollView, DrawerItem, DrawerItemList} from "@react-navigation/drawer";
import {
    LoginPage,
    TestPalette,
    ReservationPage,
    SittingsPage,
    AdminReservationPage,
    MemberReservationPage, HomePage
} from "./pages";
import {getFocusedRouteNameFromRoute, NavigationContainer, DefaultTheme} from "@react-navigation/native";
import appStyles from './pages/styles';

import moment from "moment";
import 'moment/min/locales';
import * as Localization from "expo-localization";
import api from "./services/api";

import brand from "./assets/brand.png";
import {Button} from "./components";
import {variants} from "./components/style";

const navTheme = {
    ...DefaultTheme,
    colors: {
        ...DefaultTheme.colors,
        background: 'transparent',
        primary: '#6a1a21',
        card: '#d7c6b4',
        border: '#b58d90'
    }
};

function configureLocale() {
    // See https://github.com/moment/moment/issues/4349 for reason
    // Moment has the wrong first day of week for en-au
    moment.updateLocale('en-au', {
        week: {
            dow: 1,
            doy: 4
        }
    });
}

function LoginPane({navigation, state}) {
    const {loginInfo, setLoginInfo} = useContext(api.login.LoginContext);

    async function logout() {
        if (Platform.OS === 'web') {
            if (confirm('Are you sure you would like to log out?')) {
                api.login.logout().then(setLoginInfo);
            }

            return;
        }

        Alert.alert('Log out?', 'Are you sure you would like to log out?',
            [
                {text: 'Cancel', style: 'cancel'},
                {text: 'Log Out', style: 'destructive', onPress: () => api.login.logout().then(setLoginInfo)}
            ], {cancelable: true});
    }

    if (!loginInfo.isLoggedIn) {
        return (
            <DrawerItem label="Log In" focused={state.routes[state.index].name === 'Login'}
                        onPress={() => navigation.navigate("Login")}/>
        );
    }

    let userIdentity;
    if (loginInfo.user.person) {
        const person = loginInfo.user.person;
        userIdentity = `${person.firstName} ${person.lastName}`;
    } else {
        const roles = [...loginInfo.user.roles];
        if ((roles.includes("Admin") || roles.includes("Manager")) && roles.includes("Employee")) {
            roles.splice(roles.indexOf("Employee"), 1);
        }

        userIdentity = roles[0];
    }

    return (
        <View style={[styles.drawerItem, appStyles.row, {alignItems: 'center', justifyContent: 'space-between'}]}>
            <Text style={appStyles.containerItem}>Hello {userIdentity}!</Text>
            <Button onPress={logout} variant="danger">Log Out</Button>
        </View>
    );
}

// TODO: highly inefficient
function DrawerContent(props) {
    const restaurant = useContext(api.restaurant.RestaurantContext);

    function hideDescriptors(condition) {
        return {
            ...props,
            descriptors: Object.entries(props.descriptors)
                .map(([key, descriptor]) => {
                    if (condition(descriptor)) return [key, {
                        ...descriptor,
                        options: {...descriptor.options, drawerItemStyle: {height: 0, opacity: 0, position: 'absolute'}}
                    }];
                    return [key, descriptor];
                })
                .reduce((acc, [key, descriptor]) => {
                    acc[key] = descriptor;
                    return acc;
                }, {})
        };
    }

    const propsWithHiddenPages = hideDescriptors(descriptor => descriptor.options.hidden || descriptor.options.groupName);

    const groups = Object.values(props.descriptors)
        .map(descriptor => descriptor.options.groupName)
        .filter((value, index, self) => value && self.indexOf(value) === index)
        .map(name => ({
            name,
            props: hideDescriptors(descriptor => descriptor.options.groupName !== name || descriptor.options.hidden)
        }));

    return (
        <>
            <DrawerContentScrollView {...props} style={{flex: 1}}>
                <View style={[styles.drawerItem, {marginVertical: 12, flexDirection: 'row', alignItems: 'center'}]}>
                    <Image source={brand} style={{width: 32, height: 32, marginRight: 10}}/>
                    <Text style={{fontWeight: '700'}}>{restaurant.name}</Text>
                </View>
                <DrawerItemList {...propsWithHiddenPages} />
                {groups.map(({name, props}) => (
                    <View key={name}>
                        <DrawerItem label={name + ":"} onPress={() => {
                        }}/>
                        <View style={{
                            marginLeft: 20,
                            paddingLeft: -10,
                            borderLeftWidth: 1,
                            borderColor: variants.Primary.color,
                            borderBottomLeftRadius: 5
                        }}>
                            <DrawerItemList {...props} />
                        </View>
                    </View>
                ))}
            </DrawerContentScrollView>
            <LoginPane {...props} style={{justifySelf: 'bottom'}}/>
        </>
    );
}

export default function App() {
    const [loginInfo, setLoginInfo] = useState(new api.login.LoginInfo());
    const [restaurant, setRestaurant] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const Drawer = createDrawerNavigator();
    const RestaurantContext = api.restaurant.RestaurantContext;

    useEffect(async () => {
        configureLocale();
        moment.locale(Localization.locales); // Configure moment to use the device locale

        const restaurant = await api.restaurant.getRestaurant();

        if (restaurant.error) {
            setRestaurant({name: "Could not fetch restaurant information"});
        } else {
            setRestaurant(restaurant);
        }

        const loginInfo = await api.login.getLogin();

        if (loginInfo.isLoggedIn) {
            setLoginInfo(loginInfo);
        }

        setIsLoading(false);
    }, []);

    if (isLoading) {
        return (
            <View style={[styles.root, styles.loadingContainer]}>
                <ActivityIndicator size="large" color="#6a1a21"/>
            </View>
        );
    }

    // Disables the root navigator's header if the child navigator is not on the first page
    function showHeader({route}) {
        const child = route[Object.getOwnPropertySymbols(route)[0]];
        const routeName = getFocusedRouteNameFromRoute(route);

        if (!child || !routeName || child.routeNames[0] === routeName) {
            return {};
        }

        return {headerShown: false};
    }

    const LoginContext = api.login.LoginContext;

    return (
        <NavigationContainer theme={navTheme}>
            <LoginContext.Provider value={{loginInfo, setLoginInfo}}>
                <RestaurantContext.Provider value={restaurant}>
                    <View style={styles.root}>
                        <Drawer.Navigator initialRouteName="Home" screenOptions={showHeader}
                                          drawerContent={DrawerContent}>
                            <Drawer.Screen name={'Home'} component={HomePage}/>
                            <Drawer.Screen name="Reservation" options={{title: "Reservation"}}
                                           component={ReservationPage}/>
                            {loginInfo.user?.roles.includes("Member") &&
                                <Drawer.Group>
                                    <Drawer.Screen name="MemberReservation"
                                                   options={{title: "My Reservations"}}
                                                   component={MemberReservationPage}/>
                                </Drawer.Group>
                            }
                            {loginInfo.user?.roles.includes("Employee") &&
                                <Drawer.Group screenOptions={{groupName: 'Admin'}}>
                                    <Drawer.Screen name="AdminReservation" options={{title: "Reservation"}}
                                                   component={AdminReservationPage}/>
                                    <Drawer.Screen name="Sittings" options={{title: "Sittings"}}
                                                   component={SittingsPage}/>
                                    <Drawer.Screen name="TestPalette" options={{title: "Test Palette"}}
                                                   component={TestPalette}/>
                                </Drawer.Group>
                            }
                            <Drawer.Screen name="Login" options={{title: "Login", hidden: true}} component={LoginPage}/>
                        </Drawer.Navigator>
                    </View>
                </RestaurantContext.Provider>
            </LoginContext.Provider>
            <StatusBar style="auto"/>
        </NavigationContainer>
    );
}

const styles = StyleSheet.create({
    root: {
        flex: 1,
        backgroundColor: '#f4ebdc'
    },
    loadingContainer: {
        alignItems: 'center',
        justifyContent: 'center'
    },
    drawerItem: {
        marginHorizontal: 10,
        marginVertical: 4,
        overflow: 'hidden',
    },
});
