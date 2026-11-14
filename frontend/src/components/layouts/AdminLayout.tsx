import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import Header from "./Header";
import "./AdminLayout.css";

export default function AdminLayout() {
    return (
        <div className="admin-page">
            <div className="admin-layout">
                <Sidebar />

                <div className="admin-container">
                    <Header />

                    <main className="admin-content">
                        <Outlet />
                    </main>
                </div>
            </div>
        </div>
    );
}