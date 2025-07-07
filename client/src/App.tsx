import './App.css'
import {Route, Routes} from "react-router-dom";
import {Home} from "./assets/components/Home/Home.tsx";
import Login from "./assets/components/Login/Login.tsx";
import Register from "./assets/components/Register/Register.tsx";
import AddGroup from "./assets/components/ToDo/AddGroup.tsx";
import AddTask from "./assets/components/ToDo/AddTask.tsx";

// App.tsx komponent jest głównym komponentem aplikacji,
function App() {

    return (
        <>
            {/* zarzadanie sciezkami */}
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/add-group" element={<AddGroup/>}/>
                <Route path="/add-task/:groupId" element={<AddTask />} />
                <Route path="/login" element={<Login/>}/>
                <Route path="/register" element={<Register/>}/>
            </Routes>
        </>
    );
}

export default App
