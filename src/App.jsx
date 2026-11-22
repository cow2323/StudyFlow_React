
import { BrowserRouter, Routes, Route } from "react-router-dom";
import DashboardLayout from "./layouts/DashboardLayout";
import Home from "./pages/Home";
import StudySession from "./components/StudySession";
import BookingForm from "./components/BookingForm";

//Authewntication Endpoints
import AuthPage from './auth/AuthPage'
import LoginPage from './auth/LoginForm'
import RegisterPage from './auth/RegisterPage'



import ProtectedRoute from './auth/ProtectedRoute'
import { AuthProvider } from './auth/AuthContext'


function App() {
  return (
    <AuthProvider>
    <BrowserRouter>
      <Routes>
        <Route element={<DashboardLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/sessions" element={<StudySession />} />
          <Route path="/booking" element={<BookingForm />} />
          <Route path="/login" element= {<AuthPage />}/>
        </Route>
      </Routes>
    </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
