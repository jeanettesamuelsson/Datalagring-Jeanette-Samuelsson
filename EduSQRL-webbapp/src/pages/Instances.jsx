import React, { useState, useEffect } from 'react';
import { mockData } from '../data/mockData';

const Instances = () => {

  //set states
 
  const [instances, setInstances] = useState([]);
  const [instanceData, setInstanceData] = useState({
    courseCode: '',    
    startDate: '',
    endDate: '',
    capacity: '',
    teacherEmail: '',  
    cityName: ''       
  });

   //get mock up data                           //Remove and get from database
  useEffect(() => {
    setInstances(mockData.courseInstances || []);
  }, []);


    //handle form 

  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Hitta kursnamnet för bekräftelsen med courseCode
    const selectedCourse = mockData.courses.find(c => c.courseCode === parseInt(instanceData.courseCode));
    
    const newInstance = {
      ...instanceData,
      instanceId: instances.length + 1, // mockup ID generator
      courseName: selectedCourse?.name
    };

    setInstances([...instances, newInstance]);
    alert(`Nytt tillfälle för ${selectedCourse?.name} i ${instanceData.cityName} har skapats!`);
    
    // reset form

    setInstanceData({ courseCode: '', startDate: '', endDate: '', capacity: '', teacherEmail: '', cityName: '' });
  };

  return (
    <div className="content-container">
      
      {/* List of course instances */}

      <div className="list-section">
        <h3>Kommande kurstillfällen</h3>
        <ul className="data-list">
          {instances.map((inst, index) => {
            const course = mockData.courses.find(c => c.courseCode === inst.courseCode);
            return (
              <li key={inst.instanceId || index} className="data-list-item">
                <div>
                  <span className="item-name">{course?.name || inst.courseName}</span>
                  <br />
                  <span className="item-info">{inst.cityName} | Start: {inst.startDate}</span>
                </div>
                <div style={{ textAlign: 'right' }}>
                   <span className="expertise-tag">{inst.capacity} platser</span>
                </div>
              </li>
            );
          })}
        </ul>
      </div>

      {/* Form */}

      <div className="form-container">
        <h3>Planera nytt kurstillfälle</h3>
        <form onSubmit={handleSubmit} className="course-form">
          
          
          <div className="form-group">
            <label>Kurs</label>
            <select 
              required
              value={instanceData.courseCode}
              onChange={(e) => setInstanceData({...instanceData, courseCode: e.target.value})}
            >
              <option value="">-- Välj en kurs --</option>
              {mockData.courses.map(course => (
                <option key={course.courseCode} value={course.courseCode}>
                  {course.courseCode} - {course.name}
                </option>
              ))}
            </select>
          </div>

          
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

          
          
            <div className="form-group">
              <label>Stad</label>
              <input 
                type="text"
                required
                value={instanceData.cityName}
                onChange={(e) => setInstanceData({...instanceData, cityName: e.target.value})}
              />
            
            <div className="form-group">
              <label>Antal platser</label>
              <input 
                type="number" 
                min="1"
                required
                value={instanceData.capacity}
                onChange={(e) => setInstanceData({...instanceData, capacity: e.target.value})}
              />
            </div>
          </div>

        
          <div className="form-group">
            <label>Ansvarig lärare</label>
            <select 
              required
              value={instanceData.teacherEmail}
              onChange={(e) => setInstanceData({...instanceData, teacherEmail: e.target.value})}
            >
              <option value="">-- Välj lärare --</option>
              {mockData.teachers.map(teacher => (
                <option key={teacher.email} value={teacher.email}>
                  {teacher.firstName} {teacher.lastName} ({teacher.expertiseArea})
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
  );
};

export default Instances;