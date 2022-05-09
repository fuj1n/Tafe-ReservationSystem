import {useRef} from "react";
import {Text, ScrollView} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "../styles";


export default function CreateReservation() {
    const ref = useRef(null);
    useScrollToTop(ref);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Text>Hello Sathi, you are the best teacher ever!</Text> 
        </ScrollView>
    );
}