import React, { useState, useEffect } from 'react';

const Courses = () => {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editingCourse, setEditingCourse] = useState(null);

  const [courseData, setCourseData] = useState({
    courseName: '',
    courseCode: '',
    description: ''
  });

  const API_URL = 'https://localhost:7054/api/courses';

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

  // --- update function ---
  const handleUpdate = async (e) => {

    e.preventDefault();
   
    try {
      const response = await fetch(`${API_URL}/${editingCourse.id}`, {
      method: 'PUT',
      headers: { 
        'Content-Type': 'application/json',
        'If-Match': editingCourse.rowVersion 
      },
      body: JSON.stringify(editingCourse)
    });

      if (!response.ok) {
      if (response.status === 412 || response.status === 409) {
        throw new Error("Kursen har ändrats av någon annan. Ladda om sidan.");
      }
      throw new Error("Det gick inte att uppdatera kursen");
    }

    setEditingCourse(null); // close modal
    fetchCourses();         // update list
    alert("Kursen är uppdaterad! 🐿️");
  } catch (err) {
    alert(`Hoppsan: ${err.message}`);
  }
};


  // --- delete function ---
  const handleDelete = async (id, rowVersion) => {
    if (!window.confirm("Är du säker på att du vill ta bort den här kursen ur förrådet?")) {
      return;
    }

    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
        headers: { 'If-Match': rowVersion },
       
      });

      if (!response.ok) {
        if (response.status === 409) {
          throw new Error("Kunde inte radera: Kursen har ändrats av någon annan. Ladda om sidan.");
        }
        throw new Error("Gick inte att radera kursen");
      }

      // Uupdate list after delete
      fetchCourses();
    } catch (err) {
      alert(`Hoppsan: ${err.message}`);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(courseData)
      });

      if (!response.ok) throw new Error("Gick inte att spara kursen");

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
      
      {/* List of courses */}
      <div className="list-section">
        <h3>Befintliga kurser</h3>

          {courses.length === 0 ? (
          <div className="empty-state-box">
            <p>Här var det tomt! 🐿️</p>
            <span>Lägg till nya kurser i formuläret till höger för att fylla förrådet.</span>
          </div>
        ) : (

        <ul className="data-list">
          {courses.map((course) => (
            <li key={course.id} className="data-list-item">
              <div>
                <span className="item-name">{course.courseName}</span>
                <br />
                <span className="item-info">{course.courseCode}</span>
              </div>
              
              <div style={{ maxWidth: '150px' }}>
                <p className="item-info" style={{ fontSize: '0.75rem', fontStyle: 'italic' }}>
                  {course.description}
                </p>
              </div>

             {/* Edit */}
              <button 
                className="btn-edit"
                onClick={() => setEditingCourse(course)} 
                 title="Redigera kurs"
               >
                  ✎
               </button>

              {/*delete btn*/}
              <button 
                className="btn-delete"
                onClick={() => handleDelete(course.id, course.rowVersion)}
              >
                Ta bort
              </button>
            </li>
          ))}
        </ul>
        )}
      </div>

      {/* form */}
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
              placeholder="Kort beskrivning..."
              rows="3"
              required
            ></textarea>
          </div>

          <button type="submit" className="btn-add-course">
            Lägg till kurs
          </button>
        </form>
      </div>

      {/* Modal update*/}
{editingCourse && (
  <div className="modal-overlay">
    <div className="modal-content">
      <h3>Redigera kurs: {editingCourse.courseName}</h3>
      <form onSubmit={handleUpdate}>
        <div className="form-group">
          <label>Kursens namn</label>
          <input 
            type="text" 
            value={editingCourse.courseName}
            onChange={(e) => setEditingCourse({...editingCourse, courseName: e.target.value})}
            required 
          />
        </div>
        <div className="form-group">
          <label>Kurskod</label>
          <input 
            type="text" 
            value={editingCourse.courseCode}
            onChange={(e) => setEditingCourse({...editingCourse, courseCode: e.target.value})}
            required 
          />
        </div>
        <div className="form-group">
          <label>Beskrivning</label>
          <textarea 
            value={editingCourse.description}
            onChange={(e) => setEditingCourse({...editingCourse, description: e.target.value})}
            rows="3"
            required
          ></textarea>
        </div>
        <div className="modal-buttons">
          <button type="submit" className="btn-save">Spara ändringar</button>
          <button type="button" className="btn-cancel" onClick={() => setEditingCourse(null)}>Avbryt</button>
        </div>
      </form>
    </div>
  </div>
)}



    </div>
  );
};

export default Courses;