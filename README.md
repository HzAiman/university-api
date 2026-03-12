# UniversityApi

A simple RESTful API for managing university-related data such as students, courses, and departments. This project is built with ASP.NET Core and serves as a learning exercise for creating web APIs.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Built With](#built-with)

## Features

This API provides endpoints for managing university departments, courses, students, and their associated data.

### Department Endpoints

-   **`GET /api/Department`**: Retrieves a list of all departments.

-   **`POST /api/Department`**: Creates a new department.
    -   **Request Body Example:**
        ```json
        {
          "name": "Computer Science"
        }
        ```

-   **`GET /api/Department/stats`**: Retrieves statistics for all departments, including course count, total credits, and average credits per course.
    -   **Response Body Example (Array):**
        ```json
        [
          {
            "departmentName": "Computer Science",
            "courseCount": 10,
            "totalCredits": 30,
            "averageCredits": 3
          },
          {
            "departmentName": "Physics",
            "courseCount": 8,
            "totalCredits": 28,
            "averageCredits": 3.5
          }
        ]
        ```

### Course Endpoints

-   **`POST /api/Courses`**: Creates a new course.
    -   **Request Body Example:**
        ```json
        {
          "title": "Introduction to Programming",
          "credits": 3,
          "departmentId": 1
        }
        ```

-   **`GET /api/Courses/{id}`**: Retrieves a specific course by its ID, including its associated department information.
    -   **Response Body Example:**
        ```json
        {
          "id": 1,
          "title": "Introduction to Programming",
          "credits": 3,
          "departmentId": 1,
          "department": {
            "id": 1,
            "name": "Computer Science",
            "courses": []
          }
        }
        ```

### Student & Enrollment Endpoints

-   **`POST /api/Students`**: Creates a new student.
    -   **Request Body Example:**
        ```json
        {
          "name": "Alice",
          "email": "alice@example.com"
        }
        ```
    -   **Success Response:**
        -   Status: `201 Created`
        -   Body: The created student object.
    -   **Error Response:**
        -   `400 Bad Request`: If the email is already in use.

-   **`GET /api/Students/{id}`**: Retrieves a specific student by their ID.
    -   **URL Parameter:**
        -   `id`: The ID of the student.
    -   **Response Body Example:**
        ```json
        {
          "id": 1,
          "name": "Alice",
          "email": "alice@example.com",
          "enrollments": []
        }
        ```
    -   **Error Response:**
        -   `404 Not Found`: If the student does not exist.

-   **`POST /api/Students/{studentId}/enroll/{courseId}`**: Enrolls an existing student in an existing course.
    -   **URL Parameters:**
        -   `studentId`: The ID of the student.
        -   `courseId`: The ID of the course.
    -   **Success Response:**
        -   Status: `200 OK`
        -   Body: `Student Alice enrolled in course Introduction to Programming successfully.`
    -   **Error Responses:**
        -   `404 Not Found`: If the student or course does not exist.
        -   `400 Bad Request`: If the student is already enrolled in the course.

-   **`GET /api/Students/{studentId}/schedule`**: Retrieves the course schedule for a specific student.
    -   **URL Parameter:**
        -   `studentId`: The ID of the student.
    -   **Response Body Example:**
        ```json
        {
          "name": "Alice",
          "courses": [
            {
              "title": "Introduction to Programming",
              "enrollmentDate": "2024-01-15T10:30:00Z"
            },
            {
              "title": "Data Structures",
              "enrollmentDate": "2024-01-15T10:30:00Z"
            }
          ]
        }
        ```

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

You will need the following tools installed on your system:

- [.NET SDK](https://dotnet.microsoft.com/download) (The version should match the one specified in the project file)
- [Git](https://git-scm.com/)
- A code editor of your choice (e.g., [Visual Studio Code](https://code.visualstudio.com/), [Visual Studio](https://visualstudio.microsoft.com/))

### Installation

1.  **Clone the repository**
    ```sh
    git clone https://github.com/your-username/UniversityApi.git
    ```
2.  **Navigate to the project directory**
    ```sh
    cd UniversityApi
    ```
3.  **Restore dependencies**
    ```sh
    dotnet restore
    ```
4.  **Run the application**
    ```sh
    dotnet run
    ```
    The API will start and listen on the configured ports (e.g., `https://localhost:7001` and `http://localhost:5001`).

## Usage

Once the application is running, you can explore and interact with the API using the built-in Swagger UI. Navigate to the following URL in your web browser:

`https://localhost:<port>/swagger`

The Swagger UI provides detailed documentation for all available endpoints and allows you to test them directly from the browser.

## Built With

- ASP.NET Core - The web framework used.
- Swashbuckle - For generating Swagger/OpenAPI documentation.
