import style, {variants} from "./style";
import {Modal, Text, View} from "react-native";
import {GestureHandlerRootView, RectButton} from "react-native-gesture-handler";
import {useState} from "react";
import Button from "./button";
import moment from "moment";
import {misc} from "../utility";
import TextInput from "./textInput"
import MaterialIcons from "@expo/vector-icons/MaterialIcons";

function DatePickerContent(props) {
    const {date, setDate} = props;

    const localeData = moment.localeData();
    const firstDayOfWeek = localeData.firstDayOfWeek();

    const [workingDate, setWorkingDate] = useState(moment(date));

    const selectedMonth = workingDate.month();
    const isHomeMonth = workingDate.month() === date.month() && workingDate.year() === date.year();

    const firstDay = workingDate.clone().startOf("month").startOf("week");
    const lastDay = workingDate.clone().endOf("month").endOf("week");

    const daysGrid = [[]];
    let day = firstDay;
    let counter = 0;
    while (day <= lastDay) {
        daysGrid[daysGrid.length - 1].push(day);
        day = day.clone().add(1, "day");
        counter++;
        if (counter > 6) {
            counter = 0;
            if (day <= lastDay) {
                daysGrid.push([]);
            }
        }
    }

    // Keep number of rows consistent to avoid layout changes
    while (daysGrid.length < 6) {
        daysGrid.push(null);
    }

    function changeMonth(direction) {
        let newMonth = workingDate.month() + direction;
        let newYear = workingDate.year();
        if (newMonth < 0) {
            newMonth = 11;
            newYear--;
        } else if (newMonth > 11) {
            newMonth = 0;
            newYear++;
        }
        setWorkingDate(moment(date).set("month", newMonth).set("year", newYear));
    }

    /**
     * @param value {moment.Moment}
     */
    function changeDate(value) {
        let newDate = date.clone();
        newDate.set("date", value.date()).set("month", value.month()).set("year", value.year());
        setDate(newDate);
        setWorkingDate(newDate);
    }

    return (
        <View style={[style.column, {marginBottom: 5, width: 300}]}>
            <View style={[style.row, {justifyContent: 'space-between', alignItems: 'center'}]}>
                <RectButton onPress={() => changeMonth(-1)} rippleColor="#DDDDDD">
                    <MaterialIcons name="keyboard-arrow-left" size={48}/>
                </RectButton>
                <Text style={{
                    width: 120,
                    textAlign: 'center'
                }}>{localeData.months()[selectedMonth]} {workingDate.year()}</Text>
                <RectButton onPress={() => changeMonth(1)} rippleColor="#DDDDDD">
                    <MaterialIcons name="keyboard-arrow-right" size={48}/>
                </RectButton>
            </View>
            <View style={[style.row, {justifyContent: 'center', alignItems: 'center'}]}>
                {misc.overflowMap(firstDayOfWeek, moment.weekdaysMin(), (day, index) => (
                    <View style={{
                        width: 32, height: 32, margin: 1, justifyContent: 'center', alignItems: 'center',
                        backgroundColor: '#333', borderRadius: 5
                    }}
                          key={index}>
                        <Text style={{color: '#FFF'}}>{day}</Text>
                    </View>
                ))}
            </View>
            <View style={[style.column, {justifyContent: 'center'}]}>
                {daysGrid.map((week, weekIdx) => (
                    <View key={weekIdx} style={[style.row, {justifyContent: 'center', height: 32}]}>
                        {week && week.map((day, dayIdx) => (
                            <View key={dayIdx}
                                  style={{
                                      backgroundColor: isHomeMonth && day.month() === selectedMonth && day.date() === date.date() ? variants.Primary.color :
                                          day.month() === selectedMonth ? '#FFFFFF' : '#F5F5F5',
                                      borderColor: '#DDDDDD',
                                      borderWidth: day.month() === selectedMonth ? 1 : 0,
                                      borderRadius: 5,
                                      opacity: day.month() === selectedMonth ? 1 : 0.5,
                                      width: 32,
                                      height: 32,
                                      margin: 1,
                                      justifyContent: 'center',
                                      alignItems: 'center'
                                  }}>
                                <RectButton
                                    onPress={() => changeDate(day)}
                                    rippleColor="#DDDDDD"
                                    style={{
                                        width: 32,
                                        height: 32,
                                        justifyContent: 'center',
                                        alignItems: 'center'
                                    }}>
                                    <Text style={{
                                        color: isHomeMonth && day.month() === selectedMonth && day.date() === date.date() ? '#FFFFFF' :
                                            '#000000',
                                    }}>{day.date()}</Text>
                                </RectButton>
                            </View>
                        ))}
                    </View>
                ))}
            </View>
        </View>
    );
}

