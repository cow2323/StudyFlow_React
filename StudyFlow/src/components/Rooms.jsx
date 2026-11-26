import { useEffect, useState } from "react";
import { getRooms } from "../services/roomService";
import RoomDetails from "./RoomDetails";

export default function Rooms() {
  const [rooms, setRooms] = useState([]);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [selectedRoom, setSelectedRoom] = useState(null);

  useEffect(() => {
    let active = true;

    async function loadRooms() {
      try {
        const data = await getRooms();

        if (active) {
          setRooms(data);
        }
      } catch {
        if (active) {
          setError("Kunne ikke hente rommene. Prøv igjen senere.");
        }
      } finally {
        if (active) {
          setLoading(false);
        }
      }
    }

    loadRooms();

    return () => {
      active = false;
    };
  }, []);

  const filteredRooms = rooms.filter((room) =>
    `${room.name} ${room.location}`
      .toLowerCase()
      .includes(search.trim().toLowerCase())
  );

  if (selectedRoom) {
    return (
      <RoomDetails
        room={selectedRoom}
        onBack={() => setSelectedRoom(null)}
      />
    );
  }

  return (
    <section
      className="container py-4 text-start"
      aria-labelledby="rooms-title"
    >
      <header className="mb-4">
        <h1 id="rooms-title" className="fw-bold">
          Study Rooms
        </h1>
        <p className="text-muted">
          Find a room that fits your study plans.
        </p>
      </header>

      <div className="mb-4">
        <label htmlFor="room-search" className="form-label">
          Search rooms
        </label>

        <input
          id="room-search"
          type="search"
          className="form-control"
          placeholder="Search room..."
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />
      </div>

      {loading && <p role="status">Loading rooms...</p>}

      {error && (
        <p className="alert alert-danger" role="alert">
          {error}
        </p>
      )}

      {!loading && !error && (
        <>
          <p className="text-muted" role="status">
            {filteredRooms.length} rooms found
          </p>

          <div className="row g-4">
            {filteredRooms.map((room) => {
              const imageNumber =
                room.id === 2 ? 2 : room.id === 3 ? 3 : 1;

              return (
                <article
                  className="col-md-6 col-lg-4"
                  key={room.id}
                >
                  <div className="card h-100 shadow-sm">
                    <img
                      src={`http://localhost:5245/images/Study%20rooms_side/study%20room${imageNumber}.jpg`}
                      className="card-img-top"
                      alt={room.name}
                      style={{
                        height: "220px",
                        objectFit: "cover"
                      }}
                    />

                    <div className="card-body">
                      <h2 className="h5 mb-3">{room.name}</h2>

                      <p className="mb-2">
                        <strong>Capacity:</strong> {room.capacity}
                      </p>

                      <p className="mb-2">
                        <strong>Location:</strong> {room.location}
                      </p>

                      {room.description && (
                        <p className="text-muted mb-0">
                          {room.description}
                        </p>
                      )}
                    </div>

                    <footer className="card-footer bg-white border-0">
                      <button
                        type="button"
                        className="btn btn-primary w-100"
                        onClick={() => setSelectedRoom(room)}
                      >
                        View Details
                      </button>
                    </footer>
                  </div>
                </article>
              );
            })}
          </div>
        </>
      )}
    </section>
  );
}