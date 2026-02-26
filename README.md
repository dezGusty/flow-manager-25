# Flow Manager 2025

[![C# 12](https://img.shields.io/badge/C%23-12-blue?style=flat-square)](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12)
[![.NET 9](https://img.shields.io/badge/.NET-9.0-blueviolet?style=flat-square)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-purple?style=flat-square)](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
[![SQLite 3](https://img.shields.io/badge/SQLite-3.42-lightgrey?style=flat-square)](https://www.sqlite.org/index.html)
[![HTML5](https://img.shields.io/badge/HTML-5-orange?style=flat-square)](https://developer.mozilla.org/en-US/docs/Web/HTML)
[![CSS3](https://img.shields.io/badge/CSS-3-blue?style=flat-square)](https://developer.mozilla.org/en-US/docs/Web/CSS)
[![Docker 26](https://img.shields.io/badge/Docker-26.0.0-blue?style=flat-square)](https://docs.docker.com/engine/release-notes/)

---

## ✨ Description

**Flow Manager 2025** is a web platform designed to help teams and organizations efficiently manage, automate, and monitor their internal workflows and business processes.

The application allows users to design custom flows and templates, manage teams and users, and automate the review and approval of submitted forms. It facilitates collaboration by providing a unified dashboard for all process participants, ensuring transparency and accountability at every stage.

### Key Features

- **Teams and Users Visualization:** View and organize all teams and users within the organization.
- **Flow & Template Creation:** Admins can create reusable flows and templates tailored to specific business processes.
- **Form Submission & Flow Execution:** Users fill out forms that move through a defined flow. Each submission can be reviewed, accepted, or rejected according to the flow logic.
- **Progress Tracking:** Real-time status updates for all active flows and submissions.
- **Role-based Access:** Secure, detailed permissions for users and teams.

Built with a modern tech stack (.NET 9, Blazor WebAssembly, SQLite), Flow Manager 2025 is fast, scalable, and easy to deploy via Docker in any environment.

---

## 📚 Technologies Used

- **C# 12**
- **.NET 9.0**
- **Blazor WebAssembly**
- **SQLite 3.42+**
- **HTML5**
- **CSS3**
- **Docker 26.0.0+**

---

## 🏗️ Architecture & Project Structure

The application follows Clean Architecture principles, with clear separation of concerns across several main folders:

```
flow-manager-25/
│
├── API/                      # ASP.NET Core Web API project
│   ├── Controllers/          # API endpoint controllers
│   ├── Program.cs            # Entry point for the API
│   └── appsettings.json      # API configuration
│
├── Application/              # Application layer (business logic, use cases)
│   ├── Services/             # Application services
│   ├── Utils/                # Business use cases
│   └── Interfaces/           # Application interfaces
│
├── Client/                   # Blazor WebAssembly client (UI)
│   ├── Pages/                # Blazor pages
│   ├── Components/           # Reusable UI components
│   ├── wwwroot/              # Static files (CSS, images, etc.)
│   ├── Program.cs            # Entry point for Blazor client
│   └── appsettings.json      # Client-side configuration
│
├── Domain/                   # Domain models and logic
│   ├── Entities/             # Core domain entities (Team, User, Flow, etc.)
│   ├── Dtos/                 # Data Transfer Objects for domain models
│   ├── IRepositories/        # Repository interfaces (e.g., ITeamRepository, IUserRepository)
│   └── Exceptions/           # Custom domain exceptions
│
├── Infrastructure/           # Infrastructure layer (DB, external services, dependencies)
│   ├── Context/              # Database context (DbContext) and configuration
│   ├── Middleware/           # Custom middleware for requests, authentication, etc.
│   ├── Migrations/           # Database migrations (for SQLite)
│   ├── Repositories/         # Repository implementations (TeamRepository, UserRepository, etc.)
│   ├── Seed/                 # Database seeders for initial/test data
│   ├── DependencyInjection/  # Service registration and DI configuration
│   └── flowmanager.db        # The actual SQLite database file
│
├── Shared/                   # Shared resources
│   └── Dtos/                 # Data Transfer Objects shared across the application
|
├── Tests/                    # Unit and integration tests
│   └── Services/             # Tests for services
│
├── docker/                   # Docker files for containerization
│   └── Dockerfile            # Build instructions for Docker image
│
├── README.md                 # Project documentation
├── .gitignore                # Git ignore file
```

### 📂 Folder & Content Details

- **API/**: Hosts the web API, exposes endpoints for the client, contains controllers and configuration.
- **Application/**: Contains business logic, use cases, and interfaces for orchestrating operations.
- **Client/**: Blazor WebAssembly frontend, with pages, reusable components, and static UI resources.
- **Domain/**:
  - **Entities/**: Core domain entities such as Team, User, Flow, etc.
  - **Dtos/**: Data Transfer Objects, used to transfer data between layers (e.g., UserDto, TeamDto, FlowDto).
  - **IRepositories/**: Interfaces for repositories (e.g., ITeamRepository, IUserRepository), defining data access contracts.
  - **Exceptions/**: Domain-specific exception classes.
- **Infrastructure/**:
  - **Context/**: Database context and configuration.
  - **Middleware/**: Custom request/response pipeline logic (auth, error handling, etc.).
  - **Migrations/**: Database schema migrations for setup and updates.
  - **Repositories/**: Concrete implementations for data access.
  - **Seed/**: Scripts/classes for seeding initial/test data.
  - **DependencyInjection/**: Service dependency registration and configuration.
  - **flowmanager.db**: The SQLite database file.
- **Shared/**:
  - **Dtos/**: Data Transfer Objects available for use across the solution (for API, client, etc.)
- **docker/**: Includes Dockerfile for building and running the application in a container.

---

## 🐳 Docker Usage

Make sure you have [Docker](https://www.docker.com/get-started) installed and running while doing this process.

### Run the application locally with Docker

1. **Clone the repo:**

   ```bash
   git clone https://github.com/dezGusty/flow-manager-25.git
   cd flow-manager-25
   ```

2. **Pull the docker images:**

   ```bash
   docker pull stancunicol/flowmanager:api-latest
   docker pull stancunicol/flowmanager:client-latest
   ```

3. **Navigate to the FlowManager folder:**

   ```bash
   cd FlowManager
   ```

4. **Run the application:**

   ```bash
   docker compose up -d
   ```

   The database will be automatically initialized with migrations and seed data on first run.

5. **Access the application:**
   - **Client UI:** http://localhost:3000
   - **API:** http://localhost:5000

6. **Login with default credentials:**
   - **Admin:** `admin.user@simulator.com` / `admin123`
   - **Moderator:** `moderator.user@simulator.com` / `moderator123`
   - **Basic User:** `basic.user@simulator.com` / `basic123`

7. **Stop the application:**

   ```bash
   docker compose down
   ```

   > **Note:** The database is persisted in a Docker volume named `flowmanager-data`, so your data will remain even after stopping the containers.

### Troubleshooting

- **Database initialization issues:** If you see "no such table" errors, the database volume may be corrupted. Remove it and restart:

  ```bash
  docker compose down
  docker volume rm flowmanager-data
  docker compose up -d
  ```

- **Port conflicts:** If ports 3000 or 5000 are already in use, modify the port mappings in `docker-compose.yml`:

  ```yaml
  ports:
    - "3001:80" # Change 3000 to 3001 for client
    - "5001:8080" # Change 5000 to 5001 for API
  ```

- **Fresh start:** To completely reset the application:
  ```bash
  docker compose down
  docker volume rm flowmanager-data
  docker compose up -d
  ```

### Publish and update image to Docker Hub

Replace `<your_dockerhub_username>` with your Docker Hub account.

```bash
docker build -t <your_dockerhub_username>/flow-manager-2025:latest .
docker login
docker push <your_dockerhub_username>/flow-manager-2025:latest
```

#### If you update the image:

1. Build the new image:
   ```bash
   docker build -t <your_dockerhub_username>/flow-manager-2025:latest .
   ```
2. Push to Docker Hub:
   ```bash
   docker push <your_dockerhub_username>/flow-manager-2025:latest
   ```

---

## 🛠️ Development Instructions

1. Open the project in Visual Studio 2022+.
2. Go to **Configure Startup Project...** (right click on the solution, or from the top menu).
3. For each component project (FlowManager.API and FlowManager.Client):
   - Select **Action: Start**
   - Select **Debug Target: https**
   - Always run with **New Profile** (create a new profile for each debug session).

---

## 📝 Additional Notes

- **DB Setup:** The SQLite database file (`flowmanager.db`) is automatically created at first run.
- **Extensibility:** The modular structure allows for rapid addition of new features, integration of new services, or changing the database.
