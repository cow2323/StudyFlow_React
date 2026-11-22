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
    <main className="container py-4" style={{ maxWidth: "1200px" }}>
      <h1 className="fw-bold fs-2 mb-2">
        Study Sessions
      </h1>

      <p className="text-muted mb-3">
        Find and join study sessions with other students.
      </p>

      <button className="btn btn-primary mb-4">
        Create Study Session
      </button>

      <div className="d-flex gap-4 mb-4">
        <button
          type="button"
          className="btn p-0 fw-bold border-0 border-bottom border-3 border-dark rounded-0"
        >
          All Sessions
        </button>

        <button
          type="button"
          className="btn p-0 text-primary border-0"
        >
          My Sessions
        </button>
      </div>

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
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        <div className="col-md-3">
          <label className="form-label">
            Filter
          </label>

          <select
            className="form-select"
            value={filter}
            onChange={(e) => setFilter(e.target.value)}
          >
            <option value="upcoming">Upcoming</option>
            <option value="all">All sessions</option>
          </select>
        </div>
      </div>

      <div className="row g-3">
        {filteredSessions.map((session) => {
          const start = new Date(session.startTime);
          const end = new Date(session.endTime);

          return (
            <div className="col-md-4" key={session.id}>
              <article className="card h-100 shadow-sm border-0">
                <div className="card-body p-3 d-flex flex-column">

                  <h2 className="fw-bold fs-5 mb-3">
                    {session.topic}
                  </h2>

                  <div className="mb-3">
                    <span className="badge bg-primary me-2">
                      {session.course}
                    </span>

                    <span className="badge bg-light text-dark">
                      {session.description}
                    </span>
                  </div>

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
                    <strong>Room:</strong> Room {session.roomId}
                  </p>

                  <p className="mb-3">
                    <strong>Participants:</strong>{" "}
                    {session.participants}/{session.maxParticipants}
                  </p>

                  <button className="btn btn-primary w-100 mt-auto">
                    Join Session
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

export default StudySession;