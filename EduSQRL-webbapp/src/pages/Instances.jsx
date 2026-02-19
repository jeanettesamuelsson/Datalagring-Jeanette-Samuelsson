import React, { useState, useEffect } from 'react';

const Instances = () => {
  const [sessions, setSessions] = useState([]);
  const [courses, setCourses] = useState([]);
  const [locations, setLocations] = useState([]);
  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [formData, setFormData] = useState({
    courseId: '',
    locationId: '',
    startDate: '',
    endDate: '',
    capacity: ''
  });

  const BASE_URL = 'https://localhost:7054/api';

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [sessRes, courseRes, locRes] = await Promise.all([
        fetch(`${BASE_URL}/courseSessions`),
        fetch(`${BASE_URL}/courses`),
        fetch(`${BASE_URL}/locations`)
      ]);

      if (!sessRes.ok || !courseRes.ok || !locRes.ok) 
        throw new Error("Kunde inte hämta data från servern");

      setSessions(await sessRes.json());
      setCourses(await courseRes.json());
      setLocations(await locRes.json());
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // --- RADERA KURSTILLFÄLLE ---
  const handleDelete = async (id, rowVersion) => {
    if (!window.confirm("Är du säker på att du vill ta bort detta kurstillfälle? Det kan finnas studenter registrerade.")) {
      return;
    }

    try {
      const response = await fetch(`${BASE_URL}/courseSessions/${id}`, {
        method: 'DELETE',
        headers: { 'If-Match': rowVersion },
        
      });

      if (!response.ok) {
        if (response.status === 409) {
          throw new Error("Kunde inte radera: Tillfället har uppdaterats av någon annan.");
        }
        throw new Error("Gick inte att radera kurstillfället.");
      }

      // Uppdatera listan efter lyckad radering
      const updatedSessions = await fetch(`${BASE_URL}/courseSessions`).then(res => res.json());
      setSessions(updatedSessions);
      alert("Kurstillfället är nu raderat. 🐿️");

    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = { 
        ...formData, 
        capacity: parseInt(formData.capacity) 
      };

      const response = await fetch(`${BASE_URL}/courseSessions`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (!response.ok) throw new Error("Kunde inte skapa kurstillfället");

      fetchData(); // Hämta all data på nytt
      alert(`Nytt kurstillfälle har skapats! 🐿️`);
      setFormData({ courseId: '', locationId: '', startDate: '', endDate: '', capacity: '' });
      
    } catch (err) {
      alert(`Fel: ${err.message}`);
    }
  };

  if (loading) return <div className="p-10 text-center">Planerar ... 🐿️</div>;
  if (error) return <div className="p-10 text-red-500">Fel: {error}</div>;

  return (
    <div className="content-container">
      
      {/* VÄNSTER: LISTA PÅ SESSIONER */}
      <div className="list-section">
        <h3>Kommande kurstillfällen</h3>

         {sessions.length === 0 ? (
          <div className="empty-state-box">
            <p>Här var det tomt! 🐿️</p>
            <span>Lägg till nya kurstillfällen i formuläret till höger för att fylla förrådet.</span>
          </div>
        ) : (


        <ul className="data-list">
          {sessions.map((sess) => (
            <li key={sess.id} className="data-list-item">
              <div>
                <span className="item-name">{sess.courseName}</span>
                <br />
                <span className="item-info">
                  {sess.locationName} | {new Date(sess.startDate).toLocaleDateString()}
                </span>
              </div>
              <div style={{ textAlign: 'right', display: 'flex', alignItems: 'center', gap: '15px' }}>
                 <span className="expertise-tag">{sess.capacity} platser</span>
                 
                 {/* TA BORT-KNAPPEN */}
                 <button 
                  className="btn-delete"
                  onClick={() => handleDelete(sess.id, sess.rowVersion)}
                 >
                  Ta bort
                 </button>
              </div>
            </li>
          ))}
        </ul> 

        )}
      </div>

      {/* HÖGER: FORMULÄR */}
      <div className="form-container">
        <h3>Planera nytt kurstillfälle</h3>
        <form onSubmit={handleSubmit} className="course-form">
          <div className="form-group">
            <label>Kurs</label>
            <select 
              required
              value={formData.courseId}
              onChange={(e) => setFormData({...formData, courseId: e.target.value})}
            >
              <option value="">-- Välj en kurs --</option>
              {courses.map(c => (
                <option key={c.id} value={c.id}>
                  {c.courseCode} - {c.courseName}
                </option>
              ))}
            </select>
          </div>

          <div className="form-row" style={{ display: 'flex', gap: '10px' }}>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Startdatum</label>
              <input 
                type="date" 
                required
                value={formData.startDate}
                onChange={(e) => setFormData({...formData, startDate: e.target.value})}
              />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Slutdatum</label>
              <input 
                type="date" 
                required
                value={formData.endDate}
                onChange={(e) => setFormData({...formData, endDate: e.target.value})}
              />
            </div>
          </div>

          <div className="form-group">
            <label>Plats (Stad)</label>
            <select 
              required
              value={formData.locationId}
              onChange={(e) => setFormData({...formData, locationId: e.target.value})}
            >
              <option value="">-- Välj plats --</option>
              {locations.map(l => (
                <option key={l.id} value={l.id}>{l.name}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Antal platser</label>
            <input 
              type="number" 
              min="1"
              required
              value={formData.capacity}
              onChange={(e) => setFormData({...formData, capacity: e.target.value})}
            />
          </div>

          <button type="submit" className="btn-add-course">
            Skapa tillfälle
          </button>
        </form>
      </div>
    </div>
  );
};

export default Instances;