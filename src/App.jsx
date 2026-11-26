import { BrowserRouter, Routes, Route } from "react-router-dom";
import DashboardLayout from "./layouts/DashboardLayout";
import Home from "./pages/Home";
import StudySession from "./components/StudySession";
import BookingForm from "./components/BookingForm";
import UserBookings from "./components/UserBookings"; 
import Rooms from "./components/Rooms";
import AuthPage from "./auth/AuthPage";
import { AuthProvider } from "./auth/AuthContext";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<DashboardLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/rooms" element={<Rooms />} />
            <Route path="/sessions" element={<StudySession />} />
            <Route path="/booking" element={<BookingForm />} />
            <Route path="/bookings" element={<UserBookings />} />

            <Route path="/login" element={<AuthPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;