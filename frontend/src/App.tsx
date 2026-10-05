import { useEffect, useState } from "react";

import {
    getApiStatus,
    getLatestScan,
    type ScanResult
} from "./api/apiClient";

import {
    createScanHubConnection
} from "./signalR/scanHub";

import './App.css';

function App() {
    const [apiStatus, setApiStatus] = useState<string>("Checking...");

    const [signalRStatus, setSignalRStatus] = useState<string>("Connecting...");

    const [latestScan, setLatestScan] = useState<ScanResult | null>(null);

    const [scanError, setScanError] = useState<string | null>(null);

    useEffect(() => {
        async function checkApi() {
            try {
                const result = await getApiStatus();
                setApiStatus(result);
            }
            catch {
                setApiStatus("Disconnected");
            }
        }

        checkApi();
    }, []);

    useEffect(() => {
        const connection = createScanHubConnection();

        let disposed = false;

        connection.on(
            "ScanReceived",
            (scan: ScanResult) => {
                if (disposed) {
                    return;
                }

                setLatestScan(scan);
                setScanError(null);
            }
        );

        connection.onreconnecting(() => {
            if (!disposed) {
                setSignalRStatus("Reconnecting...");
            }
        });

        connection.onreconnected(() => {
            if (!disposed) {
                setSignalRStatus("Connected");
            }
        });

        connection.onclose(() => {
            if (!disposed) {
                setSignalRStatus("Disconnected");
            }
        });

        async function start() {
            try {
                await connection.start();

                if (disposed) {
                    return;
                }

                setSignalRStatus("Connected");

                const scan = await getLatestScan();

                if (!disposed) {
                    setLatestScan(scan);
                }
            }
            catch (error) {
                if (disposed) {
                    return;
                }

                const message =
                    error instanceof Error
                        ? error.message
                        : "Unknown error";

                setSignalRStatus("Disconnected");
                setScanError(message);
            }
        }

        start();

        return () => {
            disposed = true;
            connection.stop();
        };
    }, []);

    return (
        <main className="dashboard">
            <header>
                <h1>Secure NFC Asset Tracking</h1>

                <p className="subtitle">
                    NFC reader development dashboard
                </p>
            </header>

            <section className="card">
                <h2>System status</h2>

                <div className="status-row">
                    <span>API</span>
                    <strong>{apiStatus}</strong>
                </div>

                <div className="status-row">
                    <span>Live updates</span>
                    <strong>{signalRStatus}</strong>
                </div>
            </section>

            <section className="card scan-card">
                <h2>Latest NFC scan</h2>

                {scanError && (
                    <p className="error">
                        {scanError}
                    </p>
                )}

                {latestScan && (
                    <div className="scan-data">
                        <div>
                            <span>UID</span>
                            <strong>
                                {latestScan.tagId}
                            </strong>
                        </div>

                        <div>
                            <span>Source</span>
                            <strong>
                                {latestScan.source}
                            </strong>
                        </div>

                        <div>
                            <span>Registered</span>
                            <strong>
                                {latestScan.registered
                                    ? "Yes"
                                    : "No"}
                            </strong>
                        </div>

                        <div>
                            <span>Type</span>
                            <strong>
                                {latestScan.entityType}
                            </strong>
                        </div>

                        {latestScan.name && (
                            <div>
                                <span>Name</span>
                                <strong>
                                    {latestScan.name}
                                </strong>
                            </div>
                        )}

                        {latestScan.status && (
                            <div>
                                <span>Status</span>
                                <strong>
                                    {latestScan.status}
                                </strong>
                            </div>
                        )}

                        <div>
                            <span>Scanned</span>
                            <strong>
                                {new Date(
                                    latestScan.scannedAtUtc
                                ).toLocaleString()}
                            </strong>
                        </div>
                    </div>
                )}
            </section>
        </main>
    );
}

export default App
