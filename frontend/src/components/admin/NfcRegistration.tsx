import { useEffect, useState } from "react";

import {
    assetRegistrationApi,
    type AssetRegistration,
    type CreateAssetRegistration
} from "../../api/assetRegistrationApi";

import "../../pages/admin/AdminStyling.css";

interface Props {
    asset: Omit<CreateAssetRegistration, "readerId">;
    isFormValid: boolean;
    onComplete?: () => void;
}

export default function NfcRegistration({
    asset,
    isFormValid,
    onComplete,
}: Props) {
    const [registration, setRegistration] = 
        useState<AssetRegistration | null>(null);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const registrationId = registration?.id;
    const registrationStatus =
        registration?.registrationStatus;
    
    useEffect(() => {
        if (!registrationId) return;

        if (registrationStatus == "Completed" || 
            registrationStatus == "Cancelled"
        ) {
            return;
        }

        let active = true;
        let polling = false;

        async function refresh() {
            if (polling || !registrationId) return;
            polling = true;

            try {
                const result =
                    await assetRegistrationApi.get(registrationId);

                if (active) {
                    setRegistration(result);
                }
            } catch (err) {
                if (active) {
                    setError(
                        err instanceof Error
                            ? err.message
                            : "Unable to check registration."
                    );
                }
            } finally {
                polling = false;
            }
        }

        void refresh();

        const interval = window.setInterval(() => {
            void refresh();
        }, 1000);

        return () => {
            active = false;
            window.clearInterval(interval);
        };
    }, [registrationId, registrationStatus]);

    useEffect(() => {
        if (registrationStatus === "Completed") {
            onComplete?.();
        }
    }, [registrationStatus, onComplete]);

    async function startRegistration() {
        setLoading(true);
        setError("");

        try {
            const result = await assetRegistrationApi.start({
                ...asset,
                readerId: "reader-01",
            });

            setRegistration(result);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Could not start registration."
            );
        } finally {
            setLoading(false);
        }
    }

    async function writeTag() {
        if (!registration) return;

        setLoading(true);
        setError("");

        try {
            const result = await assetRegistrationApi.write(
                registration.id
            );

            setRegistration(result);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Could not start writing."
            );
        } finally {
            setLoading(false);
        }
    }

    async function cancelRegistration() {
        if (!registration) return;

        setLoading(true);
        setError("");

        try {
            await assetRegistrationApi.cancel(registration.id);
            setRegistration(null);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Could not cancel registration."
            );
        } finally {
            setLoading(false);
        }
    }

    const status = registration?.registrationStatus;

    return (
        <div className="nfc-registration">
            <div className="nfc-registration-header">
                <h2>NFC Registration</h2>
                <p>Register a physical NFC tag to this asset.</p>
            </div>

            {!registration && (
                <div className="nfc-registration-content">
                    <p>
                        Enter the asset details, then start the NFC
                        registration process.
                    </p>

                    <button
                        type="button"
                        className="admin-primary-button"
                        disabled={!isFormValid || loading}
                        onClick={startRegistration}
                        >
                            {loading
                                ? "Starting..."
                                : "Start NFC Registration"}
                        </button>
                </div>
            )}

            {status === "WaitingForTag" && (
                <div className="nfc-registration-content">
                    <div className="nfc-status waiting">
                        <strong>Waiting for NFC tag</strong>
                        <p>Place an NFC tag on the reader.</p>
                    </div>
                </div>
            )}

            {status === "TagAvailable" && (
                <div className="nfc-registration-content">
                    <div className="nfc-status success">
                        <strong>Tag available</strong>

                        <p>
                            UID: <code>{registration?.uid}</code>
                        </p>

                        <p>
                            This tag is available for registration.
                        </p>
                    </div>

                    <button
                        type="button"
                        className="admin-primary-button"
                        disabled={loading}
                        onClick={writeTag}
                        >
                            {loading
                                ? "Preparing"
                                : "Register & Write tag"}
                        </button>
                </div>
            )}

            {status === "WaitingForWrite" && (
                <div className="nfc-registration-content">
                    <div className="nfc-status waiting">
                        <strong>Ready to write</strong>

                        <p>
                            Place tag <code>{registration?.uid}</code>
                            {" "}on the reader.
                        </p>

                        <p>
                            Writing will begin automatically when the
                            correct tag is detected.
                        </p>
                    </div>
                </div>
            )}

            {status === "Writing" && (
                <div className="nfc-registration-content">
                    <div className="nfc-status waiting">
                        <strong>Writing NFC tag...</strong>
                        
                        <p>
                            Keep the tag on the reader until verification
                            is complete.
                        </p>
                    </div>
                </div>
            )}

            {status === "Completed" && (
                <div className="nfc-registration-content">
                    <div className="nfc-status success">
                        <strong>Registration successful</strong>

                        <p>
                            The asset and NFC tag have been registered.
                        </p>

                        <p>
                            UID: <code>{registration?.uid}</code>
                        </p>
                    </div>
                </div>
            )}

            {status === "Failed" && (
                <div className="nfc-registration-content">
                    <div className="nfc-status error">
                        <strong>Registration failed</strong>

                        <p>
                            {registration?.error ??
                                "The NFC registration could not be completed."}
                        </p>
                    </div>
                </div>
            )}

            {registration && 
                status !== "Completed" && 
                status !== "Cancelled" && (
                    <button
                        type="button"
                        className="admin-secondary-button"
                        disabled={loading}
                        onClick={cancelRegistration}
                    >
                        Cancel Registration
                    </button>
            )}
            {error && (
                <p className="admin-form-message error">
                    {error}
                </p>
            )}
        </div>
    )
}