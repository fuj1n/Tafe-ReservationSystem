import Config from "../../Config";

const shouldFudge = false;

/**
 * @typedef {Object} ErrorDesc
 * @property {boolean} error
 * @property {string} message
 * @property {string[]} [errors]
 */

function fudgeResponse(data) {
    return {
        ok: true,
        status: 200,
        statusText: 'OK',
        json: () => Promise.resolve(data)
    }
}

function apiFetch(endpoint, method = 'GET', body = null) {
    return fetch(new URL(endpoint, Config.baseurl).href, {
        method: method,
        body: method !== 'GET' && body ? JSON.stringify(body) : null,
        headers: {
            "Content-Type": "application/json"
        }
    }).catch(error => {
        console.error(error);
        const response = new Response(null, { status: 599, statusText: error.message });
        response.isInternalError = true;
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

function getStatusCodeMessage(code) {
    // eslint-disable-next-line default-case -- Handle only a handful of the codes, ones that are most likely to be run into.
    switch(code) {
        case 400:
            return "Bad Request";
        case 401:
            return "You must be logged in to access this resource";
        case 403:
            return "You are not authorized to access this resource";
        case 404:
            return "Resource not found, ensure you are using an up-to-date version of the app";
    }
}

/**
 * Processes the error response from the API and maps it into consistent format
 * @param response {Response} The response from the API
 * @param overrides {Object.<string, string>} A custom look-up table for error messages based on status code
 * @returns {Promise<ErrorDesc>} An object with error flag set
 */
async function processError(response, overrides = {}) {
    let error = {error: true, message: overrides[response.status] ?? getStatusCodeMessage(response.status) ?? "An unknown error occurred"};

    if (response.isInternalError) {
        error.message = response.statusText;
    } else {
        if(response.status >= 500) {
            error.message = "An internal error occurred";

            return error;
        }

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
    }

    return error;
}

const common = {fetch: apiFetch, shouldFudge, fudgeResponse, processError}
export default common;
