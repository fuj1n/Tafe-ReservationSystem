import {createContext} from "react";
import AsyncStorage from '@react-native-async-storage/async-storage';

/**
 * The endpoint URL for the API
 * @type {string}
 */
const endpoint = "https://localhost:7264/api/v1/";

/**
 * The login state
 * @
 */
export class LoginInfo {
    /**
     * @type {boolean}
     */
    isLoggedIn;

    /**
     * @type {string}
     */
    jwt;

    /**
     * @type {{authorized : boolean, username : string, roles : string[]}}
     */
    user;

    /**
     * @type {string | null}
     */
    error;

    constructor(error = null) {
        this.isLoggedIn = false;
        this.error = error;
    }
}

/**
 * The login context to be used throughout the app
 * @type {React.Context<{loginInfo: LoginInfo, setLoginInfo: function(LoginInfo)}>}
 */
export const LoginContext = createContext(null);

/**
 * Retrieve the login state from the storage
 * @returns {Promise<LoginInfo>} The login state
 */
async function getLogin() {
    const jwt = await AsyncStorage.getItem("authorization");

    return await getLoginFromToken(jwt);
}

/**
 * Retrieve the login state from the token
 * @param jwt {string} The JWT token
 * @returns {Promise<LoginInfo>} The login state
 */
async function getLoginFromToken(jwt) {
    if (!jwt) {
        return new LoginInfo();
    }

    const response = await apiFetch("user/me", "GET", null, jwt);
    if (response.status !== 200) {
        return new LoginInfo(errorToString(response));
    }

    const json = await response.json();

    if (json.authorized) {
        let info = new LoginInfo();
        info.isLoggedIn = true;
        info.jwt = jwt;
        info.user = json;
        return info;
    }

    return new LoginInfo();
}

/**
 * Log in the user
 * @param username {string} The username
 * @param password {string} The password
 * @returns {Promise<LoginInfo>} The login state, isLoggedIn is true if the login was successful
 */
async function login(username, password) {
    const response = await apiFetch("token", "POST", {email: username, password: password});
    if (response.status !== 200) {
        return new LoginInfo(errorToString(response));
    }

    const jwt = await response.text();

    await AsyncStorage.setItem("authorization", jwt);
    return await getLoginFromToken(jwt);
}

/**
 * Log out the user
 * @returns {Promise<LoginInfo>} The login state after logging out (empty)
 */
async function logout() {
    await AsyncStorage.removeItem("authorization");
    return new LoginInfo();
}

/**
 * Perform an API call with optinal authorization
 * @param url {string} The text to append to the endpoint URL
 * @param method {string} The HTTP method to use (GET, POST, etc.)
 * @param [body] {object | null} The body of the request or null if not applicable (i.e. GET)
 * @param [jwt] {string | null} The JWT token or null if not applicable
 * @returns {Promise<Response>}
 */
async function apiFetch(url, method, body, jwt) {
    return fetch(`${endpoint}${url}`, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            "Authorization": jwt ? `Bearer ${jwt}` : ""
        },
        body: body ? JSON.stringify(body) : null

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

function errorToString(response) {
    if(response.internalError) {
        return response.statusText;
    }

    switch(response.status) {
        case 400:
            return "Invalid username or password";
        default:
            return "Unknown error";
    }
}

export default {login, logout, getLogin, apiFetch};
