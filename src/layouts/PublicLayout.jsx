import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar.jsx';
import Footer from '../components/Footer.jsx';

export default function PublicLayout() {
  return (
    <>
      <Navbar />
      <main className="min-h-[60vh] pt-24">
        <Outlet />
      </main>
      <Footer />
    </>
  );
}
