import {useCallback, useContext, useRef, useState} from "react";
import {useFocusEffect, useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import {ScrollView} from "react-native-gesture-handler";
import api from "../../services/api";
import {
    Button,
    Dropdown,
    DropdownItem,
    ErrorDisplay,
    HorizontalRule,
    Loader,
    TextInput,
    TimeSlotPicker
} from "../../components";
import moment from "moment";
import {View} from "react-native";

export default function Edit({route, navigation}) {
    const {reservation, sitting, sittingType} = route.params;

    const {loginInfo} = useContext(api.login.LoginContext);

    // Form data
    const [startTime, setStartTime] = useState(moment(moment(reservation.startTime)));
    const [duration, setDuration] = useState(reservation.duration);
    const [origin, setOrigin] = useState(reservation.reservationOriginId);
    const [numGuests, setNumGuests] = useState(reservation.numberOfGuests);

    const [firstName, setFirstName] = useState(reservation.customer.firstName);
    const [lastName, setLastName] = useState(reservation.customer.lastName);
    const [email, setEmail] = useState(reservation.customer.email);
    const [phone, setPhone] = useState(reservation.customer.phoneNumber);

    const [notes, setNotes] = useState(reservation.notes);
    // End of form data

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [blockingError, setBlockingError] = useState(null);

    const [origins, setOrigins] = useState({});
    const [timeSlots, setTimeSlots] = useState([]);

    useFocusEffect(useCallback(() => {
        async function getOrigins() {
            const origins = await api.reservations.getOrigins();

            if(origins.error) {
                setBlockingError(origins);
            } else {
                setOrigins(origins);
            }
        }

        async function getTimeSlots() {
            const timeSlots = await api.sittings.getTimeSlots(sitting.id);

            if(timeSlots.error) {
                setBlockingError(timeSlots);
            } else {
                setTimeSlots(timeSlots.map(ts => moment(ts)));
                setLoading(false); // Time slots earlier in form, and so we need them sooner
            }
        }

        setLoading(true);
        setBlockingError(null);
        getOrigins();
        getTimeSlots();
    }, []));

    async function submit() {
        setLoading(true);
        setError(null);

        const newReservation = {
            id: reservation.id,
            sittingId: sitting.id,
            startTime: startTime.toISOString(true),
            duration: duration,
            reservationOriginId: origin,
            numberOfGuests: numGuests,
            customer: {
                firstName,
                lastName,
                email,
                phoneNumber: phone,
            },
            notes
        };

        const response = await api.reservations.editReservationAsAdmin(loginInfo.jwt, newReservation);

        if(response.error) {
            setError(response);
            setLoading(false);
        } else {
            navigation.pop(); // ensure back button goes to reservation list
            navigation.navigate("Details", {reservation: response, sitting, sittingType, action: "edited"});
        }
    }

    const ref = useRef(null);
    useScrollToTop(ref);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <ErrorDisplay error={blockingError}>
                <Loader loading={loading}>
                    <ErrorDisplay error={error}/>

                    <TimeSlotPicker label="Start Time" timeSlots={timeSlots} value={startTime} setValue={setStartTime} style={styles.containerItem} />
                    <TextInput label="Duration" value={duration} onChangeText={setDuration} style={styles.containerItem} />
                    <Dropdown label="Origin" value={origin} onValueChange={setOrigin} style={styles.containerItem} >
                        <DropdownItem value="0" label="-- Please Select --" />
                        {Object.entries(origins).map(([id, label]) => <DropdownItem key={id} value={id} label={label} />)}
                    </Dropdown>
                    <TextInput label="Number of Guests" value={numGuests} onChangeText={setNumGuests} style={styles.containerItem} />
                    <HorizontalRule/>
                    <TextInput label="First Name" value={firstName} onChangeText={setFirstName} style={styles.containerItem} />
                    <TextInput label="Last Name" value={lastName} onChangeText={setLastName} style={styles.containerItem} />
                    <TextInput label="Email" value={email} onChangeText={setEmail} style={styles.containerItem} />
                    <TextInput label="Phone" value={phone} onChangeText={setPhone} style={styles.containerItem} />
                    <HorizontalRule/>
                    <TextInput label="Notes" value={notes} onChangeText={setNotes} multiline={true} style={styles.containerItem} />

                    <View style={[styles.containerItem, styles.row, {alignSelf: 'stretch', justifyContent: "flex-start"}]}>
                        <Button variant="success" style={{marginRight: 5}} onPress={submit}>Edit</Button>
                        <Button variant="primary" onPress={() => navigation.goBack()}>Back to Sittings</Button>
                    </View>
                </Loader>
            </ErrorDisplay>
        </ScrollView>
    );
}
