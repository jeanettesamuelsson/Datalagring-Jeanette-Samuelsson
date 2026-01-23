import React from 'react'
import { NavLink } from 'react-router-dom';
import { Squirrel, Home, Book, Calendar, Users } from 'lucide-react';
import CircularNav from './CircularNav';

function Header() {
  return (

    <header className="main-header">
      <div className="header-inner">
        
      
        <div className="logo">
          <Squirrel size={28} />
          <span>EduSqrl</span>
        </div>

      
    
      

      </div>
    </header>
    




  )
}

export default Header