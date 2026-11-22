import { useState } from "react";
import {useNavigate } from 'react-router-dom';
import {Form, Button, Container, Alert, Spinner } from 'react-bootstrap'; 
import { useAuth } from './AuthContext';

export default function LoginForm({onLogin}: {onLogin? : () => void}) {

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false); 
  const { login } = useAuth();
  const navigate = useNavigate(); 

  const [message, setMessage] = useState("");

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) =>{
    e.preventDefault();
    setError(null); 
    setIsSubmitting(true);
    try {
      await login({username, password });
      navigate('/Sessions'); 
    } catch(err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('Invalid username or password');
      }
    } finally {
      setIsSubmitting(false);
    }
  };


  return (

      <Container>

    <div className="col-lg-5 bg-primary bg-gradient text-white p-5
        d-flex flex-column justify-content-center">

        <h2 className="fw-bold">🎓 StudyFlow</h2>

        <p className="lead">
            Your space to study together.
        </p>

    </div>
    
    <Form onSubmit={handleSubmit}>
      <Form.Group className="mb-3" controlId="formBasicUsername">
        <Form.Label>Username</Form.Label>
        <Form.Control
        type="text"
        placeholder="Enter Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        disabled={isSubmitting}
        required />

      </Form.Group>

      <Form.Group className="mb-3" controlId="formBasicPassword">
        <Form.Label>Password</Form.Label>
        <Form.Control 
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        disabled={isSubmitting}
        required/>
      </Form.Group>


      <Button variant="primary" type="submit" disabled={isSubmitting}>
                {isSubmitting ? (
                    <>
                    <Spinner animation="border" size="sm" className="me-2" />
                    Logging in...
                    </>
                ):
                (
                    'Login'
                )}
                </Button>
              </Form>
    </Container>
  );
}