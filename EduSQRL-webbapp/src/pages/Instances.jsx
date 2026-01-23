import React, { useState } from 'react';
import { mockData } from '../data/mockData';

const Instances = () => {

  const [instanceData, setInstanceData] = useState({
    courseId: '',
    startDate: '',
    endDate: '',
    capacity: '',
    teacherId: ''
  });

const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Nytt tillfälle att spara:", instanceData);
    
    // Hitta kursens namn för en trevligare bekräftelse
    const selectedCourse = mockData.courses.find(c => c.id === parseInt(instanceData.courseId));
    alert(`Tillfälle för ${selectedCourse?.name} har lagts till!`);
    
    // Återställ formulär
    setInstanceData({ courseId: '', startDate: '', endDate: '', capacity: '', teacherId: '' });
  };


  return (
    <div>
      <h3> Kommande kurstillfällen:
        </h3>



<div className="form-container">
      <h3>Planera nytt kurstillfälle</h3>
      <form onSubmit={handleSubmit} className="course-form">
        
        {/* VÄLJ KURS */}
        <div className="form-group">
          <label>Välj kurs (Kurskod)</label>
          <select 
            required
            value={instanceData.courseId}
            onChange={(e) => setInstanceData({...instanceData, courseId: e.target.value})}
          >
            <option value="">-- Välj en kurs --</option>
            {mockData.courses.map(course => (
              <option key={course.id} value={course.id}>
                {course.courseCode} - {course.name}
              </option>
            ))}
          </select>
        </div>

        {/* DATUM-RAD */}
        <div className="form-row">
          <div className="form-group">
            <label>Startdatum</label>
            <input 
              type="date" 
              required
              value={instanceData.startDate}
              onChange={(e) => setInstanceData({...instanceData, startDate: e.target.value})}
            />
          </div>
          <div className="form-group">
            <label>Slutdatum</label>
            <input 
              type="date" 
              required
              value={instanceData.endDate}
              onChange={(e) => setInstanceData({...instanceData, endDate: e.target.value})}
            />
          </div>
        </div>

        {/* ANTAL PLATSER */}
        <div className="form-group">
          <label>Antal platser</label>
          <input 
            type="number" 
            placeholder="t.ex. 20"
            min="1"
            required
            value={instanceData.capacity}
            onChange={(e) => setInstanceData({...instanceData, capacity: e.target.value})}
          />
        </div>

        {/* VÄLJ LÄRARE */}
        <div className="form-group">
          <label>Ansvarig lärare</label>
          <select 
            required
            value={instanceData.teacherId}
            onChange={(e) => setInstanceData({...instanceData, teacherId: e.target.value})}
          >
            <option value="">-- Välj lärare --</option>
            {mockData.teachers.map(teacher => (
              <option key={teacher.id} value={teacher.id}>
                {teacher.firstName} {teacher.lastName}
              </option>
            ))}
          </select>
        </div>

        <button type="submit" className="btn-add-course">
          Skapa tillfälle
        </button>
      </form>
    </div>



        </div>

        




  )
}

export default Instances