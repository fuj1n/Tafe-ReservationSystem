import Constants from "expo-constants";

/**
 * The URL for the API
 * @type {string}
 */
const url = Constants.manifest.extra.apiUrl;

/**
 * @typedef {Object} ErrorDesc
 * @property {boolean} error
 * @property {string} message
 * @property {string[]} [errors]
 */

/**
 * Perform an API call with optional authorization
 * @param endpoint {string} The endpoint to call
 * @param method {string} The HTTP method to use (GET, POST, etc.)
 * @param [body] {object | null} The body of the request or null if not applicable (i.e. GET)
 * @param [jwt] {string | null} The JWT token or null if not applicable
 * @returns {Promise<Response>}
 */
async function apiFetch(endpoint, method, body = null, jwt = null) {
    method = method.toUpperCase();

    return fetch(`${url}${endpoint}`, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            "Authorization": jwt ? `Bearer ${jwt}` : ""
        },
        body: method !== "GET" && body ? JSON.stringify(body) : null

    }).catch(error => {
        // Return a response anyway so that it can be handled elsewhere
        let response = new Response(null, {
            status: 499,
            statusText: error.message,
            ok: false
        });
        response.internalError = true;
        return response;
    });
}

async function processValidationErrorCollection(collection) {
    if(!collection) {
        return null;
    }

    return Object.entries(collection)?.reduce?.((errors, [field, error]) => {
        errors.push({
            field: field,
            message: error
        });

        return errors;
    }, []) ?? null;
}

/**
 * Processes the error response from the API and maps it into consistent format
 * @param response {Response} The response from the API
 * @returns {Promise<ErrorDesc>} An object with error flag set
 */
async function processError(response) {
    let error = {error: true, message: "An unknown error occurred"};

    if (response.internalError) {
        error.message = response.statusText;
    } else {
        const errorObject = await response.json();

        if (!errorObject) {
            return error;
        }

        // Detect various MVC validation errors used throughout API:
        if (response.status === 400) {
            if (errorObject.title?.includes?.("validation errors")) {
                error.message = "Validation failed";
                error.errors = await processValidationErrorCollection(errorObject.errors);
            } else if (errorObject.errorMessage) { // Very rudimentary, to be removed once API is updated
                error.message = "Validation failed";
                error.errors = [{
                    field: "",
                    message: errorObject.errorMessage
                }];
            } else {
                const props = Object.keys(errorObject);
                if (props.length >= 1 && Array.isArray(errorObject[props[0]])) {
                    error.message = "Validation failed";
                    error.errors = await processValidationErrorCollection(errorObject[props[0]]);
                }
            }
        }

        return error;
    }
}

export default {
    url,
    fetch: apiFetch,
    processError
};
