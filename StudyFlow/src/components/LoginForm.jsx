import { useState } from "react";

export default function LoginForm() {
  const [message, setMessage] = useState("");

  function handleSubmit(event) {
    event.preventDefault();
    setMessage("Innlogging er foreløpig ikke koblet til API-et.");
  }

  return (
    <form onSubmit={handleSubmit}>
      <h1>Logg inn på StudyFlow</h1>

      <p>
        <label htmlFor="login-email">E-post</label><br />
        <input id="login-email" name="email" type="email"
          autoComplete="username" required />
      </p>

      <p>
        <label htmlFor="login-password">Passord</label><br />
        <input id="login-password" name="password" type="password"
          autoComplete="current-password" required />
      </p>

      <label>
        <input name="rememberMe" type="checkbox" /> Husk meg
      </label>

      <p><button type="submit">Logg inn</button></p>
      {message && <p role="status">{message}</p>}
    </form>
  );
}