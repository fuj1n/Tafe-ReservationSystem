import common from './common';

/**
 * @returns {Promise<Object.<string, string> | ErrorDesc>} Dictionary of sitting types
 */
async function getSittingTypes() {
    const response = await common.fetch('sittings/sittingTypes', "GET");

    if (response.ok) {
        const result = await response.json();
        return result.reduce((acc, type) => {
            acc[type.id] = type.description;
            return acc;
        }, {});
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<*[] | ErrorDesc>} Array of sittings
 */
async function getSittings() {
    const response = await common.fetch('sittings', "GET");

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<*[] | ErrorDesc>} Array of sittings
 */
async function getSittingsAsAdmin(jwt, includePast, includeClosed) {
    const queryString = `?includePast=${includePast}&includeClosed=${includeClosed}`;
    //TODO: update endpoint when API is updated
    const response = await common.fetch(`admin/sitting${queryString}`, "GET", null, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<string[] | ErrorDesc>}
 */
async function getTimeSlots(sittingId) {
    const response = await common.fetch(`sittings/timeSlots/${sittingId}`, "GET");

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

export default {getSittingTypes, getSittings, getSittingsAsAdmin, getTimeSlots};
