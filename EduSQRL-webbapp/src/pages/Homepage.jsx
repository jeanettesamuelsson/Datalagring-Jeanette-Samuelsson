import React, { useState, useEffect } from 'react';
import { Squirrel, ArrowRight } from 'lucide-react'; 
import { Link } from 'react-router-dom';

const Homepage = () => {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // 1. Definiera URL till ditt API
    const API_URL = 'https://localhost:7054/api/courses';

    // 2. Hämta data från backend
    fetch(API_URL)
      .then((response) => {
        if (!response.ok) {
          throw new Error('Kunde inte hämta kurserna från servern.');
        }
        return response.json();
      })
      .then((data) => {
        setCourses(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Fetch error:", err);
        setError(err.message);
        setLoading(false);
      });
  }, []);

  // Visa laddningsvy
  if (loading) return <div className="p-10">Samlar ihop nötterna... 🐿️</div>;
  
  // Visa felmeddelande
  if (error) return <div className="p-10 text-red-500">Hoppsan! {error}</div>;

  return (
    <div className="homepage-container">
      <h2>Välkommen till EduSQ(R)L</h2>

      <div className="content-container">
        {/* List of courses */}
        <div className="list-section">
          <h3>Våra pågående kurser, klicka för att registrera!</h3>
          
          <ul className="data-list">
            {courses.map((course) => ( 
              // Använd course.id som key (alltid bäst med Guid)
              <li key={course.id} className="data-list-item">
                <div style={{ display: 'flex', alignItems: 'center', gap: '15px' }}>
                  <div>
                    {/* Kontrollera att fältnamnet matchar din CourseOutput DTO (troligen courseName) */}
                    <span className="item-name">{course.courseName}</span>
                    <br />
                    <span className="item-info">{course.courseCode}</span>
                  </div>
                </div>
                
                {/* Länka till registrering - tips: skicka med kursens ID i URLen! */}
                <Link to={`/registrations?courseId=${course.id}`} style={{ color: '#ea580c' }}>
                  <ArrowRight size={15} />
                </Link>
              </li>
            ))}
          </ul>
        </div>

        {/* Info Card */}
        <div className="info-card" style={{ background: '#fff7ed', padding: '20px', borderRadius: '15px', border: '1px solid #ffedd5' }}>
           <h4 style={{ color: '#c2410c', marginTop: 0 }}>Visste du att...</h4>
           <p style={{ color: '#4b5563', fontSize: '0.9rem' }}>
             EduSQ(R)L hjälper dig att hålla koll på nötterna – jag menar, kurserna! 
             Just nu har vi **{courses.length}** kurser aktiva i systemet.
           </p>
           <Squirrel size={40} color="#ea580c" />
        </div>
      </div>
    </div>
  );
};

export default Homepage;