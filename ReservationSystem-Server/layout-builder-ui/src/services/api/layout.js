import common from './common';

async function getAreaLayout() {
    // if(common.shouldFudge) {
    //     return common.fudgeResponse([
    //         {
    //             id: 1,
    //             name: 'Area 1',
    //             rect: {
    //                 x: 50 - 25,
    //                 y: 50 - 25,
    //                 width: 25,
    //                 height: 25,
    //                 color: {r: 192, g: 0, b: 0, a: 255}
    //
    //             }
    //         },
    //         {
    //             id: 2,
    //             name: 'Area 2',
    //             rect: {
    //                 x: 50,
    //                 y: 50,
    //                 width: 25,
    //                 height: 25,
    //                 color: {r: 0, g: 140, b: 0, a: 255}
    //             }
    //         }
    //     ]);
    // }
    return await common.fetch('AreaLayout');
}

async function putAreaLayout(layout) {
    return await common.fetch('AreaLayout', 'PUT', layout);
}

async function getTableTypes() {
    // if (common.shouldFudge) {
    //     return common.fudgeResponse([
    //         {
    //             id: 1,
    //             name: 'Table Type 1',
    //             seats: 2,
    //
    //             width: 25,
    //             height: 25,
    //
    //             seats: 2,
    //             rects: [
    //                 {
    //                     x: 50 - 25,
    //                     y: 50 - 25,
    //                     width: 25,
    //                     height: 25,
    //                     color: {r: 192, g: 0, b: 0, a: 255}
    //                 }
    //             ]
    //         }
    //     ]);
    // }

    return await common.fetch('TableTypes');
}

async function putTableTypes(tableTypes) {
    return await common.fetch('TableTypes', 'PUT', tableTypes);
}

const layout = {getAreaLayout, putAreaLayout, getTableTypes, putTableTypes};
export default layout;
