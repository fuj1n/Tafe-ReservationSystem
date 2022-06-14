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
 * @returns {Promise<* | ErrorDesc>} Array of reservations
 */
async function getReservationsAsMember(jwt) {
    const response = await common.fetch(`member/reservation`, "GET", null, jwt);

    if(response.ok) {
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
async function getOrigins() {
    const response = await common.fetch("admin/reservation/origins", "GET");

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
async function getOriginById(originId) {
    const response = await common.fetch(`admin/reservation/origin/${originId}`, "GET");

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
async function getStatusBadgeVisuals() {
    const response = await common.fetch("admin/reservation/status/badges", "GET");

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

async function createReservationAsAdmin(jwt, reservation) {
    const response = await common.fetch("admin/reservation/create", "POST", reservation, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

async function editReservationAsAdmin(jwt, reservation) {
    const response = await common.fetch(`admin/reservation/${reservation.id}/edit`, "PUT", reservation, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

export default {
    getReservationsAsAdmin,
    getReservationsAsMember,
    getStatuses,
    getStatusById,
    getOrigins,
    getOriginById,
    getStatusBadgeVisuals,
    setStatus,
    createReservationAsAdmin,
    editReservationAsAdmin
};
