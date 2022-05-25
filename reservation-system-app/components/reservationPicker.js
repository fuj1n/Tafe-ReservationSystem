import {View, Text} from "react-native";
import moment from "moment";
import {RectButton} from "react-native-gesture-handler";
import StyledText from "./styledText";

/**
 * @typedef Reservation Object
 * @typedef ReservationAcceptor function(Reservation) : *
 * @param props {{reservations : Reservation[], timeOutFormat : string, onSelected : function(Reservation),
 * customerFirstNameSelector : ReservationAcceptor, customerLastNameSelector : ReservationAcceptor,
 * startTimeSelector : ReservationAcceptor, durationSelector : ReservationAcceptor}}
 */
export default function ReservationPicker(props) {
    const {reservations} = props;
    const customerFirstNameSelector = props.customerNameSelector ?? (r => r.customer?.firstName);
    const customerLastNameSelector = props.customerNameSelector ?? (r => r.customer?.lastName);
    const startTimeSelector = props.startTimeSelector ?? (r => r.startTime);
    const durationSelector = props.durationSelector ?? (r => r.duration);
    const timeFormat = props.timeOutFormat ?? "hh:mm A";

    if (!reservations) {
        throw new Error('ReservationPicker requires reservations');
    }

    const topStyle = {
        borderTopLeftRadius: 10,
        borderTopRightRadius: 10,
        borderTopWidth: 1
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
                    }, index === reservations.length - 1 ? bottomStyle : null, index === 0 ? topStyle : null]}>
                        <RectButton rippleColor="#cff4fc" style={{
                            flex: 1, justifyContent: 'center', alignItems: 'center', paddingVertical: 12
                        }} onPress={() => props.onSelected?.(r)}>
                            <Text>
                                {customerFirstNameSelector(r)} {customerLastNameSelector(r)} - {moment(startTimeSelector(r)).format(timeFormat)} to{' '}
                                {(moment(startTimeSelector(r)).add(moment.duration(durationSelector(r)))).format(timeFormat)}
                            </Text>
                        </RectButton>
                    </View>
                );
            })}
        </View>
    );
}
