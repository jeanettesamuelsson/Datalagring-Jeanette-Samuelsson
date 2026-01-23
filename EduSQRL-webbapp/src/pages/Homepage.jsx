import React, { useState, useEffect } from 'react';
import { Squirrel, ArrowRight, Book } from 'lucide-react'; 
import { Link } from 'react-router-dom';
import { mockData } from '../data/mockData';   //remove and use database 

const Homepage = () => {

  //set state
  const [courses, setCourses] = useState([]);

  useEffect(() => {
    // Get courses from mockData               //remove and use database 
    setCourses(mockData.courses);
  }, []);


  return (

    <div className="homepage-container">
      <h2>Välkommen till EduSQ(R)L</h2>

      <div className="content-container">

        {/* List of courses */}
        <div className="list-section">
          <h3>Våra pågående kurser, klicka för att registrera!</h3>
          
          <ul className="data-list">
            {courses.map((course) => ( 
              <li key={course.courseCode} className="data-list-item">
                <div style={{ display: 'flex', alignItems: 'center', gap: '15px' }}>
                
                  <div>
                    <span className="item-name">{course.name}</span>
                    <br />
                    <span className="item-info">{course.courseCode}</span>
                  </div>
                </div>
                
                 {/* Link to registration */}
                <Link to="/registration" style={{ color: '#ea580c' }}>
                  <ArrowRight size={15} />
                </Link>
              </li>
            ))}
          </ul>

      
        </div>

       
        <div className="info-card" style={{ background: '#fff7ed', padding: '20px', borderRadius: '15px', border: '1px solid #ffedd5', }}>
           <h4 style={{ color: '#c2410c', marginTop: 0 }}>Visste du att...</h4>
           <p style={{ color: '#4b5563', fontSize: '0.9rem' }}>
             EduSQ(R)L hjälper dig att hålla koll på nötterna – jag menar, kurserna! 
             Just nu har vi {courses.length} kurser aktiva i systemet.
           </p>
           <Squirrel size={40} color="#ea580c" />
        </div>
      </div>
    </div>
 

  );
};

export default Homepage;