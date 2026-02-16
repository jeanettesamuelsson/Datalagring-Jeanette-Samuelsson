import React, { useState, useEffect } from 'react';

const Participants = () => {
  // States för data från API
  const [participants, setParticipants] = useState([]);
  const [roles, setRoles] = useState([]);
  
  // States för UI-status
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Form state - notera 'phoneNumber' och 'roleId' för att matcha din C# DTO
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    roleId: ''
  });

  const BASE_URL = 'https://localhost:7054';

  // 1. Hämta deltagare och roller när komponenten laddas
  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        // Vi hämtar både deltagare och roller parallellt
        const [partRes, roleRes] = await Promise.all([
          fetch(`${BASE_URL}/participants`),
          fetch(`${BASE_URL}/roles`)
        ]);

        if (!partRes.ok || !roleRes.ok) throw new Error("Kunde inte hämta data från servern");

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

    fetchData();
  }, []);

  // 2. Hantera inskickning av formulär (POST)
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    try {
      const response = await fetch(`${BASE_URL}/participants`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || "Kunde inte spara deltagaren");
      }

      // Om det lyckas: hämta den uppdaterade listan
      const updatedList = await fetch(`${BASE_URL}/participants`).then(res => res.json());
      setParticipants(updatedList);

      alert(`Deltagaren ${formData.firstName} har registrerats!`);
      
      // Töm formuläret
      setFormData({ firstName: '', lastName: '', email: '', phoneNumber: '', roleId: '' });
      
    } catch (err) {
      alert(`Fel: ${err.message}`);
    }
  };

  if (loading) return <div className="p-10 text-center">Letar efter ekorrar... 🐿️</div>;
  if (error) return <div className="p-10 text-red-500">Hoppsan: {error}</div>;

  return (
    <div className="content-container">
      {/* VÄNSTER SIDA: LISTAN */}
      <div className="list-section">
        <h3>Deltagare</h3>
        <ul className="data-list">
          {participants.map(student => (
            <li key={student.id} className="data-list-item">
              <div>
                <span className="item-name">
                  {student.firstName} {student.lastName}
                </span>
                <br />
                <span className="item-info">{student.email}</span>
              </div>
              {/* Visar telefonnummer till höger */}
              <span className="item-info">{student.phoneNumber}</span>
            </li>
          ))}
        </ul>
      </div>

      {/* HÖGER SIDA: FORMULÄRET */}
      <div className="form-container">
        <h3>Registrera ny deltagare</h3>
        
        <form onSubmit={handleSubmit} className="course-form">
          <div className="form-group">
            <label htmlFor="firstName">Förnamn</label>
            <input 
              type="text" 
              id="firstName"
              value={formData.firstName}
              onChange={(e) => setFormData({...formData, firstName: e.target.value})}
              placeholder="Skriv förnamn..."
              required 
            />
          </div>

          <div className="form-group">
            <label htmlFor="lastName">Efternamn</label>
            <input 
              type="text" 
              id="lastName"
              value={formData.lastName}
              onChange={(e) => setFormData({...formData, lastName: e.target.value})}
              placeholder="Skriv efternamn..."
              required 
            />
          </div>

          <div className="form-group">
            <label htmlFor="email">E-postadress</label>
            <input 
              type="email" 
              id="email"
              value={formData.email}
              onChange={(e) => setFormData({...formData, email: e.target.value})}
              placeholder="namn@exempel.se"
              required 
            />
          </div>

          <div className="form-group">
            <label htmlFor="phone">Telefonnummer</label>
            <input 
              type="tel" 
              id="phone"
              value={formData.phoneNumber}
              onChange={(e) => setFormData({...formData, phoneNumber: e.target.value})}
              placeholder="070-000 00 00"
              required 
            />
          </div>

          {/* NYTT: Roll-väljare (Nödvändigt för API:et) */}
          <div className="form-group">
            <label htmlFor="role">Roll</label>
            <select 
              id="role"
              required
              value={formData.roleId}
              onChange={(e) => setFormData({...formData, roleId: e.target.value})}
            >
              <option value="">-- Välj roll --</option>
              {roles.map(role => (
                <option key={role.id} value={role.id}>
                  {role.roleName}
                </option>
              ))}
            </select>
          </div>

          <button type="submit" className="btn-add-course">
            Registrera deltagare
          </button>
        </form>
      </div>
    </div>
  );
}

export default Participants;