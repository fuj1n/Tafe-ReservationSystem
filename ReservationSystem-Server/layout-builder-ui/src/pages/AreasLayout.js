import {useEffect, useState} from "react";
import {ErrorDisplay, Loader, SvgDraw} from "../components";
import {api} from "../services";
import {SketchPicker} from "react-color";

function Area({area, selected, select, updateArea}) {
    const selectEffect = selected ? {
        className: 'marchingAnts',
        style: {stroke: '#000', strokeWidth: '2px', strokeDasharray: '5,5'}
    } : {};

    const extra = {...selectEffect, style: {...selectEffect.style, cursor: 'pointer'}};

    return (
        <g onClick={select}>
            <SvgDraw.Rect rect={area.rect} extra={extra}/>
            <SvgDraw.Label x={area.rect.x + 1.25} y={area.rect.y + 1.25} text={area.name}
                           color={SvgDraw.darkenColor(area.rect.color, 2)}/>
            <SvgDraw.MoveResizeHandle rect={area.rect} updateRect={newRect => updateArea(area.id, {rect: newRect})}
                canMove canResize/>
        </g>
    );
}

export default function AreasLayoutPage() {
    const [isLoading, setIsLoading] = useState(true);
    const [areas, setAreas] = useState([]);
    const [error, setError] = useState(null);
    const [blockingError, setBlockingError] = useState(null);
    const [generation, setGeneration] = useState(0);

    const [selection, setSelection] = useState(null);
    const selectedArea = selection ? areas.find(a => a.id === selection) : null;

    const [changeColorActive, setChangeColorActive] = useState(false);

    useEffect(() => {
        async function getAreaLayout() {
            setIsLoading(true);
            const response = await api.layout.getAreaLayout();
            if (response.isInternalError || !response.ok) {
                setBlockingError(`${!response.isInternalError ? response.status : ''} ${response.statusText}`);
            } else {
                setAreas(await response.json());
                setBlockingError(null);
            }
            setSelection(null);
            setIsLoading(false);
        }

        getAreaLayout().then();
    }, [generation]);

    function updateArea(id, changed) {
        const newAreas = areas.map(area => {
            if (area.id === id) {
                return {...area, ...changed};
            }
            return area;
        });
        setAreas(newAreas);
    }

    function getColorForPicker(color) {
        return {
            ...color,
            a: color.a / 255.0
        };
    }

    function changeColor(id, color) {
        const newAreas = areas.map(area => {
            if (area.id === id) {
                return {...area, rect: {...area.rect, color: {...color, a: color.a * 255}}};
            }
            return area;
        });
        setAreas(newAreas);
    }

    function deleteArea(id) {
        if (window.confirm('Are you sure you want to delete this area? This will also delete all tables associated with it')) {
            const newAreas = areas.filter(area => area.id !== id);
            setAreas(newAreas);
        }
    }

    function createArea() {
        let id;

        do {
            id = areas.length + Math.floor(Math.random() * 1000000);
            // eslint-disable-next-line no-loop-func -- not a problem here
        } while (areas.find(a => a.id === id));

        const newAreas = [...areas, {
            id,
            name: 'New Area',
            rect: {
                x: 50 - 25 / 2,
                y: 50 - 25 / 2,
                width: 25,
                height: 25,
                color: {r: 192, g: 168, b: 0, a: 255}
            }
        }];
        setAreas(newAreas);
        setSelection(id);
    }

    async function saveChanges() {
        setIsLoading(true);
        const response = await api.layout.putAreaLayout(areas);
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
                <div className="d-flex flex-column flex-xl-row justify-content-center rounded-2 border border-primary">
                    <div className="w-75 ratio ratio-1x1">
                        <svg xmlns="http://www.w3.org/2000/svg" className="user-select-none" id="svg-root">
                            <rect fill="#6c757d" x="0" y="0" width="100%" height="100%"/>
                            {areas.map(a => {
                                return <Area area={a} key={a.id} updateArea={updateArea}
                                             select={() => setSelection(a.id)} selected={selection === a.id}/>;
                            })}
                        </svg>
                    </div>
                    <div className="d-flex flex-column flex-grow-1 gap-2 justify-content-between">
                        <div className="d-flex flex-column gap-2">
                            <div className="d-flex justify-content-center align-items-center fw-bold bg-dark text-light"
                                 style={{height: '40px'}}>Tool Menu
                            </div>
                            <div className="mx-2 d-flex flex-column gap-1">
                                <button className="btn btn-outline-primary" onClick={createArea}>New Area</button>
                                {selection && <>
                                    <center>Selected Area:</center>
                                    <div className="form-group">
                                        <label>Name:</label>
                                        <input type="text" className="form-control" value={selectedArea?.name}
                                               onChange={e => updateArea(selection, {name: e.currentTarget.value})}/>
                                    </div>
                                    <button className="btn btn-outline-info"
                                            onClick={() => setChangeColorActive(true)}>Change Color
                                    </button>

                                    {changeColorActive && <div className="d-flex justify-content-center">
                                        <div className="position-fixed top-0 start-0 w-100 h-100"
                                             onClick={() => setChangeColorActive(false)}></div>
                                        <SketchPicker className="position-absolute"
                                                      color={getColorForPicker(selectedArea?.rect.color)}
                                                      onChange={e => changeColor(selection, e.rgb)}/>
                                    </div>}
                                    <button className="btn btn-outline-danger"
                                            onClick={() => deleteArea(selection)}>Delete
                                    </button>
                                    <button className="btn btn-outline-success"
                                            onClick={() => setSelection(null)}>Deselect
                                    </button>
                                </>}
                            </div>
                        </div>
                        <div className="d-flex flex-column">
                            <ErrorDisplay error={error}/>
                            <button className="btn btn-outline-success rounded-0" onClick={saveChanges}>Save Changes
                            </button>
                        </div>
                    </div>
                </div>
            </ErrorDisplay>
        </Loader>
    );
}
