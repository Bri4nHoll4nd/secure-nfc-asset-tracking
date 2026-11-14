import { CircleUserIcon } from "lucide-react";
import "./Header.css";

export default function Header() {
    // Temp until login is implemented
    const user = {
        name: "Ola"
    }

    return (
        <header className="admin-header">
            <span>Admin Panel</span>

            <div className="admin-profile">
                <span className="admin-profile-name">
                    {user.name}
                </span>

                <CircleUserIcon size={24} />
            </div>
        </header>
    );
}