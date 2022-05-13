import {useRef} from "react";
import {ScrollView, Text} from "react-native";
import {createNativeStackNavigator} from "@react-navigation/native-stack";
import {useScrollToTop} from "@react-navigation/native";
import styles from "./styles";
import SittingsPage  from "./reservation/sittings";
import SecondScreen from "./testPalette/SecondScreen";
import CreateReservation from "./reservation/createReservation";
import ConfirmReservation from "./reservation/confirmReservation";

export default function ReservationPage() {
    const Stack = createNativeStackNavigator();

    const ref = useRef(null);
    useScrollToTop(ref);

    return (
        <Stack.Navigator>
            <Stack.Screen name="ReservationSittings" options={{headerShown: false}} component={SittingsPage}/>
            <Stack.Screen name="CreateReservation" options={{title: "Create Reservation"}} component={CreateReservation}/>
            <Stack.Screen name="ConfirmReservation" options={{title: "Confirmation"}} component={ConfirmReservation}/>
        </Stack.Navigator>
    );
}