import { useEffect, useState } from "react";
import * as BookingService from "../services/BookingService";

function StudySession() {
  const [sessions, setSessions] = useState([]);
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState("upcoming");
  const [activeTab, setActiveTab] = useState("all");

  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const [showForm, setShowForm] = useState(false);
  const [editingSession, setEditingSession] = useState(null);

  const [roomId, setRoomId] = useState("");
  const [course, setCourse] = useState("");
  const [topic, setTopic] = useState("");
  const [description, setDescription] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [isPublic, setIsPublic] = useState(true);

  async function loadSessions() {
    try {
      setError("");

      const data = await BookingService.getAllStudySessions();
      setSessions(data);
    } catch (error) {
      setError("Could not load study sessions.");
    }
  }

  useEffect(() => {
    loadSessions();
  }, []);

  function openCreateForm() {
    setEditingSession(null);

    setRoomId("");
    setCourse("");
    setTopic("");
    setDescription("");
    setStartTime("");
    setEndTime("");
    setIsPublic(true);

    setError("");
    setMessage("");
    setShowForm(true);
  }

  function openUpdateForm(session) {
    setEditingSession(session);

    setRoomId(session.roomId);
    setCourse(session.course);
    setTopic(session.topic);
    setDescription(session.description || "");
    setStartTime(formatDateTime(session.startTime));
    setEndTime(formatDateTime(session.endTime));
    setIsPublic(session.isPublic);

    setError("");
    setMessage("");
    setShowForm(true);
  }

  function formatDateTime(value) {
    const date = new Date(value);
    const offset = date.getTimezoneOffset();

    return new Date(date.getTime() - offset * 60000)
      .toISOString()
      .slice(0, 16);
  }

  async function handleSubmit(event) {
    event.preventDefault();

    setError("");
    setMessage("");

    if (new Date(endTime) <= new Date(startTime)) {
      setError("End time must be after start time.");
      return;
    }

    const session = {
      id: editingSession ? editingSession.id : 0,
      roomId: Number(roomId),
      course: course,
      topic: topic,
      description: description,
      startTime: new Date(startTime).toISOString(),
      endTime: new Date(endTime).toISOString(),
      isPublic: isPublic
    };

    try {
      if (editingSession) {
        await BookingService.updateSession(session);
        setMessage("Study session updated.");
      } else {
        await BookingService.createStudySession(session);
        setMessage("Study session created.");
      }

      setShowForm(false);
      setEditingSession(null);

      await loadSessions();
    } catch (error) {
      setError(error.message);
    }
  }

  async function handleJoin(session) {
    try {
      setError("");
      setMessage("");

      await BookingService.joinStudySession(session);

      setMessage("You joined the study session.");
      await loadSessions();
    } catch (error) {
      setError(error.message);
    }
  }

  async function handleLeave(session) {
    try {
      setError("");
      setMessage("");

      await BookingService.leaveStudySession(session);

      setMessage("You left the study session.");
      await loadSessions();
    } catch (error) {
      if (error instanceof SyntaxError) {
        setMessage("You left the study session.");
        await loadSessions();
      } else {
        setError(error.message);
      }
    }
  }

  const filteredSessions = sessions.filter((session) => {
    const searchText = search.toLowerCase();

    const matchesSearch =
      session.topic.toLowerCase().includes(searchText) ||
      session.course.toLowerCase().includes(searchText) ||
      (session.description || "")
        .toLowerCase()
        .includes(searchText);

    if (filter === "upcoming") {
      return (
        matchesSearch &&
        new Date(session.startTime) > new Date()
      );
    }

    return matchesSearch;
  });

  return (
    <main className="container py-4" style={{ maxWidth: "1200px" }}>
      <h1 className="fw-bold fs-2 mb-2">
        Study Sessions
      </h1>

      <p className="text-muted mb-3">
        Find and join study sessions with other students.
      </p>

      <button
        className="btn btn-primary mb-4"
        onClick={openCreateForm}
      >
        Create Study Session
      </button>

      {showForm && (
        <section className="card border-0 shadow-sm mb-4">
          <div className="card-body p-3">
            <h2 className="fs-5 fw-bold mb-3">
              {editingSession
                ? "Update Study Session"
                : "Create Study Session"}
            </h2>

            <form onSubmit={handleSubmit}>
              <div className="row g-3">

                <div className="col-md-4">
                  <label className="form-label">
                    Room ID
                  </label>

                  <input
                    type="number"
                    className="form-control"
                    min="1"
                    value={roomId}
                    onChange={(e) => setRoomId(e.target.value)}
                    required
                  />
                </div>

                <div className="col-md-4">
                  <label className="form-label">
                    Course
                  </label>

                  <input
                    type="text"
                    className="form-control"
                    value={course}
                    onChange={(e) => setCourse(e.target.value)}
                    required
                  />
                </div>

                <div className="col-md-4">
                  <label className="form-label">
                    Topic
                  </label>

                  <input
                    type="text"
                    className="form-control"
                    value={topic}
                    onChange={(e) => setTopic(e.target.value)}
                    required
                  />
                </div>

                <div className="col-md-6">
                  <label className="form-label">
                    Start Time
                  </label>

                  <input
                    type="datetime-local"
                    className="form-control"
                    value={startTime}
                    onChange={(e) => setStartTime(e.target.value)}
                    required
                  />
                </div>

                <div className="col-md-6">
                  <label className="form-label">
                    End Time
                  </label>

                  <input
                    type="datetime-local"
                    className="form-control"
                    value={endTime}
                    onChange={(e) => setEndTime(e.target.value)}
                    required
                  />
                </div>

                <div className="col-12">
                  <label className="form-label">
                    Description
                  </label>

                  <textarea
                    className="form-control"
                    rows="3"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                  />
                </div>

                <div className="col-12">
                  <div className="form-check">
                    <input
                      type="checkbox"
                      className="form-check-input"
                      id="publicSession"
                      checked={isPublic}
                      onChange={(e) => setIsPublic(e.target.checked)}
                    />

                    <label
                      className="form-check-label"
                      htmlFor="publicSession"
                    >
                      Public session
                    </label>
                  </div>
                </div>

              </div>

              <div className="mt-3">
                <button
                  type="submit"
                  className="btn btn-primary me-2"
                >
                  {editingSession ? "Update" : "Create"}
                </button>

                <button
                  type="button"
                  className="btn btn-outline-secondary"
                  onClick={() => setShowForm(false)}
                >
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </section>
      )}

      <nav className="d-flex gap-4 mb-4">
        <button
          type="button"
          className={
            activeTab === "all"
              ? "btn p-0 fw-bold border-0 border-bottom border-3 border-dark rounded-0"
              : "btn p-0 text-primary border-0"
          }
          onClick={() => setActiveTab("all")}
        >
          All Sessions
        </button>

        <button
          type="button"
          className={
            activeTab === "mine"
              ? "btn p-0 fw-bold border-0 border-bottom border-3 border-dark rounded-0"
              : "btn p-0 text-primary border-0"
          }
          onClick={() => setActiveTab("mine")}
        >
          My Sessions
        </button>
      </nav>

      <section className="row g-3 align-items-end mb-4">
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
            <option value="upcoming">
              Upcoming
            </option>

            <option value="all">
              All sessions
            </option>
          </select>
        </div>
      </section>

      {error && (
        <div className="alert alert-danger">
          {error}
        </div>
      )}

      {message && (
        <div className="alert alert-success">
          {message}
        </div>
      )}

      {activeTab === "all" && (
        <section className="row g-3">
          {filteredSessions.map((session) => {
            const start = new Date(session.startTime);
            const end = new Date(session.endTime);

            return (
              <article
                className="col-md-4"
                key={session.id}
              >
                <div className="card h-100 shadow-sm border-0">
                  <div className="card-body p-3 d-flex flex-column">

                    <h2 className="fw-bold fs-5 mb-3">
                      {session.topic}
                    </h2>

                    <div className="mb-3">
                      <span className="badge bg-primary me-2">
                        {session.course}
                      </span>

                      {session.description && (
                        <span className="badge bg-light text-dark">
                          {session.description}
                        </span>
                      )}
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
                      <strong>Room:</strong>{" "}
                      Room {session.roomId}
                    </p>

                    <p className="mb-3">
                      <strong>Participants:</strong>{" "}
                      {session.participantCount}
                    </p>

                    <div className="d-flex gap-2 mt-auto">

                      <button
                        className="btn btn-primary"
                        onClick={() => handleJoin(session)}
                      >
                        Join
                      </button>

                      <button
                        className="btn btn-outline-danger"
                        onClick={() => handleLeave(session)}
                      >
                        Leave
                      </button>

                      <button
                        className="btn btn-outline-secondary"
                        onClick={() => openUpdateForm(session)}
                      >
                        Update
                      </button>

                    </div>
                  </div>
                </div>
              </article>
            );
          })}
        </section>
      )}

      {activeTab === "mine" && (
        <p className="text-muted">
          backend ?
        </p>
      )}

    </main>
  );
}

export default StudySession;  