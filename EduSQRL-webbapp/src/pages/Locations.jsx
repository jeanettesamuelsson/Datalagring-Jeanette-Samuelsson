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

  // --- update function ---
  const handleUpdate = async (e) => {

    e.preventDefault();
   
    try {
      const response = await fetch(`${API_URL}/${editingLocation.id}`, {
      method: 'PUT',
      headers: { 
        'Content-Type': 'application/json',
        'If-Match': editingLocation.rowVersion 
      },
      body: JSON.stringify(editingCourse)
    });

      if (!response.ok) {
      if (response.status === 412 || response.status === 409) {
        throw new Error("Platsen har ändrats av någon annan. Ladda om sidan.");
      }
      throw new Error("Det gick inte att uppdatera platsen");
    }

    setEditingCourse(null); // close modal
    fetchCourses();         // update list
    alert("Platsen är uppdaterad! 🐿️");
  } catch (err) {
    alert(`Hoppsan: ${err.message}`);
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
      
      {/* LISTA PÅ ORTER */}
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

                 {/* Edit */}
              <button 
                className="btn-edit"
                onClick={() => setEditingCourse(course)} 
                 title="Redigera kurs"
               >
                  ✎
               </button>
                
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

       {/* Modal update*/}
{editingCourse && (
  <div className="modal-overlay">
    <div className="modal-content">
      <h3>Redigera kurs: {editingCourse.courseName}</h3>
      <form onSubmit={handleUpdate}>
        <div className="form-group">
          <label>Kursens namn</label>
          <input 
            type="text" 
            value={editingCourse.courseName}
            onChange={(e) => setEditingCourse({...editingCourse, courseName: e.target.value})}
            required 
          />
        </div>
        <div className="form-group">
          <label>Kurskod</label>
          <input 
            type="text" 
            value={editingCourse.courseCode}
            onChange={(e) => setEditingCourse({...editingCourse, courseCode: e.target.value})}
            required 
          />
        </div>
        <div className="form-group">
          <label>Beskrivning</label>
          <textarea 
            value={editingCourse.description}
            onChange={(e) => setEditingCourse({...editingCourse, description: e.target.value})}
            rows="3"
            required
          ></textarea>
        </div>
        <div className="modal-buttons">
          <button type="submit" className="btn-save">Spara ändringar</button>
          <button type="button" className="btn-cancel" onClick={() => setEditingCourse(null)}>Avbryt</button>
        </div>
      </form>
    </div>
  </div>
)}



    </div>
  );
};

export default Locations;