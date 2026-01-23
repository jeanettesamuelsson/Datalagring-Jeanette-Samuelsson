import React, { useState } from 'react';

const Courses = () => {

const [courseData, setCourseData] = useState({
    name: '',
    courseCode: '',
    description: ''
  });

const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Ny kurs att spara:", courseData);
    // Här kommer du senare lägga till din fetch/axios för att spara i databasen
    alert(`Kursen ${courseData.name} har lagts till!`);
    
    // Rensa formuläret efteråt
    setCourseData({ name: '', courseCode: '', description: '' });
  };


  return (
    <div className="form-container">
      <h3>Lägg till ny kurs</h3>
      <form onSubmit={handleSubmit} className="course-form">
        
        <div className="form-group">
          <label htmlFor="name">Kursens namn</label>
          <input 
            type="text" 
            id="name"
            value={courseData.name}
            onChange={(e) => setCourseData({...courseData, name: e.target.value})}
            placeholder="Skriv kursens namn..."
            required 
          />
        </div>

        <div className="form-group">
          <label htmlFor="courseCode">Kurskod</label>
          <input 
            type="text" 
            id="courseCode"
            value={courseData.courseCode}
            onChange={(e) => setCourseData({...courseData, courseCode: e.target.value})}
            placeholder="t.ex. JS-101"
            required 
          />
        </div>

        <div className="form-group">
          <label htmlFor="description">Beskrivning</label>
          <textarea 
            id="description"
            value={courseData.description}
            onChange={(e) => setCourseData({...courseData, description: e.target.value})}
            placeholder="Kort beskrivning av kursens innehåll..."
            rows="4"
            required
          ></textarea>
        </div>

        <button type="submit" className="btn-add-course">
          Lägg till
        </button>
      </form>
    </div>
  )
}

export default Courses