import {NavLink, Route, Routes} from "react-router-dom";
import {AreaLayoutPage, AreasLayoutPage, TableTypesPage} from "./pages";
import {useEffect, useState} from "react";
import {api} from "./services";
import {ErrorDisplay, Loader} from "./components";

function App() {
    const [isLoading, setIsLoading] = useState(true);
    const [areas, setAreas] = useState([]);
    const [blockingError, setBlockingError] = useState(null);
    const [generation, setGeneration] = useState(0);

    useEffect(() => {
        async function getAreas() {
            setIsLoading(true);
            const response = await api.layout.getAreas();
            if (response.isInternalError || !response.ok) {
                setBlockingError(`${!response.isInternalError ? response.status : ''} ${response.statusText}`);
            } else {
                setAreas(await response.json());
                setBlockingError(null);
            }
            setIsLoading(false);
        }

        getAreas().then();
    }, [generation]);

    return (
        <div>
            <Loader loading={isLoading}>
                <ErrorDisplay error={blockingError}>
                    <ul className="nav nav-tabs">
                        <li className="nav-item">
                            <NavLink to="/" className="nav-link">Areas</NavLink>
                        </li>

                        <li className="nav-item">
                            <NavLink to="/table-types" className="nav-link">Table Types</NavLink>
                        </li>
                        <li className="nav-item mx-2 border-start"></li>
                        {areas.map(area => (
                            <li key={area.id} className="nav-item">
                                <NavLink to={`/area-layout/${area.id}`} className="nav-link">{area.name} Area Layout</NavLink>
                            </li>
                        ))}
                    </ul>
                    <Routes>
                        <Route index element={<AreasLayoutPage incrementGeneration={() => setGeneration(generation + 1)}/>}/>
                        <Route path="table-types" element={<TableTypesPage/>}/>
                        <Route path="area-layout/:areaId" element={<AreaLayoutPage/>}/>
                    </Routes>
                </ErrorDisplay>
            </Loader>
        </div>
    );
}

export default App;
