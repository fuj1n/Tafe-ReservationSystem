import common from "./common";
import {createContext} from "react";

/**
 * @typedef {Object} Restaurant
 * @property {string} name
 * @property {string} address
 * @property {string} phoneNumber
 * @property {string} email
 *
 * @type {React.Context<Restaurant>}
 */
const RestaurantContext = createContext(null);

/**
 * @returns {Promise<* | ErrorDesc>}
 */
async function getRestaurant() {
    const response = await common.fetch(`restaurant`, "GET");

    if (response.ok) {
        return await response.json();
    }

    return await common.processError(response);
}

export default {
    getRestaurant, RestaurantContext
};
