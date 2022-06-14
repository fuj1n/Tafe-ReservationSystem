import {useContext, useRef} from "react";
import {useScrollToTop} from "@react-navigation/native";
import styles from "./styles";
import {ScrollView} from "react-native-gesture-handler";
import api from "../services/api";
import {Text} from "react-native";

export default function HomePage() {
    const ref = useRef(null);
    useScrollToTop(ref);

    const restaurant = useContext(api.restaurant.RestaurantContext);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Text style={[{fontSize: 20, fontWeight: "bold"}, styles.containerItem]}>
                {restaurant.name}
            </Text>
            <Text style={[{fontSize: 20}, styles.containerItem]}>
                {restaurant.address}
            </Text>
            <Text style={[{fontSize: 20}, styles.containerItem]}>
                {restaurant.phoneNumber}
            </Text>
            <Text style={[{fontSize: 20}, styles.containerItem]}>
                {restaurant.email}
            </Text>
        </ScrollView>
    );
}
