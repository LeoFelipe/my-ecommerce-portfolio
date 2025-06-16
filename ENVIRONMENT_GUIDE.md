# 🌐 Environment Guide - Ecommerce Portfolio

This guide documents how to configure, run, and understand the environments available in this project:

* **.NET Aspire (AppHost)**
* **Docker Compose**
* **Integration Tests using Testcontainers**

Each environment is set up to support isolated execution and proper inter-service communication. Below are the definitions, conventions, and best practices to ensure smooth experience.

---

## ✅ Supported Environments Overview

| Environment                | Purpose                            | Usage                        |
| -------------------------- | ---------------------------------- | ---------------------------- |
| **Aspire (AppHost)**       | Observability, local development   | Run via Visual Studio or CLI |
| **Docker Compose**         | Standalone container orchestration | Run via `docker-compose up`  |
| **Testcontainers (xUnit)** | Automated integration tests        | Run via `dotnet test`        |

---

## 🛠️ Technology Stack

### Core Framework
* .NET 9.0 (Preview)
* ASP.NET Core Web API
* Entity Framework Core 9.0
* MongoDB Driver 3.4.0

### Infrastructure
* PostgreSQL 16
* MongoDB 7.0
* Redis 7.2
* RabbitMQ 3.12
* Docker & Docker Compose
* .NET Aspire

### Communication & Messaging
* MassTransit 8.4.1
* RabbitMQ (Message Broker)
* HTTP/REST APIs

### API Documentation
* Scalar 2.3.1
* OpenAPI/Swagger

---

## 🧠 Environment Conventions

### 🔗 Container Aliases (Service Names)

All containers use the same naming convention across environments for consistency:

| Service        | Alias / Hostname                    | Description                          |
| -------------- | ----------------------------------- | ------------------------------------ |
| PostgreSQL     | `ecommerceportfolio-postgres-db`    | Stores Orders and Deliveries data    |
| MongoDB        | `ecommerceportfolio-mongo-db`       | Stores Carts data                    |
| Redis          | `ecommerceportfolio-redis-db`       | Distributed caching                  |
| RabbitMQ       | `ecommerceportfolio-rabbit-mq`      | Message broker for async events      |
| Orders API     | `ecommerceportfolio-orders-api`     | Order management service             |
| Deliveries API | `ecommerceportfolio-deliveries-api` | Delivery tracking service            |
| Carts API      | `ecommerceportfolio-carts-api`      | Shopping cart service                |

These aliases are used for **internal container communication**, for example:

```
http://ecommerceportfolio-carts-api:5050
```

---

## 🚪 Port Mapping

| Service        | Development Ports (HTTP/HTTPS) | Container Ports | Description                    |
| -------------- | ----------------------------- | --------------- | ------------------------------ |
| Carts API      | 5050/7230                    | 8080/8081      | Shopping cart management       |
| Orders API     | 5150/7220                    | 8080/8081      | Order processing               |
| Deliveries API | 5250/7021                    | 8080/8081      | Delivery tracking              |
| PostgreSQL     | 5432                         | 5432           | Database server                |
| MongoDB        | 27017                        | 27017          | Document database              |
| Redis          | 6379                         | 6379           | Cache server                   |
| RabbitMQ       | 5672/15672                   | 5672/15672     | Message broker/Management UI   |

* Docker Compose and Testcontainers use **fixed ports**
* Aspire uses **dynamic ports**, automatically resolved using `.GetEndpoint("http").Url`
* Container ports are standardized to 8080/8081 for all services

---

## 🔄 Message Bus Configuration

### RabbitMQ Connection
```json
"ConnectionStrings": {
  "RabbitMq": "amqp://guest:guest@ecommerceportfolio-rabbit-mq:5672"
}
```

### MassTransit Endpoints
| Service        | Consumer Endpoint                    | Description                    |
| -------------- | ------------------------------------ | ------------------------------ |
| Orders API     | `create-delivery-for-order-authorized` | Creates delivery on order auth |
| Carts API      | `remove-cart-for-order-authorized`     | Removes cart on order auth     |

---

## 🧪 Testcontainers Considerations

* A **shared PostgreSQL container** is used with multiple databases:
  * `EcommercePortfolioOrder`
  * `EcommercePortfolioDelivery`
* MongoDB container for cart data
* Redis container for caching
* RabbitMQ container for message bus
* All containers are attached to the same virtual network `ecommerce_network`
* Testcontainers automatically manage container lifecycle

---

## 🧪 Testing Strategy

### Unit Tests
* Run in-memory without infrastructure dependencies
* Located in `tests/EcommercePortfolio.*.UnitTests`
* Follow [TESTING_STANDARDS.md](TESTING_STANDARDS.md)
* Use AAA pattern and Moq for mocking

### Functional Tests
* Use Testcontainers for infrastructure
* Located in `tests/EcommercePortfolio.FunctionalTests`
* Test complete service workflows
* Include database seeding and cleanup

---

## ⚠️ Environment-Specific Considerations

### Development
* Use Visual Studio 2022 or VS Code
* .NET 9.0 SDK (Preview) required
* Docker Desktop or Docker Engine (WSL2)
* Scalar UI available at `/scalar` endpoint

### Docker Compose
* Fixed port mappings
* Shared network `ecommerce_network`
* Environment variables in `docker-compose.override.yml`

### Aspire
* Dynamic port allocation
* Automatic health checks
* Service discovery
* Distributed tracing

---

## 📦 Environment Variables

### Required Variables
```json
{
  "ConnectionStrings": {
    "PostgresDb": "Host=ecommerceportfolio-postgres-db;Database=EcommercePortfolioOrder;Username=postgres;Password=postgres",
    "MongoDb": "mongodb://ecommerceportfolio-mongo-db:27017",
    "RedisCache": "ecommerceportfolio-redis-db:6379",
    "RabbitMq": "amqp://guest:guest@ecommerceportfolio-rabbit-mq:5672"
  },
  "ApiSettings": {
    "CartApiUrl": "http://ecommerceportfolio-carts-api:5101",
    "PaymentApiUrl": "http://ecommerceportfolio-payment-api:5301"
  }
}
```

### Environment Variable Pattern
* Use `__` (double underscore) for hierarchy
* Example: `ConnectionStrings__PostgresDb`

---

## 🛠️ Development Tips

1. **Docker Network**
   ```bash
   docker network ls
   docker network create ecommerce_network
   ```

2. **Container Management**
   ```bash
   # Rebuild images
   docker-compose build
   
   # View logs
   docker-compose logs -f
   
   # Clean up
   docker-compose down -v
   ```

3. **Database Management**
   * PostgreSQL: Use pgAdmin or DBeaver
   * MongoDB: Use MongoDB Compass
   * Redis: Use RedisInsight

4. **Message Bus**
   * RabbitMQ Management UI: http://localhost:15672
   * Default credentials: guest/guest

5. **API Documentation**
   * Scalar UI: http://localhost:{port}/scalar
   * OpenAPI: http://localhost:{port}/openapi

---

## 🧭 Conclusion

This guide ensures that any contributor can:
* Set up and run any environment
* Understand service communication
* Manage infrastructure components
* Run and debug tests
* Access monitoring and documentation tools

For more details, refer to:
* [`docker-compose.yml`](src/docker-compose.yml)
* [`docker-compose.testing.yml`](src/docker-compose.testing.yml)
* [`BuilderContainerFactory.cs`](src/tests/EcommercePortfolio.FunctionalTests/Factories/Configurations/BuilderContainerFactory.cs)
* [`AppHost Program.cs`](src/aspire/EcommercePortfolio.AppHost/Program.cs)

Happy coding! 🎯
