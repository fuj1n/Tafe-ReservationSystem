import {useEffect, useState} from "react";
import {api} from "../services";
import {ErrorDisplay, Loader, SvgDraw} from "../components";
import {SketchPicker} from "react-color";

function TableRectangle({rect, selected, select, updateRect}) {
    const selectEffect = selected ? {
        className: 'marchingAnts',
        style: {stroke: '#000', strokeWidth: '2px', strokeDasharray: '5,5'}
    } : {};

    const extra = {...selectEffect, style: {...selectEffect.style, cursor: 'pointer'}};

    return (
        <g onClick={select}>
            <SvgDraw.Rect rect={rect} extra={extra}/>
            <SvgDraw.MoveResizeHandle rect={rect} updateRect={updateRect} canMove canResize/>
        </g>
    );
}

export default function TableTypesPage() {
    const [isLoading, setIsLoading] = useState(true);
    const [tableTypes, setTableTypes] = useState([]);
    const [error, setError] = useState(null);
    const [blockingError, setBlockingError] = useState(null);
    const [generation, setGeneration] = useState(0);

    const [selection, setSelection] = useState('');
    const selectionInt = selection ? parseInt(selection) : null;
    const selectedTableType = selection ? tableTypes.find(t => t.id === selectionInt) : null;

    const [selectedRect, setSelectedRect] = useState(null);
    const selectedRectObj = selectedRect !== null ? selectedTableType?.rects[selectedRect] : null;

    const [changeColorActive, setChangeColorActive] = useState(false);

    useEffect(() => {
        async function getTableTypes() {
            setIsLoading(true);
            updateSelection('');

            const response = await api.layout.getTableTypes();
            if (response.isInternalError || !response.ok) {
                setBlockingError(`${!response.isInternalError ? response.status : ''} ${response.statusText}`);
            } else {
                setTableTypes(await response.json());
                setBlockingError(null);
            }
            setIsLoading(false);
        }

        getTableTypes().then();
    }, [generation]);

    function updateSelection(to) {
        setSelection(to);
        setSelectedRect(null);
    }

    function newTableType() {
        let id;

        do {
            id = tableTypes.length + Math.floor(Math.random() * 1000000);
            // eslint-disable-next-line no-loop-func -- not a problem here
        } while (tableTypes.find(t => t.id === id));

        const newTableTypes = [...tableTypes, {
            id,
            name: 'New Table Type',

            width: 25,
            height: 25,

            seats: 0,

            rects: []
        }];
        setTableTypes(newTableTypes);
        updateSelection(id.toString());
    }

    function updateTableType(id, changed) {
        const newTableTypes = tableTypes.map(tableType => {
            if (tableType.id === id) {
                return {...tableType, ...changed};
            }
            return tableType;
        });

        setTableTypes(newTableTypes);
    }

    function deleteTableType(id) {
        if (window.confirm('Are you sure you want to delete this table type? This will break all tables associated with it')) {
            const newTableTypes = tableTypes.filter(tableType => tableType.id !== id);
            setTableTypes(newTableTypes);
            updateSelection('');
        }
    }

    function updateRectangle(index, changed) {
        const newRects = [...selectedTableType.rects];
        newRects[index] = {...newRects[index], ...changed};
        updateTableType(selectedTableType.id, {rects: newRects});
    }

    function newRectangle() {
        const newRects = [...selectedTableType.rects, {
            x: 50 - 25 / 2,
            y: 50 - 25 / 2,
            width: 25,
            height: 25,
            color: {r: 192, g: 168, b: 0, a: 255}
        }];

        updateTableType(selectionInt, {rects: newRects});
    }

    function getColorForPicker(color) {
        if(!color) {
            return {r: 255, g: 0, b: 255, a: 1};
        }

        return {
            ...color,
            a: color.a / 255.0
        };
    }

    function changeRectColor(id, color) {
        updateRectangle(id, {color: {...color, a: color.a * 255}});
    }

    function deleteRectangle(id) {
        if (window.confirm('Are you sure you want to delete this rectangle?')) {
            const newRects = [...selectedTableType.rects];
            newRects.splice(id, 1);

            setSelectedRect(null);
            updateTableType(selectedTableType.id, {rects: newRects});
        }
    }

    async function saveChanges() {
        setIsLoading(true);
        const response = await api.layout.putTableTypes(tableTypes);
        if (response.ok) {
            setError(null);
            setGeneration(generation + 1);
        } else {
            setError(await api.common.processError(response));
        }
        setIsLoading(false);
    }

    return (
        <Loader loading={isLoading}>
            <ErrorDisplay error={blockingError}>
                <ErrorDisplay error={error}/>
                <div>
                    <label>Table Type:</label>
                    <div className="input-group">
                        <select className="form-select" value={selection ?? ''}
                                onChange={e => updateSelection(e.currentTarget.value)}>
                            <option value={''}>Select a table type</option>
                            {tableTypes.map((tt, index) => (
                                <option key={index} value={tt.id}>{tt.name}</option>
                            ))}
                        </select>
                        <button className="btn btn-primary" onClick={newTableType}>New Table Type</button>

                        <button className="btn btn-success" onClick={saveChanges}>Save Changes</button>
                    </div>

                    {selection ? (
                        <>
                            <div className="form-group d-flex align-items-center gap-2 my-2">
                                <label>Name:</label>
                                <input type="text" className="form-control" value={selectedTableType?.name}
                                       onChange={e => updateTableType(selectionInt, {name: e.currentTarget.value})}/>

                                <label>Width:</label>
                                <input type="number" className="form-control" value={selectedTableType?.width}
                                       onChange={e => updateTableType(selectionInt, {width: parseFloat(e.currentTarget.value)})}/>

                                <label>Height:</label>
                                <input type="number" className="form-control" value={selectedTableType?.height}
                                       onChange={e => updateTableType(selectionInt, {height: parseFloat(e.currentTarget.value)})}/>

                                <label>Seats:</label>
                                <input type="number" className="form-control" value={selectedTableType?.seats}
                                       onChange={e => updateTableType(selectionInt, {seats: parseInt(e.currentTarget.value)})}/>
                            </div>
                            <div
                                className="d-flex flex-column flex-xl-row justify-content-center rounded-2 border border-primary">
                                <div className="ratio ratio-1x1 mx-auto w-75 mh-100" style={{
                                    '--bs-aspect-ratio': `${selectedTableType?.height / selectedTableType?.width * 100}%`,
                                    maxHeight: '100vh'
                                }}>
                                    <svg xmlns="http://www.w3.org/2000/svg" className="user-select-none" id="svg-root">
                                        <rect fill="#6c757d" x="0" y="0" width="100%" height="100%"/>
                                        {selectedTableType?.rects.map((rect, index) => (
                                            <TableRectangle key={index} rect={rect}
                                                            updateRect={newRect => updateRectangle(index, newRect)}
                                                            selected={selectedRect === index}
                                                            select={() => setSelectedRect(index)}/>
                                        ))}
                                    </svg>
                                </div>
                                <div className="d-flex flex-column flex-grow-1 gap-2 justify-content-between">
                                    <div className="d-flex flex-column gap-2">
                                        <div
                                            className="d-flex justify-content-center align-items-center fw-bold bg-dark text-light"
                                            style={{height: '40px'}}>Tool Menu
                                        </div>
                                        <div className="mx-2 d-flex flex-column gap-1">
                                            <button className="btn btn-outline-danger" onClick={() => deleteTableType(selectionInt)}>
                                                Delete Table Type
                                            </button>
                                            <button className="btn btn-outline-primary" onClick={newRectangle}>
                                                New Rectangle
                                            </button>
                                        </div>

                                        {selectedRect !== null && <>
                                            <center>Selected Rectangle:</center>
                                            <button className="btn btn-outline-info"
                                                    onClick={() => setChangeColorActive(true)}>Change Color
                                            </button>

                                            {changeColorActive && <div className="d-flex justify-content-center">
                                                <div className="position-fixed top-0 start-0 w-100 h-100"
                                                     onClick={() => setChangeColorActive(false)}></div>
                                                <SketchPicker className="position-absolute"
                                                              color={getColorForPicker(selectedRectObj?.color)}
                                                              onChange={e => changeRectColor(selectedRect, e.rgb)}/>
                                            </div>}
                                            <button className="btn btn-outline-danger"
                                                    onClick={() => deleteRectangle(selectedRect)}>Delete
                                            </button>
                                            <button className="btn btn-outline-success"
                                                    onClick={() => setSelectedRect(null)}>Deselect
                                            </button>
                                        </>}
                                    </div>
                                </div>
                            </div>
                        </>
                    ) : (
                        <div className="alert alert-warning">Please select a table type to edit</div>
                    )}
                </div>
            </ErrorDisplay>
        </Loader>
    );
}
