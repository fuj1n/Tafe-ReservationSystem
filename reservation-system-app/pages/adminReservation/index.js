import {createNativeStackNavigator} from "@react-navigation/native-stack";
import Sittings from "./sittings";
import Reservations from "./reservations";
import Details from "./details";
import Create from "./create";
import Edit from "./edit";

export default function AdminReservationPage() {
    const Stack = createNativeStackNavigator();

    return (
        <Stack.Navigator>
            <Stack.Screen name="Sittings" options={{headerShown: false}} component={Sittings}/>
            <Stack.Screen name="Reservations" options={{title: "Reservations"}} component={Reservations}/>
            <Stack.Screen name="Details" options={{title: "Details"}} component={Details}/>
            <Stack.Screen name="Create" options={{title: "Create"}} component={Create}/>
            <Stack.Screen name="Edit" options={{title: "Edit"}} component={Edit}/>
        </Stack.Navigator>
    );
}
