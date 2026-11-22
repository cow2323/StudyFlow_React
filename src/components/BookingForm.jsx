import { useState } from "react";
import { createBooking } from "../services/bookingService";

function BookingForm() {
  const [roomId] = useState(10);
  const [date, setDate] = useState("");
  const [startTime, setStartTime] = useState("10:00");
  const [endTime, setEndTime] = useState("12:00");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  async function handleSubmit(event) {
    event.preventDefault();

    setError("");
    setMessage("");

    if (!date) {
      setError("Please select a date.");
      return;
    }

    const startDateTime = `${date}T${startTime}`;
    const endDateTime = `${date}T${endTime}`;

    if (endDateTime <= startDateTime) {
      setError("End time must be after start time.");
      return;
    }

    const booking = {
      roomId,
      startTime: startDateTime,
      endTime: endDateTime
    };

    try {
      await createBooking(booking);
      setMessage("Booking created.");
    } catch (error) {
      setError(error.message);
    }
  }

  return (
    <main className="container py-4" style={{ maxWidth: "1200px" }}>
      <a href="/" className="text-decoration-none d-inline-block mb-4">
        ← Back to rooms
      </a>

      <section className="card border-0 shadow-sm rounded-4">
        <div className="card-body p-4">

          <section className="row align-items-center g-4 mb-4">
            <div className="col-md-5">
              <div
                className="bg-light rounded-4 d-flex flex-column align-items-center justify-content-center"
                style={{ minHeight: "220px" }}
              >
                <p className="text-muted mb-0">
                  No room image available
                </p>
              </div>
            </div>

            <div className="col-md-7">
              <h1 className="fw-bold mb-3 fs-2">
                Room 10
              </h1>

              <p>
                <strong>Capacity:</strong> 16 people
              </p>

              <p>
                <strong>Location:</strong> Pilestredet 32, Oslo
              </p>

              <p className="mb-0">
                Large Study Room
              </p>
            </div>
          </section>

          <hr />

          <h2 className="fw-bold my-4 fs-3">
            Book this room
          </h2>

          <form onSubmit={handleSubmit}>
            <div className="row g-3">

              <div className="col-md-4">
                <label className="form-label">
                  Date
                </label>

                <input
                  type="date"
                  className="form-control"
                  value={date}
                  onChange={(e) => setDate(e.target.value)}
                />
              </div>

              <div className="col-md-4">
                <label className="form-label">
                  Start Time
                </label>

                <input
                  type="time"
                  className="form-control"
                  value={startTime}
                  onChange={(e) => setStartTime(e.target.value)}
                />
              </div>

              <div className="col-md-4">
                <label className="form-label">
                  End Time
                </label>

                <input
                  type="time"
                  className="form-control"
                  value={endTime}
                  onChange={(e) => setEndTime(e.target.value)}
                />
              </div>

            </div>

            {error && (
              <div className="alert alert-danger mt-3">
                {error}
              </div>
            )}

            {message && (
              <div className="alert alert-success mt-3">
                {message}
              </div>
            )}

            <button
              type="submit"
              className="btn btn-primary w-100 mt-4"
            >
              Book Room
            </button>
          </form>

        </div>
      </section>
    </main>
  );
}

export default BookingForm;

          