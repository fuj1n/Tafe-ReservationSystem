import {View, Text} from "react-native";
import moment from "moment";
import {RectButton} from "react-native-gesture-handler";
import StyledText from "./styledText";

/**
 * @typedef Reservation Object
 * @typedef ReservationAcceptor function(Reservation) : *
 * @param props {{reservations : Reservation[], timeOutFormat : string, onSelected : function(Reservation),
 * customerNameSelector : ReservationAcceptor, startTimeSelector : ReservationAcceptor, endTimeSelector : ReservationAcceptor}}
 */
export default function ReservationPicker(props) {
    const {reservations} = props;
    const customerNameSelector = props.customerNameSelector ?? (s => s.sittingType?.description);
    const startTimeSelector = props.startTimeSelector ?? (s => s.startTime);
    const endTimeSelector = props.endTimeSelector ?? (s => s.endTime);
    const timeFormat = props.timeOutFormat ?? "hh:mm A";

    if (!reservations) {
        throw new Error('ReservationPicker requires reservations');
    }

    const topStyle = {
        borderTopLeftRadius: 10,
        borderTopRightRadius: 10
    }

    const bottomStyle = {
        borderBottomLeftRadius: 10,
        borderBottomRightRadius: 10
    }

    if(reservations.length === 0) {
        return (
            <View style={{alignItems: "center"}}>
                <StyledText variant="danger">No reservations available</StyledText>
            </View>
        );
    }

    return (
        <View>
            {reservations.map((r, index) => {
                return (
                    <View key={index} style={[{
                        backgroundColor: '#FFF', borderWidth: 1, borderTopWidth: 0, borderColor: 'rgba(0,0,0,.125)'
                    }, index === reservations.length - 1 ? bottomStyle : index === 0 ? topStyle : null]}>
                        <RectButton rippleColor="#cff4fc" style={{
                            flex: 1, justifyContent: 'center', alignItems: 'center', paddingVertical: 12
                        }} onPress={() => props.onSelected?.(r)}>
                            <Text>
                                {customerNameSelector(r)} - {moment(startTimeSelector(r)).format(timeFormat)} to{' '}
                                {moment(endTimeSelector(r)).format(timeFormat)}
                            </Text>
                        </RectButton>
                    </View>
                );
            })}
        </View>
    );
}
