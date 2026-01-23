import React from 'react';
import { NavLink, Link } from 'react-router-dom';
import { Squirrel, Home, Book, Calendar, Users, GraduationCap } from 'lucide-react';
import SquirrelImg from '../assets/EduSQRL.svg';

const CircularNav = () => {
  const menuItems = [
    
    { to: "/courses", icon: <Book size={20} />, label: "Lägg till kurs" },
    { to: "/instances", icon: <Calendar size={20} />, label: "Tillfällen" },
    { to: "/participants", icon: <Users size={20} />, label: "Deltagare" },
    { to: "/teachers", icon: <GraduationCap size={20} />, label: "Lärare" },
    { to: "/registrations", icon: <GraduationCap size={20} />, label: "Registrera" },
  ];

  return (
    <div className="circular-nav-wrapper">
      <div className="circular-nav-container">
        
        {/* MITTPUNKTEN: Ekorren */}
        <Link to ="/" className="mascot-center" >
          <img src={SquirrelImg} alt="EduSqrl Mascot" style={{ width: '100px' }}/>
          <div className="pulse-effect"></div>
        </Link>

        {/* KNAPPARNA RUNT OM */}
        {menuItems.map((item, index) => {
          // Vi räknar ut vinkeln för varje knapp (360 grader / antal knappar)
          const angle = (index * (360 / menuItems.length)) - 90; // -90 för att börja högst upp
          
          return (
            <NavLink 
              key={item.to} 
              to={item.to} 
              className="circle-item"
              style={{ '--angle': `${angle}deg` }}
            >
              <div className="circle-icon-box">
                {item.icon}
                <span className="circle-label">{item.label}</span>
              </div>
            </NavLink>
          );
        })}

      </div>
    </div>
  );
};

export default CircularNav;