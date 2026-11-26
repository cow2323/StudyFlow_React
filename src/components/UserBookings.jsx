import {useState, useEffect} from "react";
import {getAllBookings} from "../services/BookingService";
import {useNavigate} from "react-router-dom";
function UserBookings() {
    //const [filter, SetFilter] = useState("upcoming"); 

    const [bookings, setBookings] = useState([]);
    const [error, setError] = useState(null); 
    const [upcoming, setUpcoming] = useState(true); 

    function isUpcoming(date){

        return new Date(date).getTime() > Date.now();}


    useEffect(() => {
        getAllBookings()
        .then(setBookings)
        .catch((err) => setError(err.message)); 
    }, []); 

    const navigate = useNavigate();
    function redirectToForm() {
        
navigate("/rooms");}
    

    const filteredBookings = bookings.filter((booking) => upcoming ? 

        isUpcoming(booking.startTime) :

        !isUpcoming(booking.startTime)); 

    return (
        <main className="container py-4" style={{ maxWidth: "1200px" }}>
          <h1 className="fw-bold fs-2 mb-2">
            Bookings
          </h1>
    
          <p className="text-muted mb-3">
            View Your Bookings
          </p>
    
          <button className="btn btn-primary mb-4" onClick={redirectToForm}>
            Create New Booking
          </button>
    
          <div className="d-flex gap-4 mb-4">
            <button
              type="button"
              className="btn p-0 fw-bold border-0 border-bottom border-3 border-dark rounded-0"
            >
              All Bookings
            </button>
    
            <button
              type="button"
              className="btn p-0 text-primary border-0"
            >
              My Bookings
            </button>
          </div>
    
          <div className="row g-3 align-items-end mb-4">
    
            <div className="col-md-3">
              <label className="form-label">
                Filter
              </label>
    
              <select
                className="form-select"
                value={upcoming}
               onChange={(e) => setUpcoming(e.target.value === "true")}
              >
                <option value={true}>Upcoming</option>
                <option value={false}>Past</option>
              </select>
            </div>
          </div>
    
          <div className="row g-3">
            {filteredBookings.map((booking) => {

              const start = new Date(booking.startTime);
              const end = new Date(booking.endTime);
    
              return (
                <div className="col-md-4" key={booking.id}>
                  <article className="card h-100 shadow-sm border-0">
                    <div className="card-body p-3 d-flex flex-column">
    

    
                      <p className="mb-2">
                        <strong>Date:</strong>{" "}
                        {start.toLocaleDateString("no-NO")}
                      </p>
    
                      <p className="mb-2">
                        <strong>Time:</strong>{" "}
                        {start.toLocaleTimeString("no-NO", {
                          hour: "2-digit",
                          minute: "2-digit"
                        })}
                        {" - "}
                        {end.toLocaleTimeString("no-NO", {
                          hour: "2-digit",
                          minute: "2-digit"
                        })}
                      </p>
    
                      <p className="mb-2">
                        <strong>Room:</strong> Room {booking.roomId}
                      </p>
    
    
                      <button className="btn btn-primary w-100 mt-auto">
                       Edit Booking
                      </button>
    
                    </div>
                  </article>
                </div>
              );
            })}
          </div>
        </main>
      );

}


export default UserBookings; 