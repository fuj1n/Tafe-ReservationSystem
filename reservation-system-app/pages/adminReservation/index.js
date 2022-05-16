import {createNativeStackNavigator} from "@react-navigation/native-stack";
import Sittings from "./sittings";
import Reservations from "./reservations";

export default function AdminReservationPage() {
    const Stack = createNativeStackNavigator();

    return (
        <Stack.Navigator>
            <Stack.Screen name="Sittings" options={{headerShown: false}} component={Sittings}/>
            <Stack.Screen name="Reservations" options={{title: "Reservations"}} component={Reservations}/>
        </Stack.Navigator>
    );
}
