import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./App.css";

import AdminLayout from "./components/layouts/AdminLayout";
import Dashboard from "./pages/admin/Dashboard";
import Assets from "./pages/admin/Assets";
import Users from "./pages/admin/Users";
import Tags from "./pages/admin/Tags";
import Logs from "./pages/admin/Logs";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/admin" element={<AdminLayout />}>
                    <Route index element={<Dashboard />} />
                    <Route path="assets" element={<Assets />} />
                    <Route path="users" element={<Users />} />
                    <Route path="tags" element={<Tags />} />
                    <Route path="logs" element={<Logs />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}