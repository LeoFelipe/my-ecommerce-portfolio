# 🌐 Environment Guide - Ecommerce Portfolio

This guide documents how to configure, run, and understand the three environments available in this project:

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

## 🧠 Environment Conventions

### 🔗 Container Aliases (Service Names)

All containers use the same naming convention across environments for consistency:

| Service        | Alias / Hostname                    |
| -------------- | ----------------------------------- |
| PostgreSQL     | `ecommerceportfolio-postgres-db`    |
| MongoDB        | `ecommerceportfolio-mongo-db`       |
| Redis          | `ecommerceportfolio-redis-db`       |
| RabbitMQ       | `ecommerceportfolio-rabbit-mq`      |
| Orders API     | `ecommerceportfolio-orders-api`     |
| Deliveries API | `ecommerceportfolio-deliveries-api` |
| Carts API      | `ecommerceportfolio-carts-api`      |

These aliases are used for **internal container communication**, for example:

```
http://ecommerceportfolio-carts-api:5070
```

---

## 🚪 Port Mapping

| Service        | Internal Port | Docker Compose | Testcontainers | Aspire  |
| -------------- | ------------- | -------------- | -------------- | ------- |
| Orders API     | 5050          | 5050:5051      | 5050           | dynamic |
| Deliveries API | 5060          | 5060:5061      | 5060           | dynamic |
| Carts API      | 5070          | 5070:5071      | 5070           | dynamic |

* Docker Compose and Testcontainers use **fixed ports**.
* Aspire uses **dynamic ports**, automatically resolved using `.GetEndpoint("http").Url`.

---

## 🧪 Testcontainers Considerations

* A **shared PostgreSQL container** is used with multiple databases (e.g. `EcommercePortfolioOrder`, `EcommercePortfolioDelivery`).
* The default database (`ignore`) is **replaced via `Replace("Database=ignore", ...)`** during test setup.
* Containers are attached to the same virtual network `ecommerce_network`.

---

## 🧪 Unit Tests

* Run in-memory without any container or infrastructure dependency
* Organized in the `tests/EcommercePortfolio.Orders.UnitTests` folder
* Follow the [TESTING\_STANDARDS.md](TESTING_STANDARDS.md)
* Use patterns like AAA (Arrange-Act-Assert) and mock services via `Moq`
* Provide fast feedback and guarantee business rule integrity

---

## ⚠️ Avoiding Conflicts

### Port Conflicts

* Aspire and Docker Compose **should not be run simultaneously**, as Compose binds fixed ports.
* Testcontainers are isolated and dynamically bind ports to the host, so conflict is unlikely.

### Configuration Conflicts

* AppSettings have been **cleaned from hardcoded connection strings**.
* All environments inject variables using:

  * `WithEnvironment(...)` in Aspire
  * `environment:` in Compose
  * `AddInMemoryCollection(...)` in Testcontainers

---

## 📦 Environment Variables Pattern

Configuration keys follow the standard hierarchical convention:

```json
"ConnectionStrings": {
  "OrderPostgresDbConnection": "...",
  "RabbitMqConnection": "..."
},
"ApiSettings": {
  "CartApiUrl": "http://..."
}
```

Environment or CLI use `__` (double underscore):

```
ConnectionStrings__OrderPostgresDbConnection
ApiSettings__CartApiUrl
```

---

## 🛠️ Tips

* Use `docker network ls` and `docker network create ecommerce_network` to ensure the network exists.
* If running tests: ensure **Docker Desktop is running**, or **Docker Engine** is available via WSL2.
* Don't forget to rebuild images (`docker-compose build`) after editing Dockerfiles.
* Aspire handles health checks and environment injection **automatically**, no ports required.

---

## 🧭 Conclusion

This guide ensures that any contributor can:

* Understand and run any environment
* Avoid conflict between environments
* Maintain consistency across Docker, Aspire, and Testcontainers

For more details, refer to:

* [`docker-compose.yml`](../_docker/docker-compose.yml)
* [`BuilderContainerFactory.cs`](../tests/Factories/BuilderContainerFactory.cs)
* [`AppHost Program.cs`](../_aspire/EcommercePortfolio.AppHost/Program.cs)

Happy coding! 🎯
