# FCG-MS-CatalogAPI

Microsserviço responsável pelo catálogo de jogos e pela biblioteca pessoal de cada usuário na plataforma **FIAP Cloud Games**.

## Responsabilidades

- CRUD de jogos (administradores)
- Listagem pública do catálogo
- Início do processo de compra via evento `OrderPlacedEvent`
- Adição do jogo à biblioteca do usuário após confirmação de pagamento (`PaymentProcessedEvent`)

## Fluxo de eventos

```
CatalogAPI  → [OrderPlacedEvent]     → PaymentsAPI
PaymentsAPI → [PaymentProcessedEvent] → CatalogAPI (adiciona à biblioteca)
```

---

## Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/9.0) | 9.0 |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | 24+ |
| SQL Server | 2019+ (via Docker) |
| RabbitMQ | 3.13+ (via Docker) |

---

## Variáveis de ambiente

| Variável | Descrição | Valor padrão (dev) |
|---|---|---|
| `ConnectionStrings__CatalogDB` | Connection string do SQL Server | `Server=localhost,1433;Database=CatalogDB;User Id=sa;Password=Sa12345678!;TrustServerCertificate=True` |
| `JwtSettings__SecretKey` | Chave secreta JWT (mesma do UsersAPI) | `S3cr3tK3y!@#_JWT_FCG_2024$%^` |
| `JwtSettings__ExpiracaoHoras` | Validade do token em horas | `2` |
| `RabbitMQ__Host` | Host do RabbitMQ | `localhost` |
| `RabbitMQ__VHost` | Virtual host | `/` |
| `RabbitMQ__Username` | Usuário | `guest` |
| `RabbitMQ__Password` | Senha | `guest` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente | `Development` |

> `JwtSettings__SecretKey` deve ser **idêntica** à configurada no UsersAPI.

---

## Como executar

### 1. Subir infraestrutura (SQL Server + RabbitMQ)

```bash
# SQL Server (pule se já estiver rodando)
docker run -d --name sqlserver \
  -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sa12345678!" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2019-latest

# RabbitMQ com painel de gestão (acesso em http://localhost:15672)
docker run -d --name rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

### 2. Executar via .NET CLI (desenvolvimento)

```bash
cd CatalogAPI.Api
dotnet run
```

- API: `http://localhost:5002`
- Swagger: `http://localhost:5002/swagger`

### 3. Executar via Docker (imagem isolada)

```bash
# Build a partir da raiz do repositório
docker build -t fcg-catalogapi:latest .

# Run
docker run -d --name catalogapi -p 5002:8080 \
  -e "ConnectionStrings__CatalogDB=Server=host.docker.internal,1433;Database=CatalogDB;User Id=sa;Password=Sa12345678!;TrustServerCertificate=True" \
  -e "JwtSettings__SecretKey=S3cr3tK3y!@#_JWT_FCG_2024$%^" \
  -e "JwtSettings__ExpiracaoHoras=2" \
  -e "RabbitMQ__Host=host.docker.internal" \
  -e "RabbitMQ__Username=guest" \
  -e "RabbitMQ__Password=guest" \
  fcg-catalogapi:latest
```

- Swagger: `http://localhost:5002/swagger`

### 4. Executar via Kubernetes

```bash
kubectl apply -f k8s/
kubectl get pods -n fiapcloudgames
kubectl logs -l app=catalogapi -n fiapcloudgames
```

---

## Endpoints

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/api/jogos` | Listar todos os jogos | Não |
| GET | `/api/jogos/{id}` | Detalhes de um jogo | Não |
| POST | `/api/jogos` | Cadastrar jogo | Admin |
| PUT | `/api/jogos/{id}` | Atualizar jogo | Admin |
| DELETE | `/api/jogos/{id}` | Remover jogo | Admin |
| POST | `/api/biblioteca/{jogoId}/comprar` | Iniciar compra de um jogo | Sim |
| GET | `/api/biblioteca` | Listar biblioteca do usuário logado | Sim |
| GET | `/api/biblioteca/{userId}` | Listar biblioteca de um usuário | Admin |

### Exemplo: Listar jogos

```bash
curl http://localhost:5002/api/jogos
```

### Exemplo: Comprar jogo (requer token JWT do UsersAPI)

```bash
curl -X POST http://localhost:5002/api/biblioteca/1/comprar \
  -H "Authorization: Bearer SEU_TOKEN_JWT"
```

---

## Executar testes

```bash
cd CatalogAPI.Tests
dotnet test
```

---

## Estrutura do projeto

```
FCG-MS-CatalogAPI/
├── Dockerfile
├── k8s/
├── CatalogAPI.Api/           # Controllers, Program.cs, Extensions
├── CatalogAPI.Application/   # Services, Consumers (OrderPlaced, PaymentProcessed)
├── CatalogAPI.Domain/        # Entities, Interfaces, Events (inline)
├── CatalogAPI.Infrastructure/ # EF Core, Repositories
└── CatalogAPI.Tests/         # xUnit + Moq
```

---

## Observações

- O campo `UsuarioId` em `ItemBiblioteca` é uma referência lógica — não há FK entre bancos (Database per Service).
- A compra publica `OrderPlacedEvent` e aguarda a confirmação via `PaymentProcessedEvent` do PaymentsAPI.

---

## Tecnologias

- .NET 9 / ASP.NET Core 9
- Entity Framework Core 9 + SQL Server
- MassTransit 8.3.6 + RabbitMQ
- JWT Bearer Authentication
- xUnit + Moq
