import {TextInput as NativeTextInput} from "react-native";
import style from "./style";
import {Text, View} from "react-native";
import Radio, {RadioGroup} from "./radio";

/**
 * @param props {{label: string, value: Date, setValue: function(Date), timeSlots: Array<Date | string>}}
 */
export default function TimeSlotPicker(props) {
    //TODO: proper date picker

    const {label, value, setValue, timeSlots} = props;

    function onChange(value) {
        setValue?.(new Date(value));
    }

    if(!timeSlots) {
        throw new Error("The timeSlots attribute is required for time slot picker");
    }

    const formatMap = timeSlots
        .map(slot => new Date(slot.toString())) // ensures the provided slots are a date and not just string
        .map(slot => [slot.getUTCDate(), slot.toLocaleTimeString([], {timeStyle: "short"})]);

    return (
        <View style={[style.inputContainer, props.style]}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <RadioGroup mode="button" direction="row">
                <Radio label="Test1" value={2}/>
            </RadioGroup>
        </View>
    );
}
