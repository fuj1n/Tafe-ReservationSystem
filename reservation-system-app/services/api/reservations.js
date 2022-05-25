import common from "./common";

/**
 * @returns {Promise<*[] | ErrorDesc>} Array of reservations
 */
async function getReservationsAsAdmin(jwt, sittingId) {
    const response = await common.fetch(`admin/reservation/${sittingId}`, "GET", null, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<Object.<string, string> | ErrorDesc>} A status id to description map
 */
async function getStatuses() {
    const response = await common.fetch("admin/reservation/statuses", "GET");

    if (response.ok) {
        const result = await response.json();
        return result.reduce((acc, status) => {
            acc[status.id] = status.description;
            return acc;
        }, {});
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<string | ErrorDesc>}
 */
async function getStatusById(statusId) {
    const response = await common.fetch(`admin/reservation/status/${statusId}`, "GET");

    if (response.ok) {
        return (await response.json()).description;
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<Object.<string, string> | ErrorDesc>} An origin id to description map
 */
async function getOrigins(jwt) {
    const response = await common.fetch("admin/reservation/origins", "GET", null, jwt);

    if (response.ok) {
        const result = await response.json();
        return result.reduce((acc, origin) => {
            acc[origin.id] = origin.description;
            return acc;
        }, {});
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<string | ErrorDesc>}
 */
async function getOriginById(jwt, originId) {
    const response = await common.fetch(`admin/reservation/origin/${originId}`, "GET", null, jwt);

    if (response.ok) {
        return (await response.json()).description;
    }

    return await common.processError(response);
}

/**
 * @typedef {Object} StatusBadgeVisual
 * @property {number} id
 * @property {string} htmlBadgeClass
 * @property {string} reactBadgeVariant
 * @returns {Promise<Object.<string, StatusBadgeVisual> | ErrorDesc>}
 */
async function getStatusBadgeVisuals(jwt) {
    const response = await common.fetch("admin/reservation/status/badges", "GET", null, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

/**
 * @returns {Promise<* | ErrorDesc>}
 */
async function setStatus(jwt, reservationId, statusId) {
    const response = await common.fetch(`admin/reservation/${reservationId}/status?statusId=${statusId}`, "PUT", null, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

export default {getReservationsAsAdmin, getStatuses, getStatusById, getOrigins, getOriginById, getStatusBadgeVisuals, setStatus};
