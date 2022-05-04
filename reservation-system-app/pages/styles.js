import {Platform, StyleSheet} from "react-native";

const isWeb = Platform.OS === "web";

const styles = StyleSheet.create({
    scrollView: {
        flex: 1,
        backgroundColor: "#fff"
    },
    container: {
        flexGrow: 1,
        backgroundColor: '#fff',
        alignItems: isWeb ? 'center' : undefined,
        marginHorizontal: 12
    },
    containerItem: {
        marginBottom: 5
    }
});

export default styles;
