import {useRef, useContext, useState, useEffect} from "react";
import {ScrollView, View, Text, Image} from "react-native";
import {useScrollToTop} from "@react-navigation/native";
import styles from "../styles";
import { Button, SittingPicker } from "../../components";
import login, { LoginContext } from "../../services";
import brand from "../../assets/brand.png";



function Row (props) {
    const {sitting, navigation} = props; 
    //sitting = props.sitting
    //object deconstruction create new var called sitting, assign variable sitting to value of props.sitting
    
    const startTime = new Date(sitting.startTime);
    const endTime = new Date (sitting.endTime);

    function onButtonPressed() {
        navigation.navigate("CreateReservation", {sitting}); //navigates to CreateReservation page for the sitting that was clicked
    }

    return (
        <Button style={[styles.containerItem, {alignItems:"stretch"}]} variant="success" onPress={onButtonPressed}>
            
                {sitting.sittingType} from {startTime.toLocaleTimeString([],{timeStyle:"short"})} to {endTime.toLocaleTimeString([],{timeStyle:"short"})}
            
        </Button>
    );
}

export default function SittingsPage(props) {
    const {navigation} = props;
    const ref = useRef(null); //Gesture navigation scroll to top when we switch screen
    useScrollToTop(ref);

    const {loginInfo} = useContext(LoginContext); // pull variable loginInfo out of LoginContext

    const [sittings, setSittings] = useState([]); //empty array is the initial value for sittings

    useEffect(async () => {
        const response = await login.apiFetch('reservation/sittings', 'GET', null, loginInfo.jwt);  //useEffect runs everytime the page re-renders

        if(response.ok) {  //if response status is "okay"
            setSittings(await response.json());
        }
        
    }, []); //empty dependency array causes useEffect to only run the function after the first initial render

    function onButtonPressed(sitting) {
        navigation.navigate("CreateReservation", {sitting}); //navigates to CreateReservation page for the sitting that was clicked
    }

   return (
    <ScrollView contentContainerStyle={[]} ref={ref}>
        <View style={{flex: 1, justifyContent: "center", flexDirection: "row"}}>
            <Image source={brand} style={{width: 32, height: 32}}/>
        </View>
        <Text style={{textAlign:"center",fontWeight:'bold'}}>Please select a sitting</Text>
       <SittingPicker sittings={sittings} sittingTypeSelector={s=>s.sittingType} onSelected={onButtonPressed} 
       noSittingsMessage="No sittings found, please contact the restaurant for further information"/>

    </ScrollView>

   );
    

    // const sittingsByDate = sittings.reduce ((total,s) => { //total = an object containing all sittings, s = each sitting or ?INITIAL VALUE????
    //     let date = new Date (s.startTime);
    //     date = new Date (date.getFullYear(),date.getMonth(),date.getDate()); //reformat the date to ONLY Year Month Day
        

    //     const ticks = date.getTime(); //ticks = time from date to Jan 1 1970
        
    //     if(!total[ticks]){ //if total[ticks] does NOT exist
    //                         //accessing a variable inside the total object the name of which is value of ticks 
    //         total[ticks] = []; //each tick is a group of sitting, if it doesn't exist, create it
    //     }
        

    //     total[ticks].push(s); //adds each sitting to an array within the total object
    //     return total;
    // }, {});
    


    function format (ticks){
        const date = new Date(parseInt(ticks)); //values for keys always stored as strings, must convert to Int
        return date.toLocaleDateString();  // convert it to a formatted date based on locale
    };
    
    //     console.log (Object.entries(sittingsByDate)); //array of key value pair where the ticks is the key and the value is the sittings array
    
    // return (
    //     <ScrollView contentContainerStyle={styles.container} ref={ref}>
          
    //         {Object.entries(sittingsByDate).map(([ticks,sittingsList])=>(  //separates objects into an array of entries
    //             <View style={{alignItems:"stretch"}} key={parseInt(ticks)}>

    //                 <Text style={[styles.containerItem,{textAlign:"center", fontWeight: "bold"}]}>{format(ticks)}</Text> 
    //                 {sittingsList.map((s)=>( 
    //                     <Row key={s.id} sitting={s} navigation={navigation}/> //{fromat{ticks} = returns the formatted date of ticks*/}, mapping sitting list to an array of row components
    //                 ))}

    //             </View>
    //         ))}                     

    //     </ScrollView>
    // );
}
