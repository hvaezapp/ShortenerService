# ShortenerService

A URL shortening service built with ASP.NET Core 9, utilizing MongoDB for persistent storage, Redis for caching, and Docker for containerization. The service converts long URLs into short, unique codes and redirects users to the original URL when the short code is accessed. It uses Minimal APIs for a lightweight implementation, an event-driven approach with DispatchR for cache synchronization, and Scalar for interactive API documentation.

## Table of Contents
- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
  - [Installing Docker](#installing-docker)
  - [Setting Up MongoDB and Redis with Docker](#setting-up-mongodb-and-redis-with-docker)
  - [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [How It Works](#how-it-works)
- [Event-Driven Caching](#event-driven-caching)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Features
- Converts long URLs into unique, compact short codes.
- Stores URL mappings persistently in MongoDB.
- Caches URL mappings in Redis for high-performance retrieval.
- Uses DispatchR for in-process event-driven cache updates.
- Implements Minimal APIs for simplicity and efficiency.
- Containerizes MongoDB and Redis with Docker for easy deployment.
- Provides interactive API documentation via Scalar.
- Scalable and maintainable codebase.

## Architecture
The service follows a modular architecture:
- **Minimal APIs**: Lightweight endpoints for URL shortening and redirection.
- **MongoDB**: NoSQL database for persistent storage of URL mappings.
- **Redis**: In-memory cache for fast URL resolution.
- **DispatchR**: Handles in-process events to synchronize the Redis cache.
- **Docker**: Containers for MongoDB and Redis to simplify setup.
- **Scalar**: Generates interactive API documentation for easy exploration.

The flow is as follows:
1. A user submits a long URL to shorten.
2. A unique short code is generated, stored in MongoDB, and cached in Redis.
3. When a short code is accessed, the service checks Redis for the long URL.
4. If not found in Redis, an event is dispatched via DispatchR to fetch the URL from MongoDB, update the cache, and redirect.

## Technologies Used
- **ASP.NET Core 9**: Backend framework for Minimal APIs.
- **MongoDB**: NoSQL database for persistent storage.
- **Redis**: In-memory data store for caching.
- **DispatchR**: Library for in-process event handling.
- **Docker**: Containerization for MongoDB and Redis.
- **Scalar**: Tool for generating interactive API documentation.
- **C#**: Programming language for the service logic.

## Prerequisites
To run the service, ensure you have:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started) (for containerized MongoDB and Redis)

## Setup Instructions

### Installing Docker
1. **Install Docker**:
   - Download and install Docker Desktop or Docker Engine for your platform by visiting the [official Docker website](https://www.docker.com/get-started).
   - Follow the installation instructions for your operating system (Windows, macOS, or Linux).
   - Verify the installation by running:
     ```bash
     docker --version
     ```

### Setting Up MongoDB and Redis with Docker
After installing Docker, set up MongoDB and Redis containers. You can use the provided `docker-compose.yml` file or run the containers manually.

#### Option 1: Using `docker-compose.yml`
1. **Access the Repository**:
   The source code is available at [https://github.com/hvaezapp/ShortenerService](https://github.com/hvaezapp/ShortenerService). Download the repository as a ZIP file or clone it using your preferred method.

2. **Create a `docker-compose.yml` File** (if not already present):
   ```yaml
   version: '3.8'
   services:
     mongodb:
       image: mongo:latest
       ports:
         - "27017:27017"
       volumes:
         - mongodb_data:/data/db
     redis:
       image: redis:latest
       ports:
         - "6379:6379"
       volumes:
         - redis_data:/data
   volumes:
     mongodb_data:
     redis_data:
   ```

3. **Start the Containers**:
   Run the following command to start MongoDB (port 27017) and Redis (port 6379):
   ```bash
   docker-compose up -d
   ```

#### Option 2: Running Containers Manually
If you prefer not to use `docker-compose`, run the containers individually:
- **MongoDB**:
   ```bash
   docker run -d -p 27017:27017 --name mongodb mongo:latest
   ```
- **Redis**:
   ```bash
   docker run -d -p 6379:6379 --name redis redis:latest
   ```

For detailed instructions on setting up MongoDB and Redis with Docker, visit:
- [MongoDB Docker Hub](https://hub.docker.com/_/mongo)
- [Redis Docker Hub](https://hub.docker.com/_/redis)

### Running the Application
1. **Configure the Application**:
   Update the `appsettings.json` file with the connection strings for MongoDB and Redis. The default settings work with the Docker setup:
   ```json
   {
     "MongoDB": {
       "ConnectionString": "mongodb://localhost:27017",
       "DatabaseName": "ShortenerDB"
     },
     "Redis": {
       "ConnectionString": "localhost:6379"
     }
   }
   ```

2. **Run the Application**:
   Navigate to the project directory and run:
   ```bash
   dotnet run --project ShortenerService
   ```
   The API will be available at `http://localhost:5154`.

3. **Access Scalar API Documentation**:
   Once the application is running, you can access the interactive API documentation powered by Scalar at `http://localhost:5154/scalar` (or the configured Scalar endpoint if customized). This provides a user-friendly interface to explore and test the API endpoints.

## API Endpoints
The service exposes the following Minimal API endpoints, all using the **GET** method. You can explore these endpoints interactively using the Scalar documentation at `http://localhost:5154/scalar`.

- **GET /shorten?url={longUrl}**
  - **Description**: Creates a short URL for a given long URL.
  - **Example**:
    ```bash
    curl "http://localhost:5154/shorten?url=https://github.com/hvaezapp/ShortenerService"
    ```
  - **Response**: A plain string containing the short URL, e.g.:
    ```
    http://localhost:5154/abc123
    ```

- **GET /{shortCode}**
  - **Description**: Redirects to the original long URL associated with the short code.
  - **Example**:
    ```bash
    curl "http://localhost:5154/abc123"
    ```
  - **Response**: HTTP 302 redirect to the original URL (e.g., `https://github.com/hvaezapp/ShortenerService`).

## How It Works
1. **URL Shortening**:
   - A long URL is submitted via the `/shorten?url=` endpoint.
   - The service generates a unique short code, stores the mapping in MongoDB, and caches it in Redis.
   - A plain short URL (e.g., `http://localhost:5154/abc123`) is returned.
2. **URL Resolution**:
   - When a short code is accessed via the `/{shortCode}` endpoint, the service checks Redis for the long URL.
   - If found, the user is redirected to the long URL.
   - If not found, an event is dispatched via DispatchR to fetch the URL from MongoDB, update the Redis cache, and redirect.

## Event-Driven Caching
The service uses DispatchR for in-process event handling:
- If a short code is not found in Redis, an event is published to trigger a MongoDB lookup.
- The retrieved URL is cached in Redis for subsequent requests.
- This approach minimizes database queries and improves performance.

## Configuration
The `appsettings.json` file contains:
- **MongoDB**:
  - `ConnectionString`: MongoDB server address (default: `mongodb://localhost:27017`).
  - `DatabaseName`: Database name (default: `ShortenerDB`).
- **Redis**:
  - `ConnectionString`: Redis server address (default: `localhost:6379`).

Override these settings using environment variables or a custom `appsettings.Development.json` file if needed.

## Contributing
Contributions are welcome! To contribute:
1. Access the repository at [https://github.com/hvaezapp/ShortenerService](https://github.com/hvaezapp/ShortenerService).
2. Fork the repository.
3. Create a new branch (`git checkout -b feature/your-feature`).
4. Make your changes and commit (`git commit -m "Add your feature"`).
5. Push to your branch (`git push origin feature/your-feature`).
6. Open a pull request.

Ensure your code follows the project's coding standards and includes tests where applicable.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
