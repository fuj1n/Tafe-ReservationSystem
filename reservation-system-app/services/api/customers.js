import common from "./common";

/**
 * @returns {Promise<* | ErrorDesc>}
 */
async function getCustomerByIdAsAdmin(jwt, customerId) {
    const response = await common.fetch(`admin/customer/${customerId}`, "GET", null, jwt);

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

export default {
    getCustomerByIdAsAdmin
};
