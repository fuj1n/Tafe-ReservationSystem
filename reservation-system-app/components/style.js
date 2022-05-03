import {Platform, StyleSheet} from "react-native";

const inputStyle = {
    paddingHorizontal: 6,
    paddingVertical: 12,
    fontWeight: '400',
    lineHeight: 1.5,
    color: '#212529',
    backgroundColor: '#fff',
    borderWidth: 1,
    borderStyle: 'solid',
    borderColor: '#ced4da',
    borderRadius: 4
};

const baseStyle = {
    inputContainer: {
        marginHorizontal: 12,
        fontWeight: '400',
        lineHeight: 1.5,
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
    // only applies to web as other platforms use native picker
    dropdownWebInputStyle: {
        borderWidth: 0,
        paddingVertical: 12
    }
};

const variants = {
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
                fontWeight: '400',
                lineHeight: 14
            };

            // line height is handled weird outside of web
            if (Platform.OS !== 'web') {
                output.buttonText.paddingTop = 14 / 4;
            }

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

export function getVariant(component, variant, hover) {
    // Capitalize first letter to match style naming
    variant = variant.charAt(0).toUpperCase() + variant.slice(1);
    return style[`${component}${variant}${hover ? 'Hover' : ''}`];
}

export default style;
