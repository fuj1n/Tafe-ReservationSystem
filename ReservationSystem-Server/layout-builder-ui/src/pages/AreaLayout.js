import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {api} from "../services";
import {ErrorDisplay, Loader} from "../components";
import {SvgDraw} from "../components";
import {SketchPicker} from "react-color";

function TableType({tableRect, rotation, tableType, label, extra}) {
    let accX = 0, accY = 0;
    for (const rect of tableType.rects) {
        const absolute = SvgDraw.transform(tableRect, rect, true);
        accX += absolute.x + absolute.width / 2;
        accY += absolute.y + absolute.height / 2;
    }

    accX /= tableType.rects.length;
    accY /= tableType.rects.length;

    const rotate = {
        style: {transform: `translate(${accX}%, ${accY}%) rotate(${rotation ?? 0}deg) translate(${-accX}%, ${-accY}%)`}
    }

    return (
        <g {...extra}>
            {tableType.rects.map((rect, index) => (
                <SvgDraw.Rect rect={SvgDraw.transform(tableRect, rect, true)} key={index} extra={rotate}/>
            ))}
            {label && <SvgDraw.CenterLabel
                within={{x: accX, y: accY, width: 0, height: 0}}
                text={label} color={{r: 0, g: 0, b: 0, a: 255}}/>}
        </g>
    );
}

function Table({area, table, tableType, move, select, selected}) {
    const relRect = {x: table.x, y: table.y, width: tableType.width, height: tableType.height};
    const absoluteRect = SvgDraw.transform(area.areaRect, relRect, false);

    absoluteRect.width *= area.scaleX;
    absoluteRect.height *= area.scaleY;

    function onMove(rect) {
        const asRelative = SvgDraw.invTransform(area.areaRect, rect, false);

        move(Math.max(Math.min(asRelative.x, 100 - absoluteRect.width), 0),
            Math.max(Math.min(asRelative.y, 100 - absoluteRect.height), 0));
    }

    const selectEffect = selected ? {
        className: 'marchingAnts',
        style: {stroke: '#000', strokeWidth: '2px', strokeDasharray: '5,5'}
    } : {};

    const extra = {...selectEffect, fill: 'transparent', style: {...selectEffect.style, cursor: 'pointer'}, onClick: select};

    return (
        <g>
            <TableType tableRect={absoluteRect} rotation={table.rotation} tableType={tableType} label={table.name}/>
            <SvgDraw.Rect rect={absoluteRect} extra={extra}/>
            <SvgDraw.MoveResizeHandle rect={absoluteRect} updateRect={onMove} canMove
                                      moveOffset={{x: -absoluteRect.width / 2, y: -absoluteRect.height / 2}}/>
        </g>
    );
}

