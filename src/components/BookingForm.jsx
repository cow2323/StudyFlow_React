import { useState } from "react";
import { createBooking } from "../services/bookingService";

function BookingForm({ selectedRoomId = "" }) {
  const [roomId, setRoomId] = useState(selectedRoomId);
  const [date, setDate] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [saving, setSaving] = useState(false);

  const now = new Date();
  const today = [
    now.getFullYear(),
    String(now.getMonth() + 1).padStart(2, "0"),
    String(now.getDate()).padStart(2, "0")
  ].join("-");

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

    setSaving(true);

    try {
      await createBooking(booking);
      setMessage("Booking created.");
    } catch (error) {
      setError(error.message || "Could not create booking.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <div>
      <h3 className="fw-bold mb-4">Book this room</h3>

      <form onSubmit={handleSubmit}>
        {selectedRoomId === "" && (
          <div className="mb-3">
            <label htmlFor="booking-room" className="form-label">
              Room ID
            </label>
            <input
              id="booking-room"
              type="number"
              className="form-control"
              min="1"
              step="1"
              value={roomId}
              onChange={(event) => setRoomId(event.target.value)}
              required
            />
          </div>
        )}

        <div className="row g-3 mb-4">
          <div className="col-md-4">
            <label htmlFor="booking-date" className="form-label">
              Date
            </label>
            <input
              id="booking-date"
              type="date"
              className="form-control"
              min={today}
              value={date}
              onChange={(event) => setDate(event.target.value)}
              required
            />
          </div>

          <div className="col-md-4">
            <label htmlFor="booking-start" className="form-label">
              Start time
            </label>
            <input
              id="booking-start"
              type="text"
              className="form-control"
              placeholder="10:00"
              pattern="([01][0-9]|2[0-3]):[0-5][0-9]"
              maxLength={5}
              title="Enter a time from 00:00 to 23:59"
              value={startTime}
              onChange={(event) => setStartTime(event.target.value.replace(".", ":"))}
              required
            />
          </div>

          <div className="col-md-4">
            <label htmlFor="booking-end" className="form-label">
              End time
            </label>
            <input
              id="booking-end"
              type="text"
              className="form-control"
              placeholder="12:00"
              pattern="([01][0-9]|2[0-3]):[0-5][0-9]"
              maxLength={5}
              title="Enter a time from 00:00 to 23:59"
              value={endTime}
              onChange={(event) => setEndTime(event.target.value.replace(".", ":"))}
              required
            />
          </div>
        </div>

        <p className="text-muted small"> Enter start and end times, for example 10:00–12:00.
        </p>

        {error && (
          <p className="alert alert-danger" role="alert">
            {error}
          </p>
        )}

        {message && (
          <p className="alert alert-success" role="status">
            {message}
          </p>
        )}

        <button
          type="submit"
          className="btn btn-primary w-100 py-2"
          disabled={saving}
        >
          {saving ? "Booking..." : "Book Room"}
        </button>
      </form>
    </div>
  );
}

export default BookingForm;

          