# 🎓 EduSQRL

**EduSQRL** is a course administration system designed to streamline the management of educational courses, locations, and participants. This project was developed as a school project for a **Data Management** course.

---

## Developed using these technologies:

### **Backend**
* **C# .NET 10** (ASP.NET Core Web API)
* **Entity Framework Core (EF Core):** Utilized as the primary ORM for data mapping and migrations.
* **Microsoft SQL Server:** The relational database used. 
* **Swagger (OpenAPI):** Used for interactive API documentation and testing.

### **Frontend**
* **React + Vite:** Component-based library used to create a dynamic and responsive user experience.

---

## 🏁 Getting Started


Follow these steps to set up the project on your local machine.


### 1. Database Setup
1. Open `appsettings.json` in the **Backend project**.
2. Ensure the `ConnectionString` points to your local **SQL Server** instance.
3. Open a terminal in the project root or the `Infrastructure` folder.
4. Run the following command to create the database schema: "dotnet ef database update"

### 2.  Start the Backend (API)
1. Open the solution in Visual Studio and press F5, or run the following command in the backend project folder: "dotnet run"
2. Once the server is running, verify it by navigating to: https://localhost:XXXX/swagger (replace XXXX with your specific port).

### 3. Start the Frontend (React + Vite)
1. Open a new terminal window and navigate to the frontend directory.
2. Install the necessary dependencies: "npm install".
3. Start the development server: "npm run dev".
4.  Click the link provided in the terminal (usually http://localhost:5173) to launch the application.






