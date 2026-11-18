import { useState } from "react";
import type { SubmitEvent } from "react";
import { usersApi } from "../../api/usersApi";

import "./AdminStyling.css";

export default function Users() {
    const [name, setName] = useState("");
    const [status, setStatus] = useState("Available");

    const [saving, setSaving] = useState(false);
    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    async function handleSubmit(event: SubmitEvent) {
        event.preventDefault();

        if (!isFormValid || saving) return;

        setSaving(true);
        setMessage("");
        setError("");

        try {
            const user = await usersApi.create({
                name,
                status,
            });

            setMessage(`Created user "${user.name}"`);

            setName("");
            setStatus("Active");
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Could not create user."
            );
        } finally {
            setSaving(false);
        }
    }

    const isFormValid =
        name.trim().length > 0 &&
        status.trim().length > 0;

    return (
        <div className="dashboard">
            <div className="admin-page-heading">
                <div>
                    <h1>Users</h1>
                    <p>Create and manage users registered in the system.</p>
                </div>
            </div>

            <div className="dashboard-card admin-form-card">
                <h2>Create user</h2>

                <form className="admin-form" onSubmit={handleSubmit}>
                    <div className="admin-form-grid">
                        <div className="admin-field">
                            <label htmlFor="asset-name">Name</label>
                            <input
                                id="asset-name"
                                value={name}
                                onChange={(event) => setName(event.target.value)}
                                required
                            />
                        </div>

                        <div className="admin-field">
                            <label htmlFor="asset-status">Status</label>
                            <select
                                id="asset-status"
                                value={status}
                                onChange={(event) => setStatus(event.target.value)}
                            >
                                <option value="Active">Active</option>
                                <option value="Inactive">Inactive</option>
                            </select>
                        </div>
                    </div>

                    {error && (
                        <p className="admin-form-message error">{error}</p>
                    )}

                    {message && (
                        <p className="admin-form-message success">{message}</p>
                    )}

                    <div className="admin-form-actions">
                        <button
                            className="admin-primary-button"
                            type="submit"
                            disabled={saving || !isFormValid} 
                        >
                            {saving ? "Creating..." : "Create user"}
                        </button>    
                    </div>
                </form>
            </div>
        </div>
    )
}