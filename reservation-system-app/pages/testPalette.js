import {createNativeStackNavigator} from "@react-navigation/native-stack";
import FirstScreen from "./testPalette/FirstScreen";
import SecondScreen from "./testPalette/SecondScreen";

export default function TestPalette() {
    const Stack = createNativeStackNavigator();

    return (
        <Stack.Navigator>
            <Stack.Screen name="FirstScreen" options={{headerShown: false}} component={FirstScreen}/>
            <Stack.Screen name="SecondScreen" options={{title: "Second Screen"}} component={SecondScreen}/>
        </Stack.Navigator>
    );
}
