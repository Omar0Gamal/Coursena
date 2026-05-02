# Coursena API Endpoints

> **Base URL:** `https://<your-domain>/api`  
> **Authentication:** JWT Bearer token — include `Authorization: Bearer <token>` header on protected routes.

---

## Table of Contents

1. [Auth](#1-auth)
2. [Public Courses](#2-public-courses)
3. [Student](#3-student)
4. [Student Content](#4-student-content)
5. [Teacher](#5-teacher)
6. [Teacher Courses](#6-teacher-courses)
7. [Teacher Content](#7-teacher-content)
8. [Admin](#8-admin)
9. [Admin Courses](#9-admin-courses)
10. [Reviews](#10-reviews)
11. [Messages](#11-messages)

---

## 1. Auth

Base route: `/api/auth`

---

### POST `/api/auth/register-teacher`

Register a new teacher account (pending admin approval).

**Request Body:**

```json
{
  "email": "teacher@example.com",
  "password": "Secret123!",
  "confirmPassword": "Secret123!",
  "fullName": "John Doe"
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Teacher registered successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Registration Failed",
  "detail": "Email is already taken.",
  "status": 400
}
```

---

### POST `/api/auth/register-student`

Register a new student account using a teacher's invite code.

**Request Body:**

```json
{
  "email": "student@example.com",
  "password": "Secret123!",
  "confirmPassword": "Secret123!",
  "fullName": "Jane Smith",
  "inviteCode": "ABC123",
  "gradeId": 2
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Student registered successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Registration Failed",
  "detail": "Invalid invite code.",
  "status": 400
}
```

---

### POST `/api/auth/login`

Authenticate and receive a JWT token.

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "Secret123!"
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Login successful"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Authentication Failed",
  "detail": "Invalid email or password.",
  "status": 400
}
```

---

### POST `/api/auth/Logout`

Log out the currently authenticated user.

**Headers:** `Authorization: Bearer <token>`

**Request Body:** *(none)*

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Logged out successfully"
}
```

---

### PUT `/api/auth/update`

Update the profile of the currently authenticated user.

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "email": "newemail@example.com",
  "password": "NewSecret123!",
  "confirmPassword": "NewSecret123!",
  "fullName": "John Updated"
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Profile updated successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "isSuccess": false,
  "message": "Update failed."
}
```

---

## 2. Public Courses

Base route: `/api/courses`

---

### GET `/Get-Courses`

Retrieve courses. Behavior varies by authentication state:

- **Anonymous** — returns all public courses matching the optional `inviteCode`.
- **Student** — requires `inviteCode`; returns courses the student can access.
- **Other authenticated users** — same as anonymous.

**Query Parameters:**

| Parameter    | Type   | Required | Description                  |
|--------------|--------|----------|------------------------------|
| `inviteCode` | string | No*      | Teacher's invite code. Required when caller is a Student. |

**Response `200 OK`:**

```json
[
  {
    "id": 1,
    "title": "Math 101",
    "description": "Intro to algebra",
    "price": 0.00,
    "isApproved": true,
    "teacherName": "John Doe"
  }
]
```

**Response `400 Bad Request` (Student without invite code):**

```json
"Invite code is required"
```

---

### GET `/api/courses/search`

Search courses by a specific field.

**Query Parameters:**

| Parameter      | Type   | Required | Description                                          |
|----------------|--------|----------|------------------------------------------------------|
| `inviteCode`   | string | Yes      | Teacher's invite code (tenant scope)                 |
| `searchBy`     | string | Yes      | Field to search by (e.g., `title`, `teacherName`)    |
| `searchString` | string | Yes      | The search keyword                                   |

**Example Request:**

```
GET /api/courses/search?inviteCode=ABC123&searchBy=title&searchString=Math
```

**Response `200 OK`:**

```json
[
  {
    "id": 1,
    "title": "Math 101",
    "description": "Intro to algebra",
    "price": 0.00,
    "isApproved": true,
    "teacherName": "John Doe"
  }
]
```

---

## 3. Student

Base route: `/api/student`  
**Role required:** `Student`

---

### POST `/api/student/enroll-by-code`

Enroll in a course using a one-time enrollment code.

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "courseId": 1,
  "code": "ENROLL-XYZ789"
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Enrolled successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Enrollment Failed",
  "detail": "Invalid or already used code.",
  "status": 400
}
```

---

### GET `/api/student/my-courses`

Get all courses the student is enrolled in.

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  {
    "id": 1,
    "title": "Math 101",
    "description": "Intro to algebra",
    "price": 0.00,
    "isApproved": true,
    "teacherName": "John Doe"
  }
]
```

---

### POST `/api/student/check-completion/{courseId}`

Check whether the student has completed a specific course.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter  | Type | Description     |
|------------|------|-----------------|
| `courseId` | int  | The course ID   |

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Course completed"
}
```

**Response `400 Bad Request`:**

```json
{
  "isSuccess": false,
  "message": "Course not completed yet"
}
```

---

## 4. Student Content

Base route: `/api/student/content`  
**Role required:** `Student`

---

### GET `/api/student/content/{courseId}`

Get all content items for a course the student is enrolled in.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter  | Type | Description   |
|------------|------|---------------|
| `courseId` | int  | The course ID |

**Response `200 OK`:**

```json
[
  {
    "title": "Lesson 1 - Introduction",
    "videoUrl": "https://videos.example.com/lesson1.mp4",
    "documentUrl": "https://docs.example.com/lesson1.pdf",
    "assignmentUrl": "https://assignments.example.com/hw1.pdf",
    "order": 1
  },
  {
    "title": "Lesson 2 - Basics",
    "videoUrl": "https://videos.example.com/lesson2.mp4",
    "documentUrl": null,
    "assignmentUrl": null,
    "order": 2
  }
]
```

**Response `403 Forbidden`:**

```json
{
  "title": "Access Denied",
  "detail": "You are not enrolled in this course.",
  "status": 403
}
```

---

## 5. Teacher

Base route: `/api/teacher`  
**Role required:** `Teacher`

---

### POST `/api/teacher/generate-codes`

Generate one-time enrollment codes for a course.

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "courseId": 1,
  "count": 10
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "10 codes generated successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Generate Codes Failed",
  "detail": "Course not found or not owned by you.",
  "status": 400
}
```

---

### GET `/api/teacher/See-generated-codes/{courseId}`

View all enrollment codes generated for a specific course.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter  | Type | Description   |
|------------|------|---------------|
| `courseId` | int  | The course ID |

**Response `200 OK`:**

```json
[
  {
    "code": "ENROLL-XYZ789",
    "isUsed": false
  },
  {
    "code": "ENROLL-ABC456",
    "isUsed": true
  }
]
```

---

## 6. Teacher Courses

Base route: `/api/teacher/courses`  
**Role required:** `Teacher`

---

### POST `/api/teacher/courses/Add`

Create a new course (starts as pending admin approval).

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "title": "Math 101",
  "description": "Intro to algebra",
  "price": 49.99,
  "durationInDays": 30,
  "videoUrl": "https://videos.example.com/intro.mp4",
  "content": "Full course material text",
  "subjectId": 2,
  "gradeId": 3
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Course created successfully"
}
```

---

### GET `/api/teacher/courses/Get-Courses`

Get all courses created by the authenticated teacher.

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  {
    "id": 1,
    "title": "Math 101",
    "description": "Intro to algebra",
    "price": 49.99,
    "isApproved": false,
    "teacherName": "John Doe"
  }
]
```

---

### PUT `/api/teacher/courses/Update{id}`

Update an existing course owned by the teacher.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter | Type | Description      |
|-----------|------|------------------|
| `id`      | int  | The course ID    |

**Request Body:**

```json
{
  "title": "Math 101 - Updated",
  "description": "Updated description",
  "price": 59.99,
  "durationInDays": 45,
  "videoUrl": "https://videos.example.com/updated.mp4",
  "content": "Updated content text",
  "subjectId": 2,
  "gradeId": 3
}
```

**Response `200 OK`:**

```json
"Updated successfully"
```

**Response `400 Bad Request`:**

```json
{
  "title": "Update Failed",
  "detail": "Course not found or you are not allowed to update it",
  "status": 400
}
```

---

### DELETE `/api/teacher/courses/Delete{id}`

Delete a course owned by the teacher.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter | Type | Description   |
|-----------|------|---------------|
| `id`      | int  | The course ID |

**Response `200 OK`:**

```json
"Deleted successfully"
```

**Response `400 Bad Request`:**

```json
{
  "title": "Delete Failed",
  "detail": "Course not found or you are not allowed to delete it",
  "status": 400
}
```

---

### GET `/api/teacher/courses/invite-code`

Get the teacher's personal invite code.

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
{
  "inviteCode": "ABC123"
}
```

**Response `404 Not Found`:**

```json
"Invite code not found"
```

---

### GET `/api/teacher/courses/dashboard`

Get the teacher's dashboard statistics.

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
{
  "totalCourses": 5,
  "totalStudents": 120,
  "totalCodes": 200,
  "usedCodes": 120,
  "activeStudents": 98
}
```

---

### GET `/api/teacher/courses/subjects`

Get the list of available subjects (lookup data).

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  { "id": 1, "name": "Mathematics" },
  { "id": 2, "name": "Science" },
  { "id": 3, "name": "English" }
]
```

---

### GET `/api/teacher/courses/grades`

Get the list of available grades (lookup data).

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  { "id": 1, "name": "Grade 1" },
  { "id": 2, "name": "Grade 2" },
  { "id": 3, "name": "Grade 3" }
]
```

---

## 7. Teacher Content

Base route: `/api/teacher/content`  
**Role required:** `Teacher`

---

### POST `/api/teacher/content`

Add a new content item to a course.

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "title": "Lesson 1 - Introduction",
  "videoUrl": "https://videos.example.com/lesson1.mp4",
  "documentUrl": "https://docs.example.com/lesson1.pdf",
  "assignmentUrl": "https://assignments.example.com/hw1.pdf",
  "order": 1,
  "courseId": 1
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Content added successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Add Content Failed",
  "detail": "Course not found or you do not own it.",
  "status": 400
}
```

---

## 8. Admin

Base route: `/api/admin`  
**Role required:** `Admin`

---

### GET `/api/admin/pending-teachers`

Get all teacher accounts awaiting approval.

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  {
    "id": "user-guid-123",
    "fullName": "John Doe",
    "email": "teacher@example.com",
    "isApproved": false,
    "inviteCode": "ABC123"
  }
]
```

---

### POST `/api/admin/approve-teacher/{teacherId}`

Approve a pending teacher account.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter   | Type   | Description         |
|-------------|--------|---------------------|
| `teacherId` | string | The teacher user ID |

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Teacher approved successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Approve Teacher Failed",
  "detail": "Teacher not found.",
  "status": 400
}
```

---

### DELETE `/api/admin/reject-teacher/{teacherId}`

Reject and remove a pending teacher account.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter   | Type   | Description         |
|-------------|--------|---------------------|
| `teacherId` | string | The teacher user ID |

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Teacher rejected successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "title": "Reject Teacher Failed",
  "detail": "Teacher not found.",
  "status": 400
}
```

---

### GET `/api/admin/Get-Users`

Get all users in the system.

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  {
    "id": "user-guid-123",
    "email": "user@example.com",
    "fullName": "Jane Smith",
    "role": "Student"
  },
  {
    "id": "user-guid-456",
    "email": "teacher@example.com",
    "fullName": "John Doe",
    "role": "Teacher"
  }
]
```

---

### DELETE `/api/admin/Delete{userId}`

Delete a user by ID.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter | Type   | Description    |
|-----------|--------|----------------|
| `userId`  | string | The user's ID  |

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "User deleted successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "isSuccess": false,
  "message": "User not found"
}
```

---

### POST `/api/admin/Add-User`

Create a new user with a specified role.

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "email": "newuser@example.com",
  "password": "Secret123!",
  "fullName": "New User",
  "role": "Student"
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "User created successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "isSuccess": false,
  "message": "Email already exists"
}
```

---

## 9. Admin Courses

Base route: `/api/admin/courses`  
**Role required:** `Admin`

---

### GET `/api/admin/courses/GetCourses`

Get all courses (including pending ones).

**Headers:** `Authorization: Bearer <token>`

**Response `200 OK`:**

```json
[
  {
    "id": 1,
    "title": "Math 101",
    "description": "Intro to algebra",
    "price": 49.99,
    "isApproved": false,
    "teacherName": "John Doe"
  }
]
```

---

### POST `/api/admin/courses/{id}/approve`

Approve a course for public listing.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter | Type | Description   |
|-----------|------|---------------|
| `id`      | int  | The course ID |

**Response `200 OK`:**

```json
"Course approved successfully"
```

**Response `404 Not Found`:**

```json
{
  "title": "Approval Failed",
  "detail": "Course not found",
  "status": 404
}
```

---

### POST `/api/admin/courses/{id}/reject`

Reject a submitted course.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter | Type | Description   |
|-----------|------|---------------|
| `id`      | int  | The course ID |

**Response `200 OK`:**

```json
"Course rejected successfully"
```

**Response `404 Not Found`:**

```json
{
  "title": "Rejection Failed",
  "detail": "Course not found",
  "status": 404
}
```

---

## 10. Reviews

Base route: `/api/reviews`

---

### POST `/api/reviews/Add-Review`

Add a review for a course.  
**Role required:** `Student`

**Headers:** `Authorization: Bearer <token>`

**Request Body:**

```json
{
  "courseId": 1,
  "rating": 5,
  "comment": "Excellent course!"
}
```

**Response `200 OK`:**

```json
{
  "isSuccess": true,
  "message": "Review added successfully"
}
```

**Response `400 Bad Request`:**

```json
{
  "isSuccess": false,
  "message": "You are not enrolled in this course"
}
```

---

### GET `/api/reviews/See-Reviews{courseId}`

Get all reviews for a course. Accessible anonymously.

**Path Parameters:**

| Parameter  | Type | Description   |
|------------|------|---------------|
| `courseId` | int  | The course ID |

**Example Request:**

```
GET /api/reviews/See-Reviews1
```

**Response `200 OK`:**

```json
[
  {
    "studentId": "user-guid-123",
    "rating": 5,
    "comment": "Excellent course!",
    "createdAt": "2024-01-15T10:30:00Z"
  },
  {
    "studentId": "user-guid-456",
    "rating": 4,
    "comment": "Very good content.",
    "createdAt": "2024-01-16T08:00:00Z"
  }
]
```

---

## 11. Messages

Base route: `/api/messages`  
**Authentication required**

---

### GET `/api/messages/History{userId}`

Get the conversation history between the authenticated user and another user.

**Headers:** `Authorization: Bearer <token>`

**Path Parameters:**

| Parameter | Type   | Description                       |
|-----------|--------|-----------------------------------|
| `userId`  | string | The ID of the other user          |

**Example Request:**

```
GET /api/messages/Historyuser-guid-456
```

**Response `200 OK`:**

```json
[
  {
    "senderId": "user-guid-123",
    "receiverId": "user-guid-456",
    "content": "Hello, I have a question about Lesson 2.",
    "sentAt": "2024-01-15T10:30:00Z"
  },
  {
    "senderId": "user-guid-456",
    "receiverId": "user-guid-123",
    "content": "Sure, go ahead!",
    "sentAt": "2024-01-15T10:31:00Z"
  }
]
```

---

## Summary Table

| Method | Endpoint | Auth | Role |
|--------|----------|------|------|
| POST | `/api/auth/register-teacher` | ❌ | — |
| POST | `/api/auth/register-student` | ❌ | — |
| POST | `/api/auth/login` | ❌ | — |
| POST | `/api/auth/Logout` | ✅ | Any |
| PUT | `/api/auth/update` | ✅ | Any |
| GET | `/Get-Courses` | ❌ / ✅ | Any |
| GET | `/api/courses/search` | ❌ | — |
| POST | `/api/student/enroll-by-code` | ✅ | Student |
| GET | `/api/student/my-courses` | ✅ | Student |
| POST | `/api/student/check-completion/{courseId}` | ✅ | Student |
| GET | `/api/student/content/{courseId}` | ✅ | Student |
| POST | `/api/teacher/generate-codes` | ✅ | Teacher |
| GET | `/api/teacher/See-generated-codes/{courseId}` | ✅ | Teacher |
| POST | `/api/teacher/courses/Add` | ✅ | Teacher |
| GET | `/api/teacher/courses/Get-Courses` | ✅ | Teacher |
| PUT | `/api/teacher/courses/Update{id}` | ✅ | Teacher |
| DELETE | `/api/teacher/courses/Delete{id}` | ✅ | Teacher |
| GET | `/api/teacher/courses/invite-code` | ✅ | Teacher |
| GET | `/api/teacher/courses/dashboard` | ✅ | Teacher |
| GET | `/api/teacher/courses/subjects` | ✅ | Teacher |
| GET | `/api/teacher/courses/grades` | ✅ | Teacher |
| POST | `/api/teacher/content` | ✅ | Teacher |
| GET | `/api/admin/pending-teachers` | ✅ | Admin |
| POST | `/api/admin/approve-teacher/{teacherId}` | ✅ | Admin |
| DELETE | `/api/admin/reject-teacher/{teacherId}` | ✅ | Admin |
| GET | `/api/admin/Get-Users` | ✅ | Admin |
| DELETE | `/api/admin/Delete{userId}` | ✅ | Admin |
| POST | `/api/admin/Add-User` | ✅ | Admin |
| GET | `/api/admin/courses/GetCourses` | ✅ | Admin |
| POST | `/api/admin/courses/{id}/approve` | ✅ | Admin |
| POST | `/api/admin/courses/{id}/reject` | ✅ | Admin |
| POST | `/api/reviews/Add-Review` | ✅ | Student |
| GET | `/api/reviews/See-Reviews{courseId}` | ❌ | — |
| GET | `/api/messages/History{userId}` | ✅ | Any |
