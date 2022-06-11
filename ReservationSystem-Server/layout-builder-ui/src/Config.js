/**
 * @typedef {Object} ConfigData
 * @property {string} baseurl
 * @property {string} root
 * @property {string} [isdevelopment]
 *
 * @type {ConfigData}
 */
const config = [...document.getElementsByTagName('config')[0].attributes]
    .reduce((acc, {name, value}) => ({...acc, [name]: value}), {});

export default /** @type {ConfigData} */ config;
