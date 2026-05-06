import { Navigate, Route, Routes } from 'react-router-dom';
import AdminLayout from './layouts/AdminLayout.jsx';
import PublicLayout from './layouts/PublicLayout.jsx';
import AttractionManagement from './admin/AttractionManagement.jsx';
import Dashboard from './admin/Dashboard.jsx';
import About from './pages/About.jsx';
import Cities from './pages/Cities.jsx';
import Contact from './pages/Contact.jsx';
import DestinationDetail from './pages/DestinationDetail.jsx';
import Destinations from './pages/Destinations.jsx';
import ExperienceDetail from './pages/ExperienceDetail.jsx';
import Experiences from './pages/Experiences.jsx';
import Home from './pages/Home.jsx';
import Hotels from './pages/Hotels.jsx';
import Login from './pages/Login.jsx';
import Tours from './pages/Tours.jsx';

export default function App() {
  return (
    <Routes>
      <Route element={<PublicLayout />}>
        <Route path="/" element={<Home />} />
        <Route path="/destinations" element={<Destinations />} />
        <Route path="/destinations/:id" element={<DestinationDetail />} />
        <Route path="/cities" element={<Cities />} />
        <Route path="/experiences" element={<Experiences />} />
        <Route path="/experiences/:id" element={<ExperienceDetail />} />
        <Route path="/tours" element={<Tours />} />
        <Route path="/hotels" element={<Hotels />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
        <Route path="/login" element={<Login />} />
      </Route>

      <Route path="/admin" element={<AdminLayout />}>
        <Route index element={<Dashboard />} />
        <Route path="attractions" element={<AttractionManagement />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
