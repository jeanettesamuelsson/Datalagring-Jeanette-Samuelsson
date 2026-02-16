import React, { useState, useEffect } from 'react';

const Courses = () => {
  // States för data
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Form state - mappar mot din CreateCourseInput i C#
  const [courseData, setCourseData] = useState({
    courseName: '', // Matchar C# fältnamn
    courseCode: '',
    description: ''
  });

  const API_URL = 'https://localhost:7054/api/courses';

  // 1. Hämta alla kurser vid start
  useEffect(() => {
    fetchCourses();
  }, []);

  const fetchCourses = async () => {
    try {
      setLoading(true);
      const response = await fetch(API_URL);
      if (!response.ok) throw new Error("Kunde inte hämta kurser");
      const data = await response.json();
      setCourses(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // 2. Spara ny kurs (POST)
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(courseData)
      });

      if (!response.ok) throw new Error("Gick inte att spara kursen");

      alert(`Kursen ${courseData.courseName} har sparats i förrådet!`);
      
      // Rensa formulär och uppdatera listan
      setCourseData({ courseName: '', courseCode: '', description: '' });
      fetchCourses(); 

    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  if (loading) return <div className="p-10 text-center">Räknar årsringar... 🐿️</div>;
  if (error) return <div className="p-10 text-red-500">Fel: {error}</div>;

  return (
    <div className="content-container">
      
      {/* VÄNSTER: LISTA PÅ KURSER */}
      <div className="list-section">
        <h3>Befintliga kurser</h3>
        <ul className="data-list">
          {courses.map((course) => (
            <li key={course.id} className="data-list-item">
              <div>
                <span className="item-name">{course.courseName}</span>
                <br />
                <span className="item-info">{course.courseCode}</span>
              </div>
              <div style={{ maxWidth: '200px' }}>
                <p className="item-info" style={{ fontSize: '0.75rem', fontStyle: 'italic' }}>
                  {course.description}
                </p>
              </div>
            </li>
          ))}
        </ul>
      </div>

      {/* HÖGER: FORMULÄR */}
      <div className="form-container">
        <h3>Lägg till ny kurs</h3>
        <form onSubmit={handleSubmit} className="course-form">
          
          <div className="form-group">
            <label htmlFor="courseName">Kursens namn</label>
            <input 
              type="text" 
              id="courseName"
              value={courseData.courseName}
              onChange={(e) => setCourseData({...courseData, courseName: e.target.value})}
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
            Lägg till kurs
          </button>
        </form>
      </div>
    </div>
  );
};

export default Courses;