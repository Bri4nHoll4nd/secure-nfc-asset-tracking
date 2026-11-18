import { useState } from "react";
import type { SubmitEvent } from "react";
import { assetsApi } from "../../api/assetsApi";

import "./AdminStyling.css";
import NfcRegistration from "../../components/admin/NfcRegistration";

export default function Assets() {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [status, setStatus] = useState("Available");
    const [maintenanceStatus, setMaintenanceStatus] = useState("None");

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
            const asset = await assetsApi.create({
                name,
                description,
                status,
                maintenanceStatus,
                userId: null
            });

            setMessage(`Created asset "${asset.name}"`);

            setName("");
            setDescription("");
            setStatus("Available");
            setMaintenanceStatus("None");
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Could not create asset."
            );
        } finally {
            setSaving(false);
        }
    }

    const isFormValid =
        name.trim().length > 0 &&
        description.trim().length > 0 &&
        status.trim().length > 0 &&
        maintenanceStatus.trim().length > 0;

    return (
        <div className="dashboard">
            <div className="admin-page-heading">
                <div>
                    <h1>Assets</h1>
                    <p>Create and manage equipment registered in the system.</p>
                </div>
            </div>

            <div className="dashboard-card admin-form-card">
                <h2>Create asset</h2>

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
                                <option value="Available">Available</option>
                                <option value="CheckedOut">CheckedOut</option>
                                <option value="Unavailable">Unavailable</option>
                            </select>
                        </div>

                        <div className="admin-field admin-field-full">
                            <label htmlFor="asset-description">
                                Description
                            </label>

                            <textarea 
                                id="asset-description"
                                value={description}
                                onChange={(event) =>
                                    setDescription(event.target.value)
                                }
                                rows={4}
                                required
                            />
                        </div>

                        <div className="admin-field">
                            <label htmlFor="maintenance-status">
                                Maintenance
                            </label>

                            <select
                                id="maintenance-status"
                                value={maintenanceStatus}
                                onChange={(event) => setMaintenanceStatus(event.target.value)}
                            >
                                <option value="None">None</option>
                                <option value="Required">Required</option>
                                <option value="InProgress">In Progress</option>
                            </select>
                        </div>
                    </div>

                    {error && (
                        <p className="admin-form-message error">{error}</p>
                    )}

                    {message && (
                        <p className="admin-form-message success">{message}</p>
                    )}

                    <NfcRegistration
                        asset={{
                            name: name.trim(),
                            description: description.trim(),
                            status,
                            maintenanceStatus,
                        }}
                        isFormValid={isFormValid}
                    />
                </form>
            </div>
        </div>
    )
}