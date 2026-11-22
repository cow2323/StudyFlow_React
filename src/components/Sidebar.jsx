
import { useState } from "react";
import { NavLink } from "react-router-dom";

export default function Sidebar() {
  const [menuOpen, setMenuOpen] = useState(false);

  const navigation = (
    <nav className="nav flex-column gap-2" aria-label="Main navigation">
      <NavLink className="nav-link text-white py-2" to="/" end onClick={() => setMenuOpen(false)}>
        <i className="bi bi-house-door me-2" aria-hidden="true"></i>
        Home
      </NavLink>

      <NavLink className="nav-link text-white py-2" to="/rooms" onClick={() => setMenuOpen(false)}>
        <i className="bi bi-search me-2" aria-hidden="true"></i>
        Find Rooms
      </NavLink>

      <NavLink className="nav-link text-white py-2" to="/sessions" onClick={() => setMenuOpen(false)}>
        <i className="bi bi-people me-2" aria-hidden="true"></i>
        Study Sessions
      </NavLink>

      <NavLink className="nav-link text-white py-2" to="/bookings" onClick={() => setMenuOpen(false)}>
        <i className="bi bi-calendar-check me-2" aria-hidden="true"></i>
        My Bookings
      </NavLink>

      <NavLink className="nav-link text-white py-2" to="/login" onClick={() => setMenuOpen(false)}>
        <i className="bi bi-box-arrow-in-right me-2" aria-hidden="true"></i>
        Login
      </NavLink>
    </nav>
  );

  return (
    <aside
      className="col-12 col-md-3 col-xl-2 text-white p-3 p-md-4"
      style={{ backgroundColor: "#172B4D" }}
    >
      <div className="d-flex align-items-center justify-content-between mb-md-5">
        <h3 className="fw-bold m-0 fs-4">
          🎓 StudyFlow
        </h3>

        <button
          type="button"
          className="btn btn-outline-light d-md-none"
          aria-label={menuOpen ? "Close navigation" : "Open navigation"}
          aria-expanded={menuOpen}
          aria-controls="studyflow-navigation"
          onClick={() => setMenuOpen(!menuOpen)}
          style={{ minWidth: 44, minHeight: 44 }}
        >
          <i
            className={`bi ${menuOpen ? "bi-x-lg" : "bi-list"}`}
            aria-hidden="true"
          ></i>
        </button>
      </div>

      <div
        id="studyflow-navigation"
        className={`${menuOpen ? "d-block" : "d-none"} d-md-block mt-3 mt-md-0`}
      >
        {navigation}
      </div>
    </aside>
  );
}
