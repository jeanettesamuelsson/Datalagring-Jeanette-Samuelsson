import React from 'react'
import { NavLink } from 'react-router-dom';
import { Squirrel, Home, Book, Calendar, Users } from 'lucide-react';

function Header() {
  return (

    <header className="main-header">
      <div className="header-inner">
        
        {/* LOGO */}
        <div className="logo">
          <Squirrel size={28} />
          <span>EduSqrl</span>
        </div>

        {/* NAVIGATION */}
        <nav className="nav-menu">
          {/* NavLink lägger automatiskt till klassen "active" när man är på sidan */}
          <NavLink to="/" className="nav-item">
            <Home size={18} /> Hem
          </NavLink>

          <NavLink to="/courses" className="nav-item">
            <Book size={18} /> Kurser
          </NavLink>

          <NavLink to="/instances" className="nav-item">
            <Calendar size={18} /> Tillfällen
          </NavLink>

          <NavLink to="/participants" className="nav-item">
            <Users size={18} /> Deltagare
          </NavLink>

          <NavLink to="/teachers" className="nav-item">
            <Users size={18} /> Lärare
          </NavLink>
        </nav>

      </div>
    </header>
    




  )
}

export default Header