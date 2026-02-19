import React, { useState, useEffect } from 'react';
import { MapPin, Trash2, PlusCircle } from 'lucide-react';

const Locations = () => {
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [newName, setNewName] = useState('');

  const API_URL = 'https://localhost:7054/api/locations';

  useEffect(() => {
    fetchLocations();
  }, []);

  const fetchLocations = async () => {
    try {
      setLoading(true);
      const response = await fetch(API_URL);
      if (!response.ok) throw new Error("Kunde inte hämta orter från förrådet");
      const data = await response.json();
      setLocations(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: newName })
      });

      if (!response.ok) throw new Error("Gick inte att muta in den här orten");

      setNewName('');
      fetchLocations();
      alert(`Ny plats säkrad! 🐿️`);
    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  const handleDelete = async (id, rowVersion) => {
    if (!window.confirm("Vill du verkligen ta bort den här orten? Kontrollera att inga kurstillfällen är planerade här först!")) {
      return;
    }

    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
        headers: { 
          'If-Match': rowVersion 
        }
      });

      if (!response.ok) {
        if (response.status === 409) throw new Error("Platsen har ändrats av någon annan");
        throw new Error("Gick inte att ta bort platsen (den kan vara kopplad till ett kurstillfälle)");
      }

      fetchLocations();
    } catch (err) {
      alert(`Fel vid radering: ${err.message}`);
    }
  };

  if (loading) return <div className="p-10 text-center">Letar efter lediga träd... 🐿️</div>;
  if (error) return <div className="p-10 text-red-500">Fel: {error}</div>;

  return (
    <div className="content-container">
      
      {/* VÄNSTER: LISTA PÅ ORTER */}
      <div className="list-section">
        <h3>Våra Kursorter</h3>
        
        {locations.length === 0 ? (
          <div className="empty-state-box">
            <p>Här fanns inga platser ännu! 🐿️</p>
          </div>
        ) : (
          <ul className="data-list">
            {locations.map((loc) => (
              <li key={loc.id} className="data-list-item">
                <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                  <MapPin size={18} color="#ea580c" />
                  <span className="item-name">{loc.name}</span>
                </div>
                
                <button 
                  className="btn-delete"
                  onClick={() => handleDelete(loc.id, loc.rowVersion)}
                  title="Ta bort plats"
                >
                  <Trash2 size={16} />
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* HÖGER: FORMULÄR */}
      <div className="form-container">
        <h3>Lägg till ny ort</h3>
        <p style={{fontSize: '0.8rem', color: '#666', marginBottom: '15px'}}>
          Var vill du att vi ska gömma nästa kurs?
        </p>
        
        <form onSubmit={handleSubmit} className="course-form">
          <div className="form-group">
            <label htmlFor="locationName">Ortens namn</label>
            <input 
              type="text" 
              id="locationName"
              value={newName}
              onChange={(e) => setNewName(e.target.value)}
              placeholder="t.ex. Uddevalla eller Skogen..."
              required 
            />
          </div>

          <button type="submit" className="btn-add-course" style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '8px' }}>
            <PlusCircle size={18} />
            Säkra platsen
          </button>
        </form>

        <div className="info-card" style={{ marginTop: '20px', background: '#f8fafc' }}>
          <small style={{ color: '#64748b' }}>
            <strong>Tips:</strong> Innan du raderar en ort, se till att flytta alla inplanerade kurstillfällen till en annan plats!
          </small>
        </div>
      </div>
    </div>
  );
};

export default Locations;