import React from 'react'
import {useState, useEffect} from 'react'
import { mockData } from '../data/mockData' //import mock data instead of API

const Teachers = () => {

//add states 

const [teachers, setTeachers] = useState([]);
  const [teacherData, setTeacherData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    expertiseArea: ''
  });

 
  //handle subbit
  const handleSubmit = (e) => {
    e.preventDefault();
    
    //add teacher to list and clear form
    
    setTeachers([...teachers, teacherData]);
    setTeacherData({ firstName: '', lastName: '', email: '', expertiseArea: '' });
  };

    useEffect(() => {
     
      // Change "setTeachers(mockData.teachers)" later and fetch from API()
       setTeachers(mockData.teachers);
        }, []);

  return (
    
   <div className="content-container">
      
      {/* Teacher list */}
      <div className="list-section">
        <h3>Våra lärare</h3>
        <ul className="data-list">
          {teachers.map((teacher, index) => (
            <li key={teacher.email || index} className="data-list-item">
              <div>
                <span className="item-name">
                  {teacher.firstName} {teacher.lastName}
                </span>
                <br />
                <span className="item-info">{teacher.email}</span>
              </div>
              
              <span className="expertise-tag">{teacher.expertiseArea}</span>
            </li>
          ))}
        </ul>
      </div>

      {/* Form */}
      <div className="form-container">
        <h3>Registrera ny lärare</h3>
        <form onSubmit={handleSubmit} className="course-form">
          
          <div className="form-group">
            <label htmlFor="firstName">Förnamn</label>
            <input 
              type="text" 
              id="firstName"
              value={teacherData.firstName}
              onChange={(e) => setTeacherData({...teacherData, firstName: e.target.value})}
              placeholder="Skriv förnamn..."
              required 
            />
          </div>

          <div className="form-group">
            <label htmlFor="lastName">Efternamn</label>
            <input 
              type="text" 
              id="lastName"
              value={teacherData.lastName}
              onChange={(e) => setTeacherData({...teacherData, lastName: e.target.value})}
              placeholder="Skriv efternamn..."
              required 
            />
          </div>

          <div className="form-group">
            <label htmlFor="email">E-postadress</label>
            <input 
              type="email" 
              id="email"
              value={teacherData.email}
              onChange={(e) => setTeacherData({...teacherData, email: e.target.value})}
              placeholder="namn.efternamn@edusqrl.se"
              required 
            />
          </div>

          <div className="form-group">
            <label htmlFor="expertise">Expertisområde</label>
            <input 
              type="text" 
              id="expertise"
              value={teacherData.expertiseArea}
              onChange={(e) => setTeacherData({...teacherData, expertiseArea: e.target.value})}
              placeholder="t.ex. SQL, React, Python..."
              required 
            />
          </div>

          <button type="submit" className="btn-add-course">
            Spara lärare
          </button>
        </form>
      </div>
    </div>
  )
}

export default Teachers