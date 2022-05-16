import {StyleSheet} from "react-native";

const inputStyle = {
    paddingHorizontal: 6,
    paddingVertical: 12,
    fontWeight: '400',
    color: '#212529',
    backgroundColor: '#fff',
    borderWidth: 1,
    borderStyle: 'solid',
    borderColor: '#ced4da',
    borderRadius: 4
};

const baseStyle = {
    inputContainer: {
        fontWeight: '400',
        color: '#212529',
        alignSelf: 'stretch',
    },

    inputLabel: {
        marginBottom: 4
    },
    textInput: {
        ...inputStyle
    },

    dropdownInput: {
        ...inputStyle,
        paddingVertical: 0
    },
    dropdownIosInput: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        flex: 1
    },

    dropdownIosViewContainer: {
        alignSelf: "center",
        flex: 1,
        paddingVertical: 12
    },
    // only applies to web as other platforms use native picker
    dropdownWebInputStyle: {
        borderWidth: 0,
        paddingVertical: 12
    },

    radioHost: {

    },
    radioItemCircle: {
        flexDirection: "row",
        alignItems: "center",
        paddingVertical: 12,
        paddingHorizontal: 6,
        marginRight: 6
    },
    radioItemButton: {
        paddingVertical: 2,
        paddingHorizontal: 2,
    },
    radioIcon: {
        marginRight: 6
    },

    modalHost: {
        flex: 1,
        justifyContent: "flex-end",
        backgroundColor: 'rgba(0,0,0,0.5)'
    },
    modalView: {
        margin: 20,
        marginBottom: 0,
        backgroundColor: "white",
        padding: 35,
        alignItems: "center",
        borderWidth: 2,
        borderBottomWidth: 0,
        borderColor: '#0d6efd',
        borderTopStartRadius: 5,
        borderTopEndRadius: 5
    },

    icon28: {
        width: 28,
        height: 27 // Not same as width to fix centering
    },

    row: {
        flexDirection: 'row'
    },
    column: {
        flexDirection: 'column'
    }
};

export const variants = {
    none: null, // used for default generator
    Primary: {
        color: '#0d6efd',
        borderColor: '#0d6efd',
        Hover: {
            color: '#fff',
            backgroundColor: '#0d6efd'
        }
    },
    Secondary: {
        color: '#6c757d',
        borderColor: '#6c757d',
        Hover: {
            color: '#fff',
            backgroundColor: '#6c757d'
        }
    },
    Success: {
        color: '#198754',
        borderColor: '#198754',
        Hover: {
            color: '#fff',
            backgroundColor: '#198754'
        }
    },
    Info: {
        color: '#0dcaf0',
        borderColor: '#0dcaf0',
        Hover: {
            color: '#fff',
            backgroundColor: '#0dcaf0'
        }
    },
    Warning: {
        color: '#ffc107',
        borderColor: '#ffc107',
        Hover: {
            color: '#fff',
            backgroundColor: '#ffc107'
        }
    },
    Danger: {
        color: '#dc3545',
        borderColor: '#dc3545',
        Hover: {
            color: '#fff',
            backgroundColor: '#dc3545'
        }
    },
    Dark: {
        color: '#212529',
        borderColor: '#212529',
        Hover: {
            color: '#fff',
            backgroundColor: '#212529'
        }
    },
    Light: {
        color: '#f8f9fa',
        borderColor: '#f8f9fa',
        Hover: {
            color: '#000',
            backgroundColor: '#f8f9fa'
        }
    },
};

const states = ['', 'Hover'];

const generators = {
    button: (name, variant, state) => {
        let output = {};

        if (!variant) {
            output.button = {
                borderRadius: 4,
                alignItems: 'center',
                justifyContent: 'center',
                paddingHorizontal: 6,
                paddingVertical: 12,
                borderWidth: 1,
                borderStyle: 'solid',
            };
            output.buttonText = {
                fontWeight: '400'
            };

            return output;
        }

        output[`button${name}${state}`] = {
            backgroundColor: variant.backgroundColor ?? 'transparent',
            borderColor: variant.borderColor ?? 'transparent'
        };

        output[`buttonText${name}${state}`] = {
            color: variant.color ?? 'transparent'
        };

        return output;
    },
    text: (name, variant, state) => {
        let output = {};

        if(state) {
            return {};
        }

        if (!variant) {
            output.text = {
                fontWeight: '400',
                color: '#212529'
            }

            return output;
        }

        output[`text${name}`] = {
            color: variant.color ?? 'transparent'
        }

        return output;
    },
    bg: (name, variant, state) => {
        let output = {};

        if(!variant) {
            return {};
        }

        output[`bg${name}${state}`] = {
            backgroundColor: variant.backgroundColor ?? 'transparent'
        }

        return output;
    },
    fg: (name, variant, state) => {
        let output = {};

        if(!variant) {
            return {};
        }

        output[`fg${name}${state}`] = {
            color: variant.color ?? 'transparent'
        }

        return output;
    }
};

// This is ugly, but it does a lot of work for how little code it is
// Runs all generators for each variant and state
// Though I would normally prefer to have something like this pre-compiled
const style = StyleSheet.create({
        ...baseStyle
        , ...Object.entries(variants).reduce((acc, [name, variant]) => {
            return {
                ...acc,
                ...states.reduce((acc, state) => {
                    return {
                        ...acc,
                        ...Object.values(generators).reduce((acc, generator) => {
                            return {
                                ...acc,
                                ...generator(name, variant && state !== '' ? variant[state] : variant, state)
                            };
                        }, {}) // end generator reduce
                    };
                }, {}) // end state reduce
            };
        }, {}) // end variant reduce
    }
);

/**
 * @param component {string} - The style component
 * @param variant {string} - The style variant
 * @param [hover] {string} - the current hover state (optional)
 */
export function getVariant(component, variant, hover) {
    if(!variant) {
        return style[component];
    }

    // Capitalize first letter to match style naming
    variant = variant.charAt(0).toUpperCase() + variant.slice(1);
    return style[`${component}${variant}${hover ? 'Hover' : ''}`];
}

export default style;
