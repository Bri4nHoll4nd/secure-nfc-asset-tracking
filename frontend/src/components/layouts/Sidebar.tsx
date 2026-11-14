import { NavLink } from "react-router-dom";
import {
    ScanLine,
    LayoutDashboard,
    Package,
    Users,
    CreditCard,
    ScrollText,
    Settings,
    LogOut,
} from "lucide-react";
import "./Sidebar.css";

const navigation = [
    { name: "Dashboard", path: "/admin", icon: LayoutDashboard, end: true},
    { name: "Assets", path: "/admin/assets", icon: Package},
    { name: "Users", path: "/admin/users", icon: Users},
    { name: "NFC Tags", path: "/admin/tags", icon: CreditCard},
    { name: "Logs", path: "/admin/logs", icon: ScrollText},
    { name: "Settings", path: "/admin/settings", icon: Settings},
];

export default function Sidebar() {
    return (
        <aside className="sidebar">
            <div className="sidebar-logo">
                <ScanLine size={20} />
                <span>NFC Manager</span>
            </div>

            <nav className="sidebar-nav">
                {navigation.map((item) => {
                    const Icon = item.icon;

                    return (
                        <NavLink key={item.path} to={item.path} end={item.end} className={({ isActive }) => `sidebar-link ${isActive ? "active" : ""}`}>
                            <Icon size={17} />
                            <span>{item.name}</span>
                        </NavLink>
                    );
                })}
            </nav>

            <div className="sidebar-footer">
                <button className="sidebar-logout" type="button">
                    <LogOut size={17} />
                    <span>Logout</span>
                </button>
            </div>
        </aside>
    );
}