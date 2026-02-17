import './App.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Courses from './pages/Courses';
import Instances from './pages/Instances';
import Participants from './pages/Participants';
import Homepage from './pages/Homepage';
import Header from './components/Header';
import Registrations from './pages/Registrations';
import Sidebar from './components/Sidebar';

function App() {
  return (
    <BrowserRouter>
      {/* Behållaren som styr hela sidans layout via CSS Grid */}
      <div className="app-grid-container">
        
        <header className="site-header">
          <Header />
        </header>
        
        {/* Sidebar fungerar som din navigation-section */}
        <Sidebar />

        <main className="content-section">
          <Routes>
            <Route path="/" element={<Homepage />} />
            <Route path="/courses" element={<Courses />} />
            <Route path="/instances" element={<Instances/>} />
            <Route path="/participants" element={<Participants />} />
            <Route path="/registrations" element={<Registrations />} />
          </Routes>
        </main>

      </div>
    </BrowserRouter>
  )
}

export default App