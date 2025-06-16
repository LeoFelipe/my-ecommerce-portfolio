# 🛍️ [Ecommerce Portfolio API](https://github.com/LeoFelipe/my-ecommerce-portfolio)

[![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Docker](https://img.shields.io/badge/Docker-Container-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![Docker Compose](https://img.shields.io/badge/Docker%20Compose-Orchestration-2496ED?logo=docker&logoColor=white)](https://docs.docker.com/compose/)
[![Aspire](https://img.shields.io/badge/Aspire-Observability-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspire)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework%20Core-9.0-green?logo=dotnet)](https://learn.microsoft.com/en-us/ef/core/)
[![CQRS](https://img.shields.io/badge/CQRS-Pattern-blue)](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
[![DDD](https://img.shields.io/badge/DDD-Domain--Driven%20Design-blue)](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)
[![MediatR](https://img.shields.io/badge/MediatR-Application%20Mediator-ff69b4)](https://github.com/jbogard/MediatR)
[![FluentValidation](https://img.shields.io/badge/FluentValidation-Validation-blue)](https://docs.fluentvalidation.net/)
[![Scalar](https://img.shields.io/badge/Scalar-API%20Docs-4B275F)](https://scalar.com/)
[![Domain Events](https://img.shields.io/badge/Domain%20Events-Pattern-orange)](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation)
[![Notification Pattern](https://img.shields.io/badge/Notification%20Pattern-Error%20Handling-FF6B6B)](https://martinfowler.com/articles/replaceThrowWithNotification.html)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Message%20Broker-FF6600?logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![MassTransit](https://img.shields.io/badge/MassTransit-Message%20Bus-00A4EF)](https://masstransit.io/)
[![Redis](https://img.shields.io/badge/Redis-Distributed%20Cache-DC382D?logo=redis&logoColor=white)](https://redis.io/)
[![HybridCache](https://img.shields.io/badge/HybridCache-Multi%20Layer%20Cache-6DB33F)](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This is a personal portfolio project developed with a focus on software architecture best practices, scalability, and maintainability.

## 🎯 Overview

This is a modular monolith structured by Bounded Contexts, inspired by DDD (Domain-Driven Design) and microservices principles. The system implements a complete e-commerce solution with:

- **Orders Management**: Complete order lifecycle with payment integration
- **Shopping Cart**: Redis-based cart management with expiration
- **Delivery Tracking**: Real-time delivery status updates
- **Payment Processing**: Integration with external payment gateway

## 🎓 Motivation and Goals

- Demonstrate modern .NET 9 architecture and best practices
- Implement DDD concepts (Aggregates, Value Objects, Domain Events)
- Showcase microservices patterns in a modular monolith
- Provide a production-ready e-commerce solution
- Use .NET Aspire for observability and monitoring

## 🚀 Technologies and Practices

### Core Framework
- **.NET 9.0** (Preview) - Latest Microsoft framework
- **ASP.NET Core** - High-performance web framework
- **Entity Framework Core 9.0** - Modern ORM
- **MongoDB Driver 3.4.0** - Document database access

### Architecture & Patterns
- **Domain-Driven Design (DDD)** - Business-focused architecture
- **CQRS** - Command and Query Responsibility Segregation
- **Event-Driven Architecture** - Asynchronous communication
- **Clean Architecture** - Clear separation of concerns
- **Modular Monolith** - Bounded Contexts with clear boundaries

### Infrastructure
- **PostgreSQL 16** - Relational database
- **MongoDB 7.0** - Document database
- **Redis 7.2** - Distributed caching
- **RabbitMQ 3.12** - Message broker
- **Docker & Docker Compose** - Containerization
- **.NET Aspire** - Observability and monitoring

### Application Features
- **MediatR** - Application mediator
- **FluentValidation** - Object validation
- **MassTransit** - Message bus implementation
- **Scalar** - API documentation
- **HybridCache** - Multi-layer caching

## 📋 Prerequisites

- **.NET SDK 9.0.203** (Preview)
- **Docker Desktop** with WSL2 support
- **WSL2** with Ubuntu 24.04+
- **Visual Studio 2022** (recommended) or VS Code
- **Git**

## 🛠️ Running the Project

The project offers observability via Aspire and can be run using either Aspire or Docker Compose. Just choose 
your preferred execution mode and follow the instructions for your environment (Visual Studio, CLI, or Docker 
Desktop).

1. **Clone the repository**
   ```bash
   git clone https://github.com/LeoFelipe/my-ecommerce-portfolio.git
   cd my-ecommerce-portfolio
   ```

2. **Choose your environment**
   - **Aspire** (recommended for development):
     ```bash
     cd src/aspire
     dotnet run --project EcommercePortfolio.AppHost
     ```
   - **Docker Compose**:
     ```bash
     cd src
     docker-compose up -d
     ```

3. **Access the applications**
   - **Orders API**: http://localhost:5150
   - **Carts API**: http://localhost:5050
   - **Deliveries API**: http://localhost:5250
   - **Aspire Dashboard**: http://localhost:8080

> ℹ️ See [ENVIRONMENT_GUIDE.md](ENVIRONMENT_GUIDE.md) for detailed environment setup and configuration.

## 📁 Project Structure

```
src/
├── building-blocks/
│   ├── EcommercePortfolio.ApiGateways/    # External API integrations
│   ├── EcommercePortfolio.Core/           # Shared domain and contracts
│   └── EcommercePortfolio.Services/       # Shared infrastructure
│
├── services/
│   ├── Carts/                             # Shopping cart context
│   │   ├── EcommercePortfolio.Carts.API
│   │   ├── EcommercePortfolio.Carts.Domain
│   │   └── EcommercePortfolio.Carts.Infra
│   │
│   ├── Orders/                            # Order management context
│   │   ├── EcommercePortfolio.Orders.API
│   │   ├── EcommercePortfolio.Orders.Domain
│   │   └── EcommercePortfolio.Orders.Infra
│   │
│   └── Deliveries/                        # Delivery tracking context
│       ├── EcommercePortfolio.Deliveries.API
│       ├── EcommercePortfolio.Deliveries.Domain
│       └── EcommercePortfolio.Deliveries.Infra
│
├── tests/
│   ├── EcommercePortfolio.FunctionalTests/  # End-to-end tests
│   └── EcommercePortfolio.*.UnitTests/      # Unit tests per service
│
├── aspire/                                # .NET Aspire host
│   ├── EcommercePortfolio.AppHost
│   └── EcommercePortfolio.ServiceDefaults
│
└── docker-compose.yml                     # Docker Compose configuration
```

## 🏗️ Architecture

### Bounded Contexts

Each context is a self-contained module with its own domain model, database, and API:

- **Orders Context**
  - Order management and processing
  - Payment integration
  - Domain events for order status

- **Carts Context**
  - Shopping cart management
  - Redis-based cart storage
  - Cart expiration and cleanup

- **Deliveries Context**
  - Delivery tracking
  - Status updates
  - Integration with order context

### Communication Patterns

- **Synchronous**: HTTP/REST for direct service calls
- **Asynchronous**: MassTransit + RabbitMQ for events
- **Caching**: Redis for distributed caching
- **Documentation**: Scalar for API documentation

## 🧪 Testing

The project includes comprehensive test coverage:

```bash
# Run all tests
cd src
dotnet test

# Run specific test project
dotnet test tests/EcommercePortfolio.FunctionalTests
```

### Test Categories

1. **Unit Tests**
   - Test individual components
   - Use mocks for dependencies
   - Fast execution

2. **Functional Tests**
   - Test complete workflows
   - Use Testcontainers
   - Verify cross-service communication

> ℹ️ See [TESTING_STANDARDS.md](TESTING_STANDARDS.md) for detailed testing guidelines.

## 📚 Documentation

- [ENVIRONMENT_GUIDE.md](ENVIRONMENT_GUIDE.md) - Environment setup and configuration
- [TESTING_STANDARDS.md](TESTING_STANDARDS.md) - Testing guidelines and standards
- [API Documentation](http://localhost:5050/scalar) - API documentation (when running)

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📫 Contact

**Leonardo Felipe**

- LinkedIn: [leonardo-felipe-b924a769](https://www.linkedin.com/in/leonardo-felipe-b924a769/)
- Email: [leonardo.fdns@gmail.com](mailto:leonardo.fdns@gmail.com)
- GitHub: [@LeoFelipe](https://github.com/LeoFelipe)
