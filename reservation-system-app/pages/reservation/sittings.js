import {useRef, useContext, useState, useEffect} from "react";
import {ScrollView, View, Text} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import { Button } from "../../components";
import login, { LoginContext } from "../../services";


function Row (props) {
    const {sitting} = props;
    
    const startTime = new Date(sitting.startTime);
    const endTime = new Date (sitting.endTime);

    return (
        <Button style={styles.containerItem} variant="success">
            <Text>
                {sitting.sittingType} from {startTime.toLocaleTimeString([],{timeStyle:"short"})} to {endTime.toLocaleTimeString([],{timeStyle:"short"})}
            </Text>
        </Button>
    );
}

export default function SittingsPage(props) {
    const {navigation} = props;
    const ref = useRef(null);
    useScrollToTop(ref);

    const {loginInfo} = useContext(LoginContext); // pull variable logInInfo out of LogInCOntext

    const [sittings, setSittings] = useState([]); //empty array is the initial value???

    useEffect(async () => {
        const response = await login.apiFetch('reservation/sittings', 'GET', null, loginInfo.jwt);  //useEffect runs everytime the page re-renders, so 

        if(response.status === 200) {  //if response status is "okay"
            setSittings(await response.json());
        }
    }, []); //empty dependency array causes useEffect to only run the function after the first initial render

    function onButtonPressed() {
        navigation.navigate("CreateReservation");
    }

    const sittingsByDate = sittings.reduce ((acc,s) => {
        let date = new Date (s.startTime);
        date = new Date (date.getFullYear(),date.getMonth(),date.getDate());
        

        const ticks = date.getTime();
        
        if(!acc[ticks]){
            acc[ticks] = [];
        }
        

        acc[ticks].push(s);
        return acc;
    }, {});
    console.log (sittingsByDate);

    function format (ticks){
        const date = new Date(parseInt(ticks));
        return date.toLocaleDateString();
    };

    return (
        <ScrollView contentContainerStyle={styles.container} ref={ref}>
            <Text>This is the sittings page</Text>

            {Object.entries(sittingsByDate).map(([ticks,sittingsList])=>(
                <View key={parseInt(ticks)}>

                    <Text>{format(ticks)}</Text>
                    {sittingsList.map((s)=>(
                        <Row key={s.id} sitting={s}/>
                    ))}

                </View>
            ))}          
            
            <Button variant="success" onPress={onButtonPressed}>Go to second page</Button>
        </ScrollView>
    );
}
