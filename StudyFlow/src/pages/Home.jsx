
import { Link } from "react-router-dom";
import "./Home.css";

export default function Home() {
  return (
    <main className="home-page container py-5">
      <section className="home-welcome p-4 mb-4 bg-white rounded-4 shadow-sm">
        <div className="row align-items-center g-4">
          <div className="col-lg-6">
            <h1 className="home-title fw-bold mb-3">
              Your space to study together.
            </h1>

            <p className="fs-5 text-secondary mb-4">
              Find study rooms, create bookings and organize
              your study sessions in one place.
            </p>

            <Link to="/rooms" className="btn btn-primary btn-lg">
              Find Rooms
            </Link>
          </div>

          <div className="col-lg-6">
            <img
              src="/images/Home_side.png"
              alt="Modern study room"
              className="home-image img-fluid rounded-4 w-100 object-fit-cover"
            />
          </div>
        </div>
      </section>
    </main>
  );
}
