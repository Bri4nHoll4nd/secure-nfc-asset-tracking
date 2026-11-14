import "./Dashboard.css";

const stats = [
    { title: "Assets" },
    { title: "Users" },
    { title: "Tags" },
    { title: "Activity" },
];

export default function Dashboard() {
    return (
        <div className="dashboard">
            <h1>Dashboard</h1>

            <div className="dashboard-grid">
                {stats.map((stat) => (
                    <div className="dashboard-card" key={stat.title}>
                        <span>{stat.title}</span>
                        <div className="dashboard-placeholder" />
                    </div>
                ))}
            </div>

            <section className="dashboard-main-card">
                <h2>Main page content</h2>
                <p>
                    This section changes depending on the selected page.
                </p>
            </section>
        </div>
    );
}