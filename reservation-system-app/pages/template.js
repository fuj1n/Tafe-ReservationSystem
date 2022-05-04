/**
 * This file acts as a template for creating pages, and acts as minimum required code to have a working page with
 * scrollable content.
 */
import {useRef} from "react";
import {ScrollView} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "./styles";

export default function MyPageName() {
    const ref = useRef(null);
    useScrollToTop(ref);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            {/*Content goes here*/}
        </ScrollView>
    );
}
