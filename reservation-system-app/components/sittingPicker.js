import {View, Text} from "react-native";
import moment from "moment";
import {RectButton} from "react-native-gesture-handler";
import StyledText from "./styledText";

/**
 * @typedef Sitting Object
 * @typedef SittingAcceptor function(Sitting) : *
 * @param props {{sittings : Sitting[], dateOutFormat : string, timeOutFormat : string, onSelected : function(Sitting),
 * sittingTypeSelector : SittingAcceptor, startTimeSelector : SittingAcceptor,
 * endTimeSelector : SittingAcceptor}}
 */
export default function SittingPicker(props) {
    const {sittings} = props;
    const sittingTypeSelector = props.sittingTypeSelector ?? (s => s.sittingType?.description);
    const startTimeSelector = props.startTimeSelector ?? (s => s.startTime);
    const endTimeSelector = props.endTimeSelector ?? (s => s.endTime);
    const dateFormat = props.dateOutFormat ?? "DD/MM/YYYY";
    const timeFormat = props.timeOutFormat ?? "hh:mm A";

    if (!sittings) {
        throw new Error('SittingPicker requires sittings');
    }

    let currentDate = moment("01/01/0001", "DD/MM/YYYY");

    const bottomStyle = {
        borderBottomLeftRadius: 10,
        borderBottomRightRadius: 10
    }

    if(sittings.length === 0) {
        return (
            <View style={{alignItems: "center"}}>
                <StyledText variant="danger">No sittings available</StyledText>
            </View>
        );
    }

    return (
        <View>
            {sittings.map((s, index) => {
                let prepend = null;
                const thisDate = moment(startTimeSelector(s)).set({hour: 0, minute: 0, second: 0, millisecond: 0});
                if (!thisDate.isSame(currentDate)) {
                    currentDate = thisDate;

                    const radius = index === 0 ? 10 : 0;

                    prepend = (
                        <View key={thisDate.unix()} style={{
                            justifyContent: 'center', alignItems: 'center', flex: 1,
                            paddingVertical: 12, backgroundColor: '#cff4fc', borderTopLeftRadius: radius,
                            borderTopRightRadius: radius, borderWidth: 1, borderTopWidth: index === 0 ? 1 : 0,
                            borderColor: 'rgba(0,0,0,.125)'
                        }}>
                            <Text style={{color: '#055160', fontWeight: '700'}}>{currentDate.format(dateFormat)}</Text>
                        </View>
                    );
                }

                return [
                    prepend,
                    <View key={index} style={[{
                        backgroundColor: '#FFF', borderWidth: 1, borderTopWidth: 0, borderColor: 'rgba(0,0,0,.125)'
                    }, index === sittings.length - 1 ? bottomStyle : null]}>
                        <RectButton rippleColor="#cff4fc" style={{
                            flex: 1, justifyContent: 'center', alignItems: 'center', paddingVertical: 12
                        }} onPress={() => props.onSelected?.(s)}>
                            <Text>
                                {sittingTypeSelector(s)} from {moment(startTimeSelector(s)).format(timeFormat)} to{' '}
                                {moment(endTimeSelector(s)).format(timeFormat)}
                            </Text>
                        </RectButton>
                    </View>
                ];
            })}
        </View>
    );
}
