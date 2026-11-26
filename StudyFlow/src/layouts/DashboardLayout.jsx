
import { Outlet } from "react-router-dom";
import Sidebar from "../components/Sidebar";

export default function DashboardLayout() {
  return (
    <div className="container-fluid">
      <div className="row min-vh-100">
        <Sidebar />

        <main
          className="col-12 col-md-9 col-xl-10 p-4"
          style={{ backgroundColor: "#F8FAFC" }}
        >
          <Outlet />
        </main>
      </div>
    </div>
  );
}

