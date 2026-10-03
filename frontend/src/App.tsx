import { useEffect, useState } from "react";

import {
    getApiStatus,
    getLatestScan,
    type ScanResult
} from "./api/apiClient"

import './App.css'

function App() {
    const [apiStatus, setApiStatus] = useState<string>("Checking...");

    const [latestScan, setLatestScan] = useState<ScanResult | null>(null);

    const [scanError, setScanError] = useState<string | null>(null);

    useEffect(() => {
        async function checkApi() {
            try {
                const result = await getApiStatus();
                setApiStatus(result);
            } catch {
                setApiStatus("Disconnected");
            }
        }

        checkApi();
    }, []);

    useEffect(() => {
        async function updateLatestScan() {
            try {
                const scan = await getLatestScan();

                setLatestScan(scan);
                setScanError(null);
            } catch (error) {
                const message =
                    error instanceof Error
                        ? error.message
                        : "Unknown error";

                setScanError(message);
            }
        }

        updateLatestScan();

        const interval = window.setInterval(
            updateLatestScan,
            1000
        );

        return () => {
            window.clearInterval(interval);
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
                    <strong>
                        {apiStatus}
                    </strong>
                </div>
            </section>

            <section className="card scan-card">
                <h2>Latest NFC scan</h2>
                
                {scanError && (
                    <p className="error">
                        {scanError}
                    </p>
                )}

                {!latestScan && !scanError && (
                    <p className="waiting">
                        Waiting for NFC tag...
                    </p>
                )}

                {latestScan && (
                    <div className="scan-data">
                        <div>
                            <span>UID</span>
                            <strong>
                                {latestScan.tagUid}
                            </strong>
                        </div>

                        <div>
                            <span>Source</span>
                            <strong>
                                {latestScan.source}
                            </strong>
                        </div>

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
