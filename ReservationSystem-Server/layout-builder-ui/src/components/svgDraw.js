/**
 * @typedef {Object} Color
 * @property {number} r
 * @property {number} g
 * @property {number} b
 * @property {number} a
 */

/**
 * @typedef {Object} Rect
 * @property {number} x
 * @property {number} y
 * @property {number} width
 * @property {number} height
 * @property {Color} color
 */

import {useEffect, useState} from "react";

/**
 * @param color {Color}
 * @returns {string}
 */
function processColor(color) {
    if (color instanceof String || typeof color === 'string') {
        return color;
    }

    return `rgba(${color?.r ?? 255}, ${color?.g ?? 0}, ${color?.b ?? 255}, ${(color?.a ?? 255) / 255.0})`;
}

/**
 * @param color {Color}
 * @param by {number}
 * @returns {Color}
 */
function darkenColor(color, by) {
    return {
        r: color.r / by,
        g: color.g / by,
        b: color.b / by,
        a: color.a
    };
}

/**
 * @summary Transform child from parent percentage coordinate to SVG percentage coordinate
 * @param parent {Rect}
 * @param child {Rect}
 */
function transform(parent, child) {
    return {
        x: parent.x + (child.x * parent.width / 100),
        y: parent.y + (child.y * parent.height / 100),
        width: child.width,
        height: child.height,
        color: child.color
    };
}

function Rect({rect, extra}) {
    return (
        <rect x={`${rect.x}%`} y={`${rect.y}%`} width={`${rect.width}%`} height={`${rect.height}%`}
              fill={processColor(rect.color)} {...extra}/>
    );
}

function Circle({cx, cy, radius, color, extra}) {
    return (
        <circle cx={`${cx}%`} cy={`${cy}%`} r={`${radius}%`} fill={processColor(color)} {...extra}/>
    );
}

function Label({x, y, text, color, extra}) {
    return (
        <text x={`${x}%`} y={`${y}%`} fill={processColor(color)} fontWeight={700} dominantBaseline="hanging"
              {...extra}>{text}</text>
    );
}

function MoveResizeHandle({rect, updateRect, canMove, canResize}) {
    const [resizingHandle, setResizingHandle] = useState(null);

    const corners = [
        {
            x: rect.x, y: rect.y, handle: 'nw-resize', updateRect: (newX, newY) => ({
                x: newX,
                y: newY,
                width: rect.width - (newX - rect.x),
                height: rect.height - (newY - rect.y)
            })
        },
        {
            x: rect.x + rect.width, y: rect.y, handle: 'ne-resize', updateRect: (newX, newY) => ({
                x: rect.x,
                y: newY,
                width: newX - rect.x,
                height: rect.height - (newY - rect.y)
            })
        },
        {
            x: rect.x, y: rect.y + rect.height, handle: 'sw-resize', updateRect: (newX, newY) => ({
                x: newX,
                y: rect.y,
                width: rect.width - (newX - rect.x),
                height: newY - rect.y
            })
        },
        {
            x: rect.x + rect.width, y: rect.y + rect.height, handle: 'se-resize', updateRect: (newX, newY) => ({
                x: rect.x,
                y: rect.y,
                width: newX - rect.x,
                height: newY - rect.y
            })
        },
        {
            x: rect.x + rect.width / 2, y: rect.y + rect.height / 2, hide: true, handle: 'center-resize', updateRect: (newX, newY, distanceFromCenter) => ({
                width: distanceFromCenter * 1.5,
                height: distanceFromCenter * 1.5,
                x: rect.x + rect.width / 2 - distanceFromCenter * 1.5 / 2,
                y: rect.y + rect.height / 2 - distanceFromCenter * 1.5 / 2
            })
        },
        {
            x: rect.x + rect.width / 2, y: rect.y + rect.height / 2, handle: 'move', updateRect: (newX, newY) => ({
                x: newX - rect.width / 2,
                y: newY - rect.height / 2,
                width: rect.width,
                height: rect.height
            })
        }
    ];

    function onMouseUp() {
        setResizingHandle(null);
    }

    function onMouseMove(e) {
        if (resizingHandle) {
            const target = document.getElementById('svg-root');
            const targetBounds = target.getBoundingClientRect();

            const localX = e.clientX - targetBounds.left;
            const localY = e.clientY - targetBounds.top;

            const localXPercent = localX / targetBounds.width * 100;
            const localYPercent = localY / targetBounds.height * 100;

            const distanceFromCenter = Math.sqrt(Math.pow(localXPercent - rect.x - rect.width / 2, 2) + Math.pow(localYPercent - rect.y - rect.height / 2, 2));

            const effectiveHandle = e.altKey ? 'center-resize' : resizingHandle;
            let newRect = corners.find(h => h.handle === effectiveHandle).updateRect(localXPercent, localYPercent, distanceFromCenter);

            // if a coordinate is out of allowed bounds or NaN, cancel out the change
            if(newRect.x < 0 || newRect.x + newRect.width > 100 || newRect.width < 5 || isNaN(newRect.x) || isNaN(newRect.width)) {
                newRect.x = rect.x;
                newRect.width = rect.width;
            }
            if(newRect.y < 0 || newRect.y + newRect.height > 100 || newRect.height < 5 || isNaN(newRect.y) || isNaN(newRect.height)) {
                newRect.y = rect.y;
                newRect.height = rect.height;
            }

            newRect.color = rect.color;

            updateRect(newRect);
        }
    }

    useEffect(() => {
        document.addEventListener('mouseup', onMouseUp);
        document.addEventListener('mousemove', onMouseMove);
        return () => {
            document.removeEventListener('mouseup', onMouseUp);
            document.removeEventListener('mousemove', onMouseMove);
        };
    });

    return (
        <g>
            {corners.map((c, index) => (
                !c.hide && <Circle key={index} cx={c.x} cy={c.y} radius={0.5} color="#00AFFF"
                        extra={{
                            stroke: '#005780', strokeWidth: 2, style: {cursor: c.handle},
                            onMouseDown: e => {setResizingHandle(c.handle); e.stopPropagation()},
                            onClick: e => e.stopPropagation()
                        }}/>
            ))}
        </g>
    );
}

const SvgDraw = {
    processColor,
    darkenColor,
    transform,
    Rect,
    Circle,
    Label,
    MoveResizeHandle
};

export default SvgDraw;
