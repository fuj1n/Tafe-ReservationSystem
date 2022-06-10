import {Route, Routes} from "react-router-dom";
import {AreasLayoutPage} from "./pages";

function App() {
    return (
        <div>
            <Routes>
                <Route index element={<AreasLayoutPage/>}/>
            </Routes>
        </div>
    );
}

export default App;
