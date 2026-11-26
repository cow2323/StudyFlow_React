import BookingForm from "./BookingForm";

export default function RoomDetails({ room, onBack }) {
  const imageNumber =
    room.id === 2 ? 2 : room.id === 3 ? 3 : 1;

  return (
    <section className="container py-4 text-start">
      <button
        type="button"
        className="btn btn-link p-0 text-decoration-none"
        onClick={onBack}
      >
        ← Back to rooms
      </button>

      <div className="card border-0 shadow-sm rounded-4 mt-4">
        <div className="card-body p-4">
          <div className="row g-4 align-items-center">
            <div className="col-md-5">
              <div className="ratio ratio-4x3">
                <img
                  src={`http://localhost:5245/images/Study%20rooms_side/study%20room${imageNumber}.jpg`}
                  alt={room.name}
                  className="w-100 h-100 object-fit-cover rounded-3"
                />
              </div>
            </div>

            <div className="col-md-7">
              <h2 className="fw-bold mb-4">{room.name}</h2>

              <p>
                <strong>Capacity:</strong> {room.capacity} people
              </p>

              <p>
                <strong>Location:</strong> {room.location}
              </p>

              {room.description && <p>{room.description}</p>}
            </div>
          </div>

          <hr className="my-4" />

          <BookingForm
            key={room.id}
            selectedRoomId={room.id}
          />
        </div>
      </div>
    </section>
  );
}