function TableTypePicker({name, tableTypes, pick}) {
    const [hover, setHover] = useState(null);

    return (
        <div className="w-75 ratio ratio-1x1 d-flex">
            <div>
                <div>Please select a table type for the table {name}</div>
                <div className="d-flex justify-content-start align-items-center gap-2 flex-wrap">
                    {tableTypes.map(tableType => (
                        <div key={tableType.id} className="ratio" style={{
                            '--bs-aspect-ratio': `${tableType?.height / tableType?.width * 100}%`,
                            width: `${tableType?.width * 2}%`,
                        }}>
                            <svg xmlns="http://www.w3.org/2000/svg" className="user-select-none"
                                 onClick={() => pick(tableType.id)} onMouseEnter={() => setHover(tableType.id)}
                                 onMouseLeave={() => setHover(null)} style={{cursor: 'hand'}}>
                                <TableType tableRect={{x: 25, y: 25, width: 50, height: 50}} tableType={tableType}
                                           label={tableType.name}/>
                                <rect fill={ hover === tableType.id ? '#33333355' : 'transparent'} x="0" y="0" width="100%" height="100%"/>
                            </svg>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default function AreaLayoutPage() {
    const {areaId} = useParams();

    const [area, setArea] = useState(null);
    const [tableTypes, setTableTypes] = useState([]);

    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [blockingError, setBlockingError] = useState(null);
    const [generation, setGeneration] = useState(0);

    const [selection, setSelection] = useState(null);
    const selectedTable = selection ? area?.tables?.find(table => table.id === selection) : null;

    const untypedTable = area?.tables.find(t => !tableTypes.find(tt => tt.id === t.tableTypeId));

    function updateTable(id, changed) {
        const newTables = area.tables.map(table => {
            if (table.id === id) {
                return {...table, ...changed};
            }
            return table;
        });

        setArea({...area, tables: newTables});
    }

    useEffect(() => {
        async function getArea() {
            setIsLoading(true);
            const response = await api.layout.getArea(areaId);
            if (response.isInternalError || !response.ok) {
                setBlockingError(`${!response.isInternalError ? response.status : ''} ${response.statusText}`);
            } else {
                const area = await response.json();

                // Normalize the area rectangle
                let rect = area.areaRect;

                if (rect.width > rect.height) {
                    rect = {
                        width: 100,
                        height: 100 * rect.height / rect.width
                    };
                } else {
                    rect = {
                        width: 100 * rect.width / rect.height,
                        height: 100
                    };
                }

                rect.x = 0;
                rect.y = 0;
                rect.color = area.areaRect.color;

                area.scaleX = rect.width / area.areaRect.width;
                area.scaleY = rect.height / area.areaRect.height;
                area.areaRect = rect;

                setArea(area);
                setBlockingError(null);
            }
            setIsLoading(false);
        }

        async function getTableTypes() {
            const response = await api.layout.getTableTypes();
            if (response.isInternalError || !response.ok) {
                setBlockingError(`${!response.isInternalError ? response.status : ''} ${response.statusText}`);
            } else {
                setTableTypes(await response.json());
                setBlockingError(null);
            }
        }

        getArea().then();
        getTableTypes().then();
    }, [generation, areaId]);

    function addTable() {
        let id;

        do {
            id = area.tables.length + Math.floor(Math.random() * 1000000);
            // eslint-disable-next-line no-loop-func -- not a problem here
        } while (area.tables.find(a => a.id === id));

        const newTable = {
            id,
            name: 'New Table',
            tableTypeId: 0,
            x: 50,
            y: 50,
            rotation: 0
        }

        setArea({...area, tables: [...area.tables, newTable]});
        setSelection(id);
    }

    function deleteTable(id) {
        if(window.confirm('Are you sure you want to delete this table?')) {
            setArea({...area, tables: area.tables.filter(t => t.id !== id)});
            setSelection(null);
        }
    }

    async function saveChanges() {
        setIsLoading(true);
        const response = await api.layout.putArea(area);
        if (response.isInternalError || !response.ok) {
            setBlockingError(await api.common.processError(response));
        }
        // setGeneration(generation + 1);
        setIsLoading(false);
    }

    return (
        <Loader loading={isLoading}>
            <ErrorDisplay error={blockingError}>
                <ErrorDisplay error={error}/>
                <div
                    className="d-flex flex-column flex-xl-row justify-content-center rounded-2 border border-primary">
                    {untypedTable ? (
                            <TableTypePicker name={untypedTable.name} tableTypes={tableTypes}
                                             pick={typeId => updateTable(untypedTable.id, {tableTypeId: typeId})}/>
                        ) :
                        (<div className="w-75 ratio ratio-1x1">
                            <svg xmlns="http://www.w3.org/2000/svg" className="user-select-none" id="svg-root">
                                <rect fill="#6c757d" x="0" y="0" width="100%" height="100%"/>
                                <SvgDraw.Rect rect={area?.areaRect}/>

                                {area?.tables.map(t => {
                                    const tableType = tableTypes.find(tt => tt.id === t.tableTypeId);
                                    return <Table key={t.id} area={area} table={t} tableType={tableType}
                                                  move={(x, y) => updateTable(t.id, {x, y})}
                                                  selected={t.id === selection}
                                                    select={() => setSelection(t.id)}/>;
                                })}
                            </svg>
                        </div>)}

                    <div className="d-flex flex-column flex-grow-1 gap-2 justify-content-between">
                        <div className="d-flex flex-column gap-2">
                            <div className="d-flex justify-content-center align-items-center fw-bold bg-dark text-light"
                                 style={{height: '40px'}}>Tool Menu
                            </div>
                            <div className="mx-2 d-flex flex-column gap-1">
                                <button className="btn btn-outline-primary" onClick={addTable}>Add Table</button>
                                {selection && <>
                                    <center>Selected Table:</center>
                                    <div className="form-group">
                                        <label>Name:</label>
                                        <input type="text" className="form-control" value={selectedTable?.name}
                                               onChange={e => updateTable(selection, {name: e.currentTarget.value})}/>
                                    </div>
                                    <div className="form-group">
                                        <label>Rotation (degrees):</label>
                                        <input type="number" className="form-control" value={selectedTable?.rotation}
                                                  onChange={e => updateTable(selection, {rotation: parseInt(e.currentTarget.value)})}/>
                                    </div>
                                    <button className="btn btn-outline-danger" onClick={() => deleteTable(selection)}>Delete
                                    </button>
                                    <button className="btn btn-outline-success" onClick={() => setSelection(null)}>
                                        Deselect
                                    </button>
                                </>}
                            </div>
                        </div>
                        <div className="d-flex flex-column">
                            <ErrorDisplay error={error}/>
                            <button className="btn btn-outline-success rounded-0" onClick={saveChanges}>
                                Save Changes
                            </button>
                        </div>
                    </div>
                </div>
            </ErrorDisplay>
        </Loader>
    );
}
