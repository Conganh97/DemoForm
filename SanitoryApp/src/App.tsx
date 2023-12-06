import './App.css'
import Login from "@/pages/login.tsx";
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import Register from "@/pages/register.tsx";
import Home from "@/pages/home.tsx";

const router = createBrowserRouter([
    {
        path: "/",
        Component: Login
    },
    {
        path: "/register",
        Component: Register,
    },
    {
        path: "/home",
        Component: Home,
    }
]);


export default function App() {
    return <RouterProvider router={router} fallbackElement={<Fallback/>}/>;
}

export function Fallback() {
    return <p>404 Not Found</p>;
}