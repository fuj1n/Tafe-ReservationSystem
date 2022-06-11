import {NavLink, Route, Routes} from "react-router-dom";
import {AreasLayoutPage, TableTypesPage} from "./pages";

function App() {
    return (
        <div>
            <ul className="nav nav-tabs">
                <li className="nav-item">
                    <NavLink to="/" className="nav-link">Areas</NavLink>
                </li>

                <li className="nav-item">
                    <NavLink to="/table-types" className="nav-link">Table Types</NavLink>
                </li>
                <li className="nav-item mx-2 border-start"></li>
                <li className="nav-item">
                    <NavLink to="/brazil" className="nav-link">Example Area</NavLink>
                </li>
            </ul>
            <Routes>
                <Route index element={<AreasLayoutPage/>}/>
                <Route path="table-types" element={<TableTypesPage/>}/>
            </Routes>
        </div>
    );
}

export default App;
