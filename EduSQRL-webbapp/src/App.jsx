
import './App.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { NavLink } from 'react-router-dom';
import Courses from './pages/Courses';
import Instances from './pages/Instances';
import Participants from './pages/Participants';
import Homepage from './pages/Homepage';
import Header from './components/Header';
import Teachers from './pages/Teachers';

function App() {
  
  // En hjälpfunktion för att slippa skriva samma Tailwind-klasser om och om igen
  const navLinkClasses = ({ isActive }) => 
    `flex items-center gap-3 p-3 rounded-lg transition-all ${
      isActive 
        ? "bg-orange-100 text-orange-700 font-bold shadow-sm" 
        : "text-gray-600 hover:bg-orange-50 hover:text-orange-600"
    }`;

  return (
    <>
     <BrowserRouter>
      <div >
        <nav>
          <Header/ >
        </nav>
        
      
        {/* --- CONTENT --- */}
        <main>
          <Routes>
            <Route path="/" element={<Homepage />} />
            <Route path="/courses" element={<Courses />} />
            <Route path="/instances" element={<Instances/>} />
            <Route path="/participants" element={<Participants />} />
            <Route path="/teachers" element={<Teachers />} />
          </Routes>
        </main>

      </div>
    </BrowserRouter>
    </>
  )
}

export default App
