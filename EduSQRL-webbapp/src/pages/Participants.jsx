import React from 'react'
import { mockData } from '../data/mockData' //import mock data instead of API
import {useState, useEffect} from 'react'


const Participants = () => {
    
    //set states

    const [students, setStudents] = useState([]);
    const [studentData, setStudentData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: ''
  });


  // form submit handling

    const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Ny student att registrera:", studentData);
    
    alert(`Studenten ${studentData.firstName} ${studentData.lastName} har registrerats!`);
    
    // reset form
    setStudentData({ firstName: '', lastName: '', email: '', phone: '' });
    };

    useEffect(() => {
   
    // Change "setStudents(mockData.participants)" later and fetch from API()
     setStudents(mockData.participants);
      }, []);

//Map students by email, print name and city
  return (
    <div className="content-container">
      {/* VÄNSTER SIDA: LISTAN */}
      <div className="list-section">
        <h3>Deltagare</h3>
        <ul className="data-list">
          {students.map(student => (
            // Här lägger vi till klassen för själva kortet
            <li key={student.email} className="data-list-item">
              <div>
                <span className="item-name">
                  {student.firstName} {student.lastName}
                </span>
                <br />
                <span className="item-info">{student.email}</span>
              </div>
              {/* Telefonnummer som info till höger */}
              <span className="item-info">{student.phone}</span>
            </li>
          ))}
        </ul>
      </div>

      <div className="form-container">
      {/* RUBRIK FÖR FORMULÄRET */}
      <h3>Registrera ny student</h3>
      
      <form onSubmit={handleSubmit} className="course-form">
        
        {/* FÖRMANMN */}
        <div className="form-group">
          <label htmlFor="firstName">Förnamn</label>
          <input 
            type="text" 
            id="firstName"
            value={studentData.firstName}
            onChange={(e) => setStudentData({...studentData, firstName: e.target.value})}
            placeholder="Skriv förnamn..."
            required 
          />
        </div>

        {/* EFTERNAMN */}
        <div className="form-group">
          <label htmlFor="lastName">Efternamn</label>
          <input 
            type="text" 
            id="lastName"
            value={studentData.lastName}
            onChange={(e) => setStudentData({...studentData, lastName: e.target.value})}
            placeholder="Skriv efternamn..."
            required 
          />
        </div>

        {/* E-POST - Använder type="email" för automatisk validering av @-tecken */}
        <div className="form-group">
          <label htmlFor="email">E-postadress</label>
          <input 
            type="email" 
            id="email"
            value={studentData.email}
            onChange={(e) => setStudentData({...studentData, email: e.target.value})}
            placeholder="namn@exempel.se"
            required 
          />
        </div>

        {/* TELEFONNUMMER - Använder type="tel" */}
        <div className="form-group">
          <label htmlFor="phone">Telefonnummer</label>
          <input 
            type="tel" 
            id="phone"
            value={studentData.phone}
            onChange={(e) => setStudentData({...studentData, phone: e.target.value})}
            placeholder="070-000 00 00"
            required 
          />
        </div>

        {/* KNAPP FÖR ATT SKICKA IN FORMULÄRET */}
        <button type="submit" className="btn-add-course">
          Registrera student
        </button>
      </form>
    </div>
    </div>
  )
}

export default Participants