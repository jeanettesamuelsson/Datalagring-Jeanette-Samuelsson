import React from 'react'
import { mockData } from '../data/mockData' //import mock data instead of API
import {useState, useEffect} from 'react'


const Participants = () => {
    
    //set state
    const [students, setStudents] = useState([]);

    useEffect(() => {
   
    // Change "setStudents(mockData.participants)" later and fetch from API()
     setStudents(mockData.participants);
      }, []);

//Map students by email, print name and city
  return (
    <div>
      <h3 >Deltagare</h3>
      <ul >

        
        {students.map(student => (
          <li key={student.email} >
            {student.firstName} {student.lastName} ({student.cityName})
          </li>
        ))}
      </ul>
    </div>
  )
}

export default Participants