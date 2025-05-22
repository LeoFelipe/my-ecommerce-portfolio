# [Ecommerce Portfolio API](https://github.com/LeoFelipe/my-ecommerce-portfolio)

![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Container-2496ED?logo=docker&logoColor=white)
![Docker Compose](https://img.shields.io/badge/Docker%20Compose-Orchestration-2496ED?logo=docker&logoColor=white)
![Aspire](https://img.shields.io/badge/Aspire-Observability-512BD4?logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework%20Core-9.0-green?logo=dotnet)
![CQRS](https://img.shields.io/badge/CQRS-Pattern-blue)
![DDD](https://img.shields.io/badge/DDD-Domain--Driven%20Design-blue)
![MediatR](https://img.shields.io/badge/MediatR-Application%20Mediator-ff69b4)
![FluentValidation](https://img.shields.io/badge/FluentValidation-Validation-blue)
![Scalar](https://img.shields.io/badge/Scalar-API%20Docs-4B275F)
![Domain Events](https://img.shields.io/badge/Domain%20Events-Pattern-orange)
![Notification Pattern](https://img.shields.io/badge/Notification%20Pattern-Error%20Handling-FF6B6B)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Message%20Broker-FF6600?logo=rabbitmq&logoColor=white)
![MassTransit](https://img.shields.io/badge/MassTransit-Message%20Bus-00A4EF)
![Redis](https://img.shields.io/badge/Redis-Distributed%20Cache-DC382D?logo=redis&logoColor=white)
![HybridCache](https://img.shields.io/badge/HybridCache-Multi%20Layer%20Cache-6DB33F)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This is a personal portfolio project developed with a focus on software architecture best practices, scalability, and maintainability.

## 🎯 Overview

This is a modular monolith structured by Bounded Contexts, inspired by DDD (Domain-Driven Design) and microservices principles. The system implements a complete e-commerce solution, integrating orders, payments, deliveries, and shopping cart.

## 🎓 Motivation and Goals

- Demonstrate deep understanding of modern architecture
- Apply concepts such as CQRS, Value Objects, and Domain Events
- Simulate a real environment with integration of external services and infrastructure
- Use Aspire for observability and traceability

## 🚀 Technologies and Practices

- **.NET 9** - Latest Microsoft framework
- **Domain-Driven Design (DDD)** - Business domain-focused architecture
- **Event-Driven Architecture** - Asynchronous communication between services
- **CQRS** - Command and Query Responsibility Segregation
- **Clean Architecture** - Clear separation of concerns
- **Unit Testing** - Quality and maintainability assurance
- **Docker** - Containerization and orchestration with WSL2 support
- **ASP.NET Core** - Modern and high-performance web framework
- **Entity Framework Core** - ORM for data access
- **MediatR** - Mediator pattern implementation
- **FluentValidation** - Object validation
- **Scalar** - API documentation
- **.NET Aspire** - Observability and traceability

## 📋 Prerequisites

- .NET SDK 9.0.203
- Docker Desktop with WSL2 support
- WSL2 with Ubuntu 24.04+
- Visual Studio 2022 (recommended) or VS Code
- Git

## 🛠️ Running the Project

The project offers observability via Aspire and can be run using either Aspire or Docker Compose. Just choose your preferred execution mode and follow the instructions for your environment (Visual Studio, CLI, or Docker Desktop).

## 📁 Project Structure

```
_aspire
│   EcommercePortfolio.AppHost
│   EcommercePortfolio.ServiceDefaults
_docker
│   docker-compose
src
│
├── Building Blocks
│   ├── EcommercePortfolio.ApiGateways
│   ├── EcommercePortfolio.Core
│   └── EcommercePortfolio.Services
│
├── Services
│   ├── Carts
│   │   ├── EcommercePortfolio.Carts.API
│   │   ├── EcommercePortfolio.Carts.Domain
│   │   └── EcommercePortfolio.Carts.Infra
│   ├── Deliveries
│   │   ├── EcommercePortfolio.Deliveries.API
│   │   ├── EcommercePortfolio.Deliveries.Domain
│   │   └── EcommercePortfolio.Deliveries.Infra
│   └── Orders
│       ├── EcommercePortfolio.Orders.API
│       ├── EcommercePortfolio.Orders.Domain
│       └── EcommercePortfolio.Orders.Infra
│
├── Tests
│   └── UnitTests
│       └── EcommercePortfolio.Orders.UnitTests
```

## 🏗️ Architecture

The project follows Domain-Driven Design (DDD) and Clean Architecture principles, organized into Bounded Contexts:

### Bounded Contexts
- **Orders** - Order management
- **Deliveries** - Delivery control
- **Payments** - Payment gateway integration
- **Carts** - Shopping cart management

### Layers
- **Domain Layer**: Contains entities, value objects, aggregates, and business rules
- **Application Layer**: Implements use cases, commands, and queries (CQRS)
- **Infrastructure Layer**: Implements data access, external services, and infrastructure
- **API Layer**: Exposes RESTful APIs and handles HTTP requests

### About Building Blocks

The `Building Blocks` projects concentrate reusable components and abstractions shared among different system modules:

- **EcommercePortfolio.ApiGateways**: Gateways for integration with external APIs and inter-module communication.
- **EcommercePortfolio.Core**: Contracts, base entities, value objects, custom exceptions, and common utilities.
- **EcommercePortfolio.Services**: Shared infrastructure and application services, such as messaging, cache, and technical integrations.

## 🧩 Architecture Patterns Used
- **Domain Events**: To propagate events between aggregates and contexts with low coupling.
- **Notification Pattern**: For aggregation and structured exposure of errors.
- **Eventual Consistency via Messaging**: Asynchronous communication with MassTransit + RabbitMQ between domains.
- **Hybrid Caching**: Resilient and adaptable caching implementation (Redis/In-Memory).

## ⚙️ Main Features

- Order creation and management
- Integration with a fake payment service (simulating an external gateway)
- Delivery management
- Validations with FluentValidation
- Well-defined mappings with DTOs and Value Objects
- Observability and traceability with Aspire Dashboard
- Decoupled integrations via interfaces and dependency injection

## 🧪 Tests

The project includes unit and integration tests. To run the tests:
```bash
cd src
dotnet test
```

### Testing Standards

We follow a well-defined set of testing standards to ensure code quality and maintainability. Our testing approach includes:

- **Naming Conventions**: Clear and consistent test naming following the pattern `[Feature]_[Action]_[ExpectedResult]`
- **Test Organization**: Structured separation of unit and integration tests
- **Factory Patterns**: Reusable test data creation
- **Assertion Helpers**: Database-specific assertion utilities
- **Best Practices**: AAA pattern, test isolation, and comprehensive coverage

For detailed information about our testing standards, please refer to [TESTING_STANDARDS.md](TESTING_STANDARDS.md).

## 🤝 How to Contribute

1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is under the MIT license. See the [LICENSE](LICENSE) file for more details.

## 📫 Contact

Leonardo Felipe

LinkedIn: [https://www.linkedin.com/in/leonardo-felipe-b924a769/](https://www.linkedin.com/in/leonardo-felipe-b924a769/)

Email: [leonardo.fdns@gmail.com](mailto:leonardo.fdns@gmail.com)