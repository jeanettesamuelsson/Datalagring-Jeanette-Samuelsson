//Mockdata istället för att anropa API:et

export const mockData = {
  participants: [
    { email: 'nettes@edu.se', firstName: 'Nette', lastName: 'Samuelsson', phoneNumber: '0701112233' },
    { email: 'egon@sqrl.se', firstName: 'Egon', lastName: 'Ekorre', phoneNumber: '070998877' }
  ],
  
  courses: [
    { courseCode: 101, name: 'C# Basics', description: 'Lär dig grunderna i C#' },
    { courseCode: 102, name: 'React & Frontend', description: 'Bygg snygga UI:n' }
  ],

  teachers: [
    { email: 'boss@edu.se', firstName: 'Bertil', lastName: 'Olofsson', expertiseArea: 'Backend' }
  ],

  // Lägger till läraren direkt i instansen
  courseInstances: [
    { 
      instanceId: 1, 
      startDate: '2026-03-01', 
      endDate: '2026-06-01', 
      capacity: 20, 
      courseCode: 101, 
      teacherEmail: 'boss@edu.se',
      cityName: 'Uddevalla' 
    }
  ],

  //aktiv registrering
  registrations: [
    { registrationId: 1, status: 'Active', registrationDate: '2026-01-20', email: 'nettes@edu.se', instanceId: 1 }
  ]
};