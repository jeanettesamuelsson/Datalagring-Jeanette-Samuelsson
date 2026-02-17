import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Users, BookOpen, Calendar, ClipboardCheck, Home } from 'lucide-react';
import SquirrelImg from '../assets/EduSQRL.svg';

const Sidebar = () => {
  const location = useLocation();

  const menuItems = [
    { path: '/', label: 'Hem', icon: <Home size={22} /> },
    { path: '/participants', label: 'Deltagare', icon: <Users size={22} /> },
    { path: '/courses', label: 'Kurser', icon: <BookOpen size={22} /> },
    { path: '/instances', label: 'Kurstillfällen', icon: <Calendar size={22} /> },
    { path: '/registrations', label: 'Registreringar', icon: <ClipboardCheck size={22} /> },
  ];

  return (
    <aside className="sidebar">
      <div className="sidebar-header">
        
          <img className="sidebar-image" src={SquirrelImg} alt="EduSqrl Mascot" />

        <h2 className="sidebar-logo">EduSQ(R)L</h2>
      </div>
      
      <nav className="sidebar-nav">
        {menuItems.map((item) => (
          <Link
            key={item.path}
            to={item.path}
            className={`nav-button ${location.pathname === item.path ? 'active' : ''}`}
          >
            <span className="nav-icon">{item.icon}</span>
            <span className="nav-label">{item.label}</span>
          </Link>
        ))}
      </nav>
    </aside>
  );
};

export default Sidebar;