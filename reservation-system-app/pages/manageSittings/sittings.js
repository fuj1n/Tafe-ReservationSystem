import {createNativeStackNavigator} from "@react-navigation/native-stack";
import SittingsList from "./sittingsList";
import CreateSitting from "./createSitting";
import EditSitting from "./editSitting";
import SittingDetailsPage from "./sittingDetails";

export default function SittingsPage() {
    const Stack = createNativeStackNavigator();

    return (
        <Stack.Navigator>
            <Stack.Screen name="SittingsList" options={{headerShown: false}} component={SittingsList}/>
            <Stack.Screen name="CreateSitting" options={{title: "Create Sitting"}} component={CreateSitting}/>
            <Stack.Screen name="EditSitting" options={{title: "Edit Sitting"}} component={EditSitting}/>
            <Stack.Screen name="SittingDetails" options={{title: "Sitting Details"}} component={SittingDetailsPage}/>
        </Stack.Navigator>
    );
}
