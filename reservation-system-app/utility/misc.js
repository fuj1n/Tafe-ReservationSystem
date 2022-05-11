/**
 * Similar to the map function, but with a start position and overflow
 * @param first {number} The index of the first element
 * @param data {Array} The array to map
 * @param func {Function} The function to apply
 * @returns {*[]} The mapped array
 */
function overflowMap(first, data, func) {
    let result = [];
    for(let i = first; i < data.length + first; i++) {
        let idx = i % data.length;
        result.push(func(data[idx], idx));
    }
    return result;
}

export default {overflowMap};
