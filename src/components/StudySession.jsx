import { useState } from "react";

function StudySession() {
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState("upcoming");

  const sessions = [
    {
      id: 1,
      course: "DAVE3625",
      topic: "Introduction to Artificial Intelligence",
      description: "Exam preparation",
      startTime: "2026-11-18T10:00",
      endTime: "2026-11-18T12:00",
      roomId: 1,
      participants: 1,
      maxParticipants: 4
    },
    {
      id: 2,
      course: "ITPE3200",
      topic: "Web Applications",
      description: "Project work",
      startTime: "2026-11-19T14:00",
      endTime: "2026-11-19T16:00",
      roomId: 2,
      participants: 2,
      maxParticipants: 6
    },
    {
      id: 3,
      course: "ITPE3100",
      topic: "Cybersecurity",
      description: "Exam preparation",
      startTime: "2026-11-20T10:00",
      endTime: "2026-11-20T12:00",
      roomId: 3,
      participants: 1,
      maxParticipants: 10
    }
  ];

  const filteredSessions = sessions.filter((session) => {
    const searchText = search.toLowerCase();

    return (
      session.topic.toLowerCase().includes(searchText) ||
      session.course.toLowerCase().includes(searchText) ||
      session.description.toLowerCase().includes(searchText)
    );
  });

  return (
    <div className="container py-5">
      <h1 className="fw-bold">Study Sessions</h1>

      <p className="text-muted">
        Find and join study sessions with other students.
      </p>

      <button className="btn btn-primary mb-4">
        Create Study Session
      </button>

      <div className="row g-3 align-items-end mb-4">
        <div className="col-md-9">
          <label className="form-label">
            Search sessions
          </label>

          <input
            type="text"
            className="form-control"
            placeholder="Search by course, topic or keyword..."
            value={search}
            onChange={(event) => setSearch(event.target.value)}
          />
        </div>

        <div className="col-md-3">
          <label className="form-label">
            Filter
          </label>

          <select
            className="form-select"
            value={filter}
            onChange={(event) => setFilter(event.target.value)}
          >
            <option value="upcoming">Upcoming</option>
            <option value="all">All sessions</option>
          </select>
        </div>
      </div>

      <div className="row g-4">
        {filteredSessions.map((session) => (
          <div className="col-md-4" key={session.id}>
            <div className="card h-100 shadow-sm border-0">
              <div className="card-body d-flex flex-column">
                <h4 className="fw-bold">
                  {session.topic}
                </h4>

                <div className="mb-3">
                  <span className="badge bg-primary me-2">
                    {session.course}
                  </span>

                  <span className="badge bg-light text-dark">
                    {session.description}
                  </span>
                </div>

                <p>
                  <strong>Date:</strong>{" "}
                  {new Date(session.startTime).toLocaleDateString("no-NO")}
                </p>

                <p>
                  <strong>Time:</strong>{" "}
                  {new Date(session.startTime).toLocaleTimeString([], {
                    hour: "2-digit",
                    minute: "2-digit"
                  })}{" "}
                  -{" "}
                  {new Date(session.endTime).toLocaleTimeString([], {
                    hour: "2-digit",
                    minute: "2-digit"
                  })}
                </p>

                <p>
                  <strong>Room:</strong>{" "}
                  Room {session.roomId}
                </p>

                <p>
                  <strong>Participants:</strong>{" "}
                  {session.participants}/{session.maxParticipants}
                </p>

                <button className="btn btn-primary w-100 mt-auto">
                  Join Session
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default StudySession;