# TaskManagerAPI

TaskManagerAPI is a web API built using ASP.NET Core that allows users to manage tasks effectively. It provides a robust set of features for creating, retrieving, updating, deleting, and querying tasks. Users can also track task progress and search for specific tasks based on various filters.

## Features

- **Create Tasks**: Add new tasks to the system with details such as title, description, due date, status, priority, and category.
- **Retrieve Tasks**:
  - Retrieve all tasks with support for pagination, filtering by status, priority, category, and search queries.
  - Retrieve a single task by its unique ID.
- **Update Tasks**: Modify existing tasks by providing their ID and updated data.
- **Delete Tasks**: Remove tasks by their unique ID.
- **View Task Progress**: Calculate the completion rate of all tasks.
- **Search Tasks**: Search for tasks using keywords in the title or description.

## Technology Stack

- **Backend Framework**: ASP.NET Core
- **Database**: SQL Server (using Entity Framework Core for ORM)
- **Tools**: Visual Studio 2022, .NET CLI
- **API Documentation**: Swagger

## Endpoints

### Retrieve All Tasks

`GET: /api/tasks`

#### Parameters:

- `status` (optional): Filter tasks by status (e.g., Pending, In Progress, Completed).
- `priority` (optional): Filter tasks by priority (e.g., Low, Medium, High).
- `category` (optional): Filter tasks by category.
- `search` (optional): Search tasks by title or description.
- `page` (optional, default=1): Specify the page number for pagination.
- `pageSize` (optional, default=10): Specify the number of tasks per page.

#### Example:

```
https://localhost:7289/api/Tasks?page=1&pageSize=10
```

### Retrieve Task by ID

`GET: /api/tasks/{id}`

#### Example:

```
https://localhost:7053/api/Tasks/3
```

### Create a Task

`POST: /api/tasks`

#### Body:

```json
{
  "title": "New Task",
  "description": "Description of the new task.",
  "status": "Pending",
  "priority": "Medium",
  "category": "Work"
}
```

### Update a Task

`PUT: /api/tasks/{id}`

#### Body:

```json
{
  "id": 3,
  "title": "Updated Task",
  "description": "Updated description for testing.",
  "status": "In Progress",
  "priority": "Medium",
  "category": "Personal"
}
```

#### Example:

```
https://localhost:7053/api/Tasks/3
```

### Delete a Task

`DELETE: /api/tasks/{id}`

#### Example:

```
https://localhost:7053/api/Tasks/4
```

### View Task Progress

`GET: /api/tasks/progress`

Retrieve the total number of tasks, the count of completed tasks, and the completion rate.

#### Example:

```
https://localhost:7053/api/Tasks/progress
```

### Search Tasks

`GET: /api/tasks/search`

#### Query Parameters:

- `query` (required): The search keyword to match tasks by title or description.

#### Example:

```
https://localhost:7053/api/Tasks/search?query=dev
```

