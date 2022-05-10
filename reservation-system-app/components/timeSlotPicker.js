import style from "./style";
import {Text, View} from "react-native";
import Radio, {RadioGroup} from "./radio";
import moment from "moment";
import {useState} from "react";

/**
 * @param props {{label: string, value: Moment, setValue: function(Moment), timeSlots: Array<Moment>}}
 */
export default function TimeSlotPicker(props) {
    const {label, timeSlots} = props;

    const [value, setValue] = useState(props.value?.unix());

    function onChange(value) {
        setValue(value);
        props.setValue(moment.unix(value));
    }

    if(!timeSlots) {
        throw new Error("The timeSlots attribute is required for time slot picker");
    }

    const formatMap = timeSlots
        .map(slot => [slot.unix(), slot.format("hh:mm A")])

    return (
        <View style={[style.inputContainer, props.style]}>
            {label && <Text style={style.inputLabel}>{label}</Text>}
            <RadioGroup mode="button" direction="row" style={{flexWrap: 'wrap', justifyContent: 'center'}} itemStyle={{flexBasis: '33.3%'}} value={value} onChange={onChange}>
                {formatMap.map(([time, fmt]) => (
                        <Radio key={time} label={fmt} value={time}/>
                    ))}
            </RadioGroup>
        </View>
    );
}
