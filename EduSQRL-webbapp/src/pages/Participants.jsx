import React, { useState, useEffect } from 'react';

const Participants = () => {
  const [participants, setParticipants] = useState([]);
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // State för modalen
  const [editingParticipant, setEditingParticipant] = useState(null);

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    roleId: ''
  });

  const BASE_URL = 'https://localhost:7054';

  const fetchData = async () => {
    try {
      setLoading(true);
      const [partRes, roleRes] = await Promise.all([
        fetch(`${BASE_URL}/participants`),
        fetch(`${BASE_URL}/roles`)
      ]);

      if (!partRes.ok || !roleRes.ok) throw new Error("Kunde inte hämta data");

      const partData = await partRes.json();
      const roleData = await roleRes.json();

      setParticipants(partData);
      setRoles(roleData);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // --- RADERA DELTAGARE ---
  const handleDelete = async (id, rowVersion) => {
    if (!window.confirm("Vill du verkligen ta bort denna deltagare?")) return;

    try {
      const response = await fetch(`${BASE_URL}/participants/${id}`, {
        method: 'DELETE',
        headers: { 'If-Match': rowVersion }
      });

      if (!response.ok) {
        if (response.status === 409) throw new Error("Konflikt: Deltagaren har ändrats nyss.");
        throw new Error("Gick inte att radera deltagaren");
      }

      alert("Deltagaren har raderats.");
      fetchData();
    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  // --- UPPDATERA DELTAGARE ---
  const handleUpdate = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`${BASE_URL}/participants/${editingParticipant.id}`, {
        method: 'PUT',
        headers: { 
          'Content-Type': 'application/json',
          'If-Match': editingParticipant.rowVersion 
        },
        body: JSON.stringify(editingParticipant)
      });

      if (!response.ok) {
        if (response.status === 412 || response.status === 409) {
          throw new Error("Deltagaren har ändrats av någon annan. Ladda om sidan.");
        }
        throw new Error("Det gick inte att uppdatera deltagaren");
      }

      setEditingParticipant(null); // Stäng modal
      fetchData();                 // Uppdatera listan
      alert("Deltagaren är uppdaterad! 🐿️");
    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  // --- SKAPA NY DELTAGARE ---
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`${BASE_URL}/participants`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      if (!response.ok) throw new Error("Kunde inte spara deltagaren");

      alert(`Deltagaren ${formData.firstName} har registrerats!`);
      setFormData({ firstName: '', lastName: '', email: '', phoneNumber: '', roleId: '' });
      fetchData();
    } catch (err) {
      alert(`Fel: ${err.message}`);
    }
  };

  if (loading) return <div className="p-10 text-center">Letar efter ekorrar... 🐿️</div>;
  if (error) return <div className="p-10 text-red-500">Hoppsan: {error}</div>;

  return (
    <div className="content-container">
      <div className="list-section">
        <h3>Deltagare</h3>
        {participants.length === 0 ? (
          <div className="empty-state-box"><p>Här var det tomt! 🐿️</p></div>
        ) : (
          <ul className="data-list">
            {participants.map(student => (
              <li key={student.id} className="data-list-item">
                <div>
                  <span className="item-name">{student.firstName} {student.lastName}</span>
                  <br />
                  <span className="item-info">{student.email}</span>
                </div>
                
                <div className="item-actions">
                  <button className="btn-edit" onClick={() => setEditingParticipant(student)} title="Redigera">
                    ✎
                  </button>
                  <button className="btn-delete" onClick={() => handleDelete(student.id, student.rowVersion)}>
                    Ta bort
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </div>

      <div className="form-container">
  <h3>Registrera ny deltagare</h3>
  <form onSubmit={handleSubmit} className="course-form">
    
    <div className="form-group">
      <label htmlFor="firstName">Förnamn</label>
      <input 
        type="text" 
        id="firstName"
        placeholder="Förnamn" 
        value={formData.firstName} 
        onChange={(e) => setFormData({ ...formData, firstName: e.target.value })} 
        required 
      />
    </div>

    <div className="form-group">
      <label htmlFor="lastName">Efternamn</label>
      <input 
        type="text" 
        id="lastName"
        placeholder="Efternamn" 
        value={formData.lastName} 
        onChange={(e) => setFormData({ ...formData, lastName: e.target.value })} 
        required 
      />
    </div>

    <div className="form-group">
      <label htmlFor="email">E-post</label>
      <input 
        type="email" 
        id="email"
        placeholder="E-post" 
        value={formData.email} 
        onChange={(e) => setFormData({ ...formData, email: e.target.value })} 
        required 
      />
    </div>

    <div className="form-group">
      <label htmlFor="phone">Telefon</label>
      <input 
        type="tel" 
        id="phone"
        placeholder="Telefon" 
        value={formData.phoneNumber} 
        onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })} 
        required 
      />
    </div>

    <div className="form-group">
      <label htmlFor="role">Roll</label>
      <select 
        id="role"
        required 
        value={formData.roleId} 
        onChange={(e) => setFormData({ ...formData, roleId: e.target.value })}
      >
        <option value="">-- Välj roll --</option>
        {roles.map(role => (
          <option key={role.id} value={role.id}>
            {role.roleName}
          </option>
        ))}
      </select>
    </div>

    <button type="submit" className="btn-add-course">Registrera</button>
  </form>
</div>

      {/* --- MODAL UPDATE --- */}
      {editingParticipant && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Redigera: {editingParticipant.firstName}</h3>
            <form onSubmit={handleUpdate} className="course-form">
              <div className="form-group">
                <label>Förnamn</label>
                <input 
                  type="text" 
                  value={editingParticipant.firstName}
                  onChange={(e) => setEditingParticipant({...editingParticipant, firstName: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Efternamn</label>
                <input 
                  type="text" 
                  value={editingParticipant.lastName}
                  onChange={(e) => setEditingParticipant({...editingParticipant, lastName: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>E-post</label>
                <input 
                  type="email" 
                  value={editingParticipant.email}
                  onChange={(e) => setEditingParticipant({...editingParticipant, email: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Telefon</label>
                <input 
                  type="tel" 
                  value={editingParticipant.phoneNumber}
                  onChange={(e) => setEditingParticipant({...editingParticipant, phoneNumber: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Roll</label>
                <select 
                  value={editingParticipant.roleId} 
                  onChange={(e) => setEditingParticipant({...editingParticipant, roleId: e.target.value})}
                  required
                >
                  {roles.map(role => <option key={role.id} value={role.id}>{role.roleName}</option>)}
                </select>
              </div>
              <div className="modal-buttons">
                <button type="submit" className="btn-save">Spara ändringar</button>
                <button type="button" className="btn-cancel" onClick={() => setEditingParticipant(null)}>Avbryt</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default Participants;