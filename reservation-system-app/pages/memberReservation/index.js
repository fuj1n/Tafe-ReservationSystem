import {createNativeStackNavigator} from "@react-navigation/native-stack";
import List from "./list";
import Details from "./details";

export default function AdminReservationPage() {
    const Stack = createNativeStackNavigator();

    return (
        <Stack.Navigator>
            <Stack.Screen name="List" options={{headerShown: false}} component={List}/>
            <Stack.Screen name="Details" options={{title: "Reservation Details"}} component={Details}/>
        </Stack.Navigator>
    );
}
