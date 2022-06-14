import { useRef, useState, useEffect, useContext } from "react";
import { ScrollView, StyleSheet } from "react-native";
import { useScrollToTop } from "@react-navigation/native";
import styles from "../styles";
import {Button, TimeSlotPicker, Dropdown, StyledText, TextInput, Loader} from "../../components";
import api from "../../services/api";
import moment from "moment";


export default function CreateReservation(props) {
    const ref = useRef(null);
    useScrollToTop(ref);
    const {navigation, route} = props;
    const { sitting } = route.params;

    const [details, setDetails] = useState({});
    console.log(props);
    const [startTime, setStartTime] = useState(moment());
    const [isLoading, setIsLoading] = useState(true);
    const [noOfGuests, setNoOfGuests] = useState(1);
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [phone, setPhone] = useState("");
    const [email, setEmail] = useState("");
    const [notes, setNotes] = useState("");
    const [error, setError] = useState([]);

    const noOfGuestsValues = [];
    for (let i = 1; i <= 10; i++) {
        noOfGuestsValues.push({ label: i.toString(), value: i });
    }
    noOfGuestsValues.push({ label: "More than 10", value: 10000 });

    const { loginInfo } = useContext(api.login.LoginContext); // pull variable loginInfo out of LoginContext

    async function onSubmit() {
        const body = {
            sittingId: sitting.id,
            firstName, lastName, email, notes,
            startTime: startTime.toISOString(true),
            noOfPeople: noOfGuests,
            phoneNumber: phone,
            sittingStartTime: moment().toISOString(),
            sittingEndTime: moment().toISOString(),
            duration: "00:00:00",
            sittingType: ""
        };

        setError([]);

        const response = await api.common.fetch("reservation/create", "POST", body, loginInfo.jwt);
        if(response.ok) {
            navigation.navigate("ConfirmReservation",{returnedBody: await response.json()}); //navigates to CreateReservation page for the sitting that was clicked
        } else {
            if(response.status === 400) {
                const data = await response.json();
                setError(Object.values(data.errors).reduce((total, current) => [...total, ...current], []));
            }
        }

    }

    useEffect(async () => {
        const response = await api.common.fetch(`reservation/details?sittingId=${sitting.id}`, 'GET', null, loginInfo.jwt);  //useEffect runs everytime the page re-renders

        if (response.ok) {  //if response status is "okay"
            const data = await response.json();
            data.timeSlots = data.timeSlots.map(ts => moment(ts));
            setDetails(data); //setting details to data

            setStartTime(data.timeSlots[0]); //default start time to value of the first timeslot
            setIsLoading(false);
        }

        async function getUserInfo() {
            const response = await api.common.fetch("reservation/userinfo", 'GET', null, loginInfo.jwt);
            if(response.ok && response.status !== 204) {
                const userInfo = await response.json();

                setFirstName(userInfo.firstName);
                setLastName(userInfo.lastName);
                setEmail(userInfo.email);
                setPhone(userInfo.phoneNumber);
            }
        }

        await getUserInfo();
    }, []);

    if (isLoading) {
        return (
            <Loader loading={true}/>
        );
    }

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            {/* <Text>Please Select the Start Time</Text> */}
            <TimeSlotPicker label="Please select a time" timeSlots={details?.timeSlots} value={startTime} setValue={setStartTime} />

            {/* <Text>No. of Guests</Text> */}

            <Dropdown label="Number of Guests" items={noOfGuestsValues} selectedValue={noOfGuests}
                onValueChange={setNoOfGuests} />
            {noOfGuests > 10 &&
                <StyledText variant="danger">
                    Bookings of more than 10 must be made by contacting the restaurant
                </StyledText>}

            <TextInput label="First Name" style={styles.containerItem} placeholder="Your First Name" value={firstName} onChangeText={setFirstName}/>
            <TextInput label="Last Name" style={styles.containerItem} placeholder="Your Last Name" value={lastName} onChangeText={setLastName}/>
            <TextInput label="Email" style={styles.containerItem} placeholder="Your Email" value={email} onChangeText={setEmail}/>
            <TextInput label="Phone" style={styles.containerItem} placeholder="Your Phone Number" value={phone} onChangeText={setPhone}/>
            <TextInput label="Notes" style={styles.containerItem} placeholder="Any additional requests" value={notes} onChangeText={setNotes}/>

            <Button disabled={noOfGuestsValues>10} variant="primary" onPress={onSubmit}>Submit</Button>
            {error.map((e, index) => (
                <StyledText key={index} variant="danger">{e}</StyledText>
            ))}
        </ScrollView>
    );

}

const tempstyles = StyleSheet.create({
    root: {
        flex: 1,
        backgroundColor: '#fff'
    },
    loadingContainer: {
        alignItems: 'center',
        justifyContent: 'center'
    },
});
