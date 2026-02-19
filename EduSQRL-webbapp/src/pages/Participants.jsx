import React, { useState, useEffect } from 'react';

const Participants = () => {
  const [participants, setParticipants] = useState([]);
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    roleId: ''
  });

  const BASE_URL = 'https://localhost:7054';

  // 1. FLYTTA UT fetchData HÄR (så att både useEffect och handleDelete kan nå den)
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
    if (!window.confirm("Vill du verkligen ta bort denna deltagare?")) {
      return;
    }

    try {
      const response = await fetch(`${BASE_URL}/participants/${id}`, {
        method: 'DELETE',
        headers: {
          'If-Match': rowVersion // Sigillet i headern!
        }
      });

      if (!response.ok) {
        if (response.status === 409) {
          throw new Error("Kunde inte radera: Deltagaren har ändrats nyss. Ladda om sidan.");
        }
        throw new Error("Gick inte att radera deltagaren");
      }

      alert("Deltagaren har raderats.");
      fetchData(); // Nu funkar detta anrop!

    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

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
          <div className="empty-state-box">
            <p>Här var det tomt! 🐿️</p>
            <span>Lägg till nya deltagare i formuläret till höger för att fylla förrådet.</span>
          </div>
        ) : (

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
                <span className="item-info">{student.phoneNumber}</span>

                {/* RÄTTAT: Vi använder student.id och student.rowVersion här! */}
                <button
                  className="btn-delete"
                  onClick={() => handleDelete(student.id, student.rowVersion)}
                >
                  Ta bort
                </button>
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
              value={formData.firstName}
              onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
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
              onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
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
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
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
              onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })}
              placeholder="070-000 00 00"
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

          <button type="submit" className="btn-add-course">
            Registrera deltagare
          </button>
        </form>
      </div>
    </div>
  );
}

export default Participants;