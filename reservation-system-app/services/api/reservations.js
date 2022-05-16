import common from "./common";

/**
 * @returns {Promise<*[] | ErrorDesc>} Array of reservations
 */
async function getReservationsAsAdmin(jwt, sittingId) {
    const response = await common.fetch(`admin/reservation/list/${sittingId}`, "GET", null, jwt);

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

export default {getReservationsAsAdmin, getStatuses};
