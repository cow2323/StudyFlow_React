
import { BrowserRouter, Routes, Route } from "react-router-dom";
import DashboardLayout from "./layouts/DashboardLayout";
import Home from "./pages/Home";
import StudySession from "./components/StudySession";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<DashboardLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/sessions" element={<StudySession />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
