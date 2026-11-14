import { useState } from "react";

function BookingForm() {
  const [roomId, setRoomId] = useState("");
  const [startDateTime, setStartDateTime] = useState("");
  const [endDateTime, setEndDateTime] = useState("");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  function handleSubmit(event) {
    event.preventDefault();

    setError("");
    setMessage("");

    if (Number(roomId) < 1) {
      setError("Room ID must be 1 or higher.");
      return;
    }

    if (endDateTime <= startDateTime) {
      setError("End time must be after start time.");
      return;
    }

    const booking = {
      roomId: Number(roomId),
      startDateTime,
      endDateTime
    };

    console.log(booking);

    setMessage("Booking data is ready.");
  }

  return (
    <div style={{ maxWidth: "500px", margin: "40px auto" }}>
      <h1>Book a Study Room</h1>

      <form onSubmit={handleSubmit}>
        <div style={{ marginBottom: "15px" }}>
          <label>Room ID</label>
          <br />

          <input
            type="number"
            min="1"
            value={roomId}
            onChange={(event) => setRoomId(event.target.value)}
            required
          />
        </div>

        <div style={{ marginBottom: "15px" }}>
          <label>Start time</label>
          <br />

          <input
            type="datetime-local"
            value={startDateTime}
            onChange={(event) => setStartDateTime(event.target.value)}
            required
          />
        </div>

        <div style={{ marginBottom: "15px" }}>
          <label>End time</label>
          <br />

          <input
            type="datetime-local"
            value={endDateTime}
            onChange={(event) => setEndDateTime(event.target.value)}
            required
          />
        </div>

        {error && (
          <p style={{ color: "red" }}>
            {error}
          </p>
        )}

        {message && (
          <p style={{ color: "green" }}>
            {message}
          </p>
        )}

        <button type="submit">
          Book room
        </button>
      </form>
    </div>
  );
}

export default BookingForm;