import React, { useState, useEffect } from 'react';

const Registrations = () => {
  const [registrations, setRegistrations] = useState([]);
  const [participants, setParticipants] = useState([]);
  const [sessions, setSessions] = useState([]);
  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [formData, setFormData] = useState({
    participantId: '',
    courseSessionId: '',
  });

  const BASE_URL = 'https://localhost:7054';

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [regRes, partRes, sessRes] = await Promise.all([
        fetch(`${BASE_URL}/api/registrations`),
        fetch(`${BASE_URL}/participants`),
        fetch(`${BASE_URL}/api/courseSessions`)
      ]);

      if (!regRes.ok || !partRes.ok || !sessRes.ok) throw new Error("Kunde inte hämta data");

      const regData = await regRes.json();
      const partData = await partRes.json();
      const sessData = await sessRes.json();

      setRegistrations(regData);
      setParticipants(partData);
      setSessions(sessData);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // --- delete ---
  const handleDelete = async (id, rowVersion) => {
    if (!window.confirm("Vill du verkligen ta bort denna registrering?")) {
      return;
    }

    try {
      const response = await fetch(`${BASE_URL}/api/registrations/${id}`, {
        method: 'DELETE',
        headers: { 'If-Match': rowVersion },
       
      });

      if (!response.ok) {
        if (response.status === 409) {
          throw new Error("Kunde inte radera: Registreringen har ändrats nyss. Ladda om sidan.");
        }
        throw new Error("Gick inte att radera registreringen");
      }

      // Update list aftr deleting
      fetchData(); 
      alert("Registreringen har raderats ur förrådet.");
      
    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`${BASE_URL}/api/registrations`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || "Registreringen misslyckades");
      }

      fetchData(); // Hämta på nytt
      alert(`Ekorr-post skickad! Registreringen är klar.`);
      setFormData({ participantId: '', courseSessionId: '' });
      
    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  if (loading) return <div className="p-10 text-center">Hämtar data från ekorr-förrådet... 🐿️</div>;
  if (error) return <div className="p-10 text-red-500">Fel: {error}</div>;

  return (
    <div className="content-container">
      
      {/* LIST OF REGISTRATIONS */}
      <div className="list-section">
        <h3>Genomförda registreringar</h3>

        {registrations.length === 0 ? (
          <div className="empty-state-box">
            <p>Här var det tomt! 🐿️</p>
            <span>Lägg till nya registreringar i formuläret till höger för att fylla förrådet.</span>
          </div>
        ) : (
        <ul className="data-list">
          {registrations.map((reg) => (
            <li key={reg.id} className="data-list-item">
              <div>
                <span className="item-name">{reg.participantName}</span>
                <br />
                <span className="item-info">{reg.courseName}</span>
              </div>
              <div style={{ textAlign: 'right', display: 'flex', alignItems: 'center', gap: '15px' }}>
                <div>
                  <span className="item-info" style={{ display: 'block' }}>{new Date(reg.created).toLocaleDateString()}</span>
                  <span className={`status-badge ${reg.status.toLowerCase()}`}>{reg.status}</span>
                </div>
                
                {/* DELETE BTN */}
                <button 
                  className="btn-delete"
                  onClick={() => handleDelete(reg.id, reg.rowVersion)}
                >
                  Ta bort
                </button>
              </div>
            </li>
          ))}
        </ul>
        )}
      </div>

      {/* HÖGER: FORMULÄRET */}
      <div className="form-container">
        <h3>Ny kursregistrering</h3>

          
        <form onSubmit={handleSubmit} className="course-form">
          <div className="form-group">
            <label>Välj student</label>
            <select 
              required 
              value={formData.participantId}
              onChange={(e) => setFormData({...formData, participantId: e.target.value})}
            >
              <option value="">-- Välj student --</option>
              {participants.map(p => (
                <option key={p.id} value={p.id}>
                  {p.firstName} {p.lastName}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Välj kurstillfälle</label>
            <select 
              required 
              value={formData.courseSessionId}
              onChange={(e) => setFormData({...formData, courseSessionId: e.target.value})}
            >
              <option value="">-- Välj tillfälle --</option>
              {sessions.map(s => (
                <option key={s.id} value={s.id}>
                  {s.courseName} - {s.locationName} ({new Date(s.startDate).toLocaleDateString()})
                </option>
              ))}
            </select>
          </div>

          <button type="submit" className="btn-add-course">Registrera på kurs</button>
        </form>
        
      </div>
    </div>
  );
};

export default Registrations;