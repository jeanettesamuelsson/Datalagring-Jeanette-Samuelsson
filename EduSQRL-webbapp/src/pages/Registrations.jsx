import React, { useState, useEffect } from 'react';
import { mockData } from '../data/mockData';

const Registrations = () => {
  const [registrations, setRegistrations] = useState([]);
  const [formData, setFormData] = useState({
    email: '', // Ändrat från studentId till email
    instanceId: '',
    regDate: new Date().toISOString().split('T')[0] 
  });

  useEffect(() => {
    // Vi hämtar de befintliga registreringarna från mockData vid start
    setRegistrations(mockData.registrations || []); 
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Vi letar upp rätt person och kurs med de nya namnen
    const student = mockData.participants.find(p => p.email === formData.email);
    const instance = mockData.courseInstances.find(i => i.instanceId === parseInt(formData.instanceId));
    const course = mockData.courses.find(c => c.courseCode === instance?.courseCode);

    const newReg = {
      registrationId: registrations.length + 1,
      studentName: `${student?.firstName} ${student?.lastName}`,
      courseName: course?.name,
      regDate: formData.regDate
    };

    setRegistrations([...registrations, newReg]);
    alert(`Ekorr-post skickad! ${student?.firstName} är nu registrerad.`);
    
    setFormData({ email: '', instanceId: '', regDate: new Date().toISOString().split('T')[0] });
  };

  return (
    <div className="content-container">
      
      {/* VÄNSTER: LISTA PÅ REGISTRERINGAR */}
      <div className="list-section">
        <h3>Genomförda registreringar</h3>
        <ul className="data-list">
          {registrations.map((reg, index) => (
            <li key={reg.registrationId || index} className="data-list-item">
              <div>
                <span className="item-name">{reg.studentName || "Okänd student"}</span>
                <br />
                <span className="item-info">{reg.courseName || "Okänd kurs"}</span>
              </div>
              <span className="item-info">{reg.regDate || reg.registrationDate}</span>
            </li>
          ))}
        </ul>
      </div>

      {/* HÖGER: FORMULÄRET */}
      <div className="form-container">
        <h3>Ny kursregistrering</h3>
        <form onSubmit={handleSubmit} className="course-form">
          
          <div className="form-group">
            <label>Välj student</label>
            <select 
              required 
              value={formData.email}
              onChange={(e) => setFormData({...formData, email: e.target.value})}
            >
              <option value="">-- Välj student --</option>
              {/* ÄNDRAT: Använder participants och email */}
              {mockData.participants?.map(p => (
                <option key={p.email} value={p.email}>
                  {p.firstName} {p.lastName} ({p.email})
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Välj kurstillfälle</label>
            <select 
              required 
              value={formData.instanceId}
              onChange={(e) => setFormData({...formData, instanceId: e.target.value})}
            >
              <option value="">-- Välj tillfälle --</option>
              {/* ÄNDRAT: Använder courseInstances */}
              {mockData.courseInstances?.map(i => {
                const course = mockData.courses.find(c => c.courseCode === i.courseCode);
                return (
                  <option key={i.instanceId} value={i.instanceId}>
                    {course?.name} - {i.cityName} ({i.startDate})
                  </option>
                );
              })}
            </select>
          </div>

          <div className="form-group">
            <label>Datum</label>
            <input 
              type="date" 
              value={formData.regDate}
              onChange={(e) => setFormData({...formData, regDate: e.target.value})}
            />
          </div>

          <button type="submit" className="btn-add-course">Registrera på kurs</button>
        </form>
      </div>
    </div>
  );
};

export default Registrations;