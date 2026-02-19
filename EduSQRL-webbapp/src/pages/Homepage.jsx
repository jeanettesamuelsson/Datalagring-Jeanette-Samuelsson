import React, { useState, useEffect } from 'react';
import { Squirrel, ArrowRight, Calendar, MapPin } from 'lucide-react'; 
import { Link } from 'react-router-dom';

const Homepage = () => {
  const [sessions, setSessions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // 1. Ändra URL till ditt endpoint för kurstillfällen
    const API_URL = 'https://localhost:7054/api/courseSessions';

    // 2. Hämta data från backend
    fetch(API_URL)
      .then((response) => {
        if (!response.ok) {
          throw new Error('Kunde inte hämta kurstillfällen från servern.');
        }
        return response.json();
      })
      .then((data) => {
        setSessions(data);
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
      <div className="content-container">
        {/* List of course sessions */}
        <div className="list-section">
          <h3>Aktuella kurstillfällen</h3>
          <p style={{ marginBottom: '20px', color: '#666' }}>Passa på att säkra din plats!</p>
          
          <ul className="data-list">
            {sessions.map((session) => ( 
              <li key={session.id} className="data-list-item">
                <div style={{ display: 'flex', flexDirection: 'column', gap: '5px' }}>
                  {/* Kursens namn (nu när vi fixat backenden så den följer med!) */}
                  <span className="item-name" style={{ fontWeight: 'bold' }}>
                    {session.courseName}
                  </span>
                  
                  {/* Info om plats och datum */}
                  <div style={{ display: 'flex', gap: '15px', color: '#6b7280', fontSize: '0.85rem' }}>
                    <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
                      <MapPin size={14} /> {session.locationName}
                    </span>
                    <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
                      <Calendar size={14} /> {new Date(session.startDate).toLocaleDateString()}
                    </span>
                  </div>
                </div>
                
                {/* Länka till registrering med sessionId */}
                <Link to={`/registrations?sessionId=${session.id}`} style={{ color: '#ea580c' }}>
                  <ArrowRight size={20} />
                </Link>
              </li>
            ))}
          </ul>
        </div>

      </div>
    </div>
  );
};

export default Homepage;