function TimePickerContent(props) {
    const {time, setTime} = props;

    const showSeconds = props.showSeconds ?? false;
    const minuteStep = props.minuteStep ?? 15;
    const secondStep = props.secondStep ?? 15;

    const timeElementStyle = {
        width: 48,
        height: 48,
        justifyContent: 'center',
        alignItems: 'center',
        textAlign: 'center',
        margin: 1
    };

    function TimeElement(props) {
        const {value, hasEdit, increase, decrease} = props;
        return (
            <View style={[style.column]}>
                {hasEdit && (
                    <RectButton
                        style={timeElementStyle}
                        rippleColor="#DDDDDD"
                        onPress={decrease}>
                        <MaterialIcons name="keyboard-arrow-up" size={48}/>
                    </RectButton>
                )}
                <View style={timeElementStyle}>
                    <Text style={{fontSize: 24}}>{value}</Text>
                </View>
                {hasEdit && (
                    <RectButton
                        rippleColor="#DDDDDD"
                        style={timeElementStyle}
                        onPress={increase}>
                        <MaterialIcons name="keyboard-arrow-down" size={48}/>
                    </RectButton>
                )}
            </View>
        );
    }

    return (
        <View style={[style.row, {justifyContent: 'center', alignItems: 'center', marginBottom: 5, width: 200}]}>
            <TimeElement value={time.format('hh')} hasEdit={true} increase={() => setTime(time.clone().add(1, 'hour'))}
                         decrease={() => setTime(time.clone().subtract(1, 'hour'))}/>
            <TimeElement value={":"}/>
            <TimeElement value={time.format('mm')} hasEdit={true}
                         increase={() => setTime(time.clone().add(minuteStep, 'minute'))}
                         decrease={() => setTime(time.clone().subtract(minuteStep, 'minute'))}/>
            {showSeconds && <>
                <TimeElement value={":"}/>
                <TimeElement value={time.format('ss')} hasEdit={true}
                             increase={() => setTime(time.clone().add(secondStep, 'second'))}
                             decrease={() => setTime(time.clone().subtract(secondStep, 'second'))}/>
            </>}
            <TimeElement value={time.format('A')} hasEdit={false}/>
        </View>
    );
}

/**
 * @param props {{label: string, value: string, onChange: function(string), timePicker : boolean,
 * minuteStep : number, showSeconds : boolean, secondStep : number}}
 */
export default function DatePicker(props) {
    const {label} = props;

    return (
        <TextInput {...props} value={props.value.toISOString(true)} onChangeText={v => props.setValue(moment(v))}/>
    )

    const [show, setShow] = useState(false);
    const [value, setValue] = useState(props.value ? moment(props.value) : moment());

    const timePicker = props.timePicker ?? true;
    const format = timePicker ? "DD/MM/YYYY hh:mm A" : "DD/MM/YYYY";

    const onChange = (value) => {
        setValue(value);
        props.onChange?.(value.toISOString(true));
    };

    return (
        <View style={[style.inputContainer, props.style]}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <RectButton {...props} onPress={() => setShow(true)} rippleColor="#DDDDDD">
                <Text style={[style.textInput]}>{value.format(format)}</Text>
            </RectButton>
            <Modal visible={show} transparent={true} onRequestClose={() => setShow(false)} animationType="slide"
                   statusBarTranslucent={true}>
                <GestureHandlerRootView style={style.modalHost}>
                    <View style={style.modalView}>
                        <Text>Please Select Date{timePicker && "/Time"}</Text>

                        <View style={[style.column, {
                            justifyContent: 'center',
                            alignItems: 'center',
                            alignSelf: 'stretch'
                        }]}>
                            <DatePickerContent {...props} date={value} setDate={onChange}/>
                            {timePicker && <TimePickerContent {...props} time={value} setTime={onChange}/>}
                        </View>

                        <Button onPress={() => setShow(false)} variant="success"
                                style={{alignSelf: "stretch"}}>Done</Button>
                    </View>
                </GestureHandlerRootView>
            </Modal>
        </View>
    );
}
