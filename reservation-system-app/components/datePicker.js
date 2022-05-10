import style from "./style";
import {Modal, Text, View} from "react-native";
import {GestureHandlerRootView, RectButton} from "react-native-gesture-handler";
import {useState} from "react";
import Button from "./button";
import moment from "moment";
import {misc} from "../utility";

function DatePickerContent(props) {
    const localeData = moment.localeData();
    const firstDayOfWeek = moment.locale() === "en-au" ? 1 : localeData.firstDayOfWeek(); // See https://github.com/moment/moment/issues/4349 for reason for hardcode
    const days = moment.weekdaysMin();
    const months = moment.monthsShort();

    function Row(props) {
        const {data} = props;

        return (
            <View style={{flexDirection: "row"}}>
                {misc.overflowMap(firstDayOfWeek, data, (day, index) => (
                    <Text key={index}>{day}</Text>
                ))}
            </View>
        );
    }

    return (
        <View style={{flexDirection: "column"}}>
            <Row data={days}/>
        </View>
    );
}

function TimePickerContent(props) {
    return (
        <View>

        </View>
    );
}

/**
 * @param props {{label: string, value: Moment, setValue: function(Moment), timePicker : boolean}}
 */
export default function DatePicker(props) {
    const {label} = props;
    const [show, setShow] = useState(false);
    const timePicker = props.timePicker ?? true;

    function onChange() {
        setShow(false);
    }

    return (
        <View style={[style.inputContainer, props.style]}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <RectButton {...props} onPress={() => setShow(true)} rippleColor="#DDDDDD">
                <Text style={[style.textInput]}>{props.value?.toString()}</Text>
            </RectButton>
            <Modal visible={show} transparent={true} onRequestClose={() => setShow(false)} animationType="slide"
                   statusBarTranslucent={true}>
                <GestureHandlerRootView style={style.modalHost}>
                    <View style={style.modalView}>
                        <Text>Please Select Date{timePicker && "/Time"}</Text>

                        <DatePickerContent {...props}/>
                        {timePicker && <TimePickerContent {...props}/>}

                        <Button onPress={onChange} variant="success" style={{alignSelf: "stretch"}}>Done</Button>
                    </View>
                </GestureHandlerRootView>
            </Modal>
        </View>
    );
}
