import {useRef, useState, useEffect, useContext} from "react";
import {Text, ScrollView} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import { Button, TimeSlotPicker } from "../../components";
import login, { LoginContext } from "../../services";


export default function CreateReservation(props) {
    const ref = useRef(null);
    useScrollToTop(ref);

    const [details, setDetails] = useState({});
    const {sitting} = props.route.params;
    console.log(props);

    const [startTime, setStartTime] = useState(new Date());

    const {loginInfo} = useContext(LoginContext); // pull variable loginInfo out of LoginContext
    useEffect(async () => {
        const response = await login.apiFetch(`reservation/details?sittingId=${sitting.id}`, 'GET', null, loginInfo.jwt);  //useEffect runs everytime the page re-renders

        if(response.ok) {  //if response status is "okay"
            const data = await response.json();
            setDetails(data);
            setStartTime(data.timeSlots[0]); //default start time to value of the first timeslot
        }
        
    }, []);

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Text>Peter is the best teacher ever!</Text> 
            <TimeSlotPicker timeSlots={details?.timeSlots} value={startTime} setValue={setStartTime}/> 
        
        </ScrollView>
    );
}