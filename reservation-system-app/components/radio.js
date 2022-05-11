import {createContext, useContext, useEffect, useState} from "react";
import {Text, View} from "react-native";
import {RectButton} from "react-native-gesture-handler";
import style from "./style";
import MaterialIcons from "@expo/vector-icons/MaterialIcons";
import Button from "./button";

/**
 * @typedef radioType {"circle"|"button"}
 * @typedef radioDirection {"row"|"column"}
 * @type {React.Context<{value : any, setValue : function(any), style : any, type : radioType}>}
 */
const RadioGroupContext = createContext(null);

/**
 * @param props {{label : string, mode : radioType, direction : radioDirection, style : object, itemStyle : object,
 * value : any, onChange : function(any), children : any[]}}
 */
export function RadioGroup(props) {
    const {label} = props;
    const [value, setValue] = useState(props.value ?? null);

    function onChange(value) {
        setValue(value);
        props.onChange?.(value);
    }

    useEffect(() => {
        if(!value) {
            // Finds first child and sets default value to it
            const first = props?.children?.length >= 1 ? props.children[0] : undefined;
            if(first && first.props) {
                onChange(first.props.value);
            }
        }
    }, [props.children]);

    return (
        <RadioGroupContext.Provider value={{value, setValue: onChange, style: props.itemStyle,
            type: props.mode ?? "circle"}}>
            <View style={[style.inputContainer, props.style]}>
                {label && <Text style={style.inputLabel}>{label}</Text>}
                <View style={[style.radioHost, {flexDirection: props.direction ?? "column"}, props.style]}>
                    {props.children}
                </View>
            </View>
        </RadioGroupContext.Provider>
    )
}

function CircleRadio(props) {
    const {value, setValue, style : customStyle, children, active} = props;

    const icon = active ? "radio-button-on" : "radio-button-off";

    return (
        <View style={customStyle}>
            <RectButton style={style.radioItemCircle} onPress={() => setValue(value)} rippleColor="#DDDDDD">
                <MaterialIcons name={icon} size={28} color="black" style={[style.icon28, style.radioIcon]} />
                <Text>{children}</Text>
            </RectButton>
        </View>
    )
}

function ButtonRadio(props) {
    const {value, setValue, style : customStyle, children, active} = props;

    return (
        <View style={customStyle}>
            <Button style={style.radioItemButton} forceActive={active} onPress={() => setValue(value)} variant="dark">{children}</Button>
        </View>
    )
}

/**
 * @param props {{label: string, value: any}}
 */
export default function Radio(props) {
    const group = useContext(RadioGroupContext);
    if(group === null) {
        throw new Error("A radio must be surrounded by a radio group");
    }
    const {value, setValue, style, type} = group;

    let TypeComponent;

    switch(type) {
        case "circle":
            TypeComponent = CircleRadio;
            break;
        case "button":
            TypeComponent = ButtonRadio;
            break;
    }

    return (
        <TypeComponent style={style} value={props.value} setValue={setValue} active={value === props.value}>
            {props.label ?? props.value}
        </TypeComponent>
    )
}
