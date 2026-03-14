# CatalogAPI

Microsserviço responsável pelo catálogo de jogos e pela biblioteca pessoal de cada usuário na plataforma FIAP Cloud Games.

## Responsabilidades

- CRUD de jogos (administradores)
- Listagem pública do catálogo
- Início do processo de compra via evento `OrderPlacedEvent`
- Adição do jogo à biblioteca do usuário após confirmação de pagamento (`PaymentProcessedEvent`)

## Fluxo de eventos

```
CatalogAPI → OrderPlacedEvent → PaymentsAPI
PaymentsAPI → PaymentProcessedEvent → CatalogAPI (adiciona à biblioteca)
```

## Endpoints

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/jogos | Listar todos os jogos | Não |
| GET | /api/jogos/{id} | Detalhes de um jogo | Não |
| POST | /api/jogos | Cadastrar jogo | Admin |
| PUT | /api/jogos/{id} | Atualizar jogo | Admin |
| DELETE | /api/jogos/{id} | Remover jogo | Admin |
| POST | /api/biblioteca/{jogoId}/comprar | Iniciar compra de um jogo | Sim |
| GET | /api/biblioteca | Listar biblioteca do usuário logado | Sim |

## Variáveis de ambiente

| Variável | Descrição | Exemplo |
|---|---|---|
| `ConnectionStrings__CatalogDB` | Connection string do SQL Server | `Server=sqlserver;Database=CatalogDB;User Id=sa;Password=...` |
| `JwtSettings__SecretKey` | Chave secreta para validar tokens JWT (compartilhada com UsersAPI) | `S3cr3tK3y!@#_JWT_2024$%...` |
| `JwtSettings__ExpiracaoHoras` | Validade do token em horas | `2` |
| `RabbitMQ__Host` | Host do broker RabbitMQ | `rabbitmq` |
| `RabbitMQ__VHost` | Virtual host do RabbitMQ | `/` |
| `RabbitMQ__Username` | Usuário do RabbitMQ | `guest` |
| `RabbitMQ__Password` | Senha do RabbitMQ | `guest` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Production` |

## Executar localmente

```bash
# A partir da raiz da solução
docker-compose up --build catalogapi
```

Ou via Docker isolado:

```bash
docker build -f CatalogAPI/CatalogAPI.Api/Dockerfile -t catalogapi:latest .
docker run -p 5002:8080 catalogapi:latest
```

Swagger disponível em: `http://localhost:5002/swagger`

## Observações

- O campo `UsuarioId` em `ItemBiblioteca` é uma referência lógica ao usuário — não existe chave estrangeira entre bancos (Database per Service).
- A compra não é processada diretamente: o serviço publica um evento e aguarda a confirmação do PaymentsAPI via `PaymentProcessedEvent`.

## Tecnologias

- .NET 9
- Entity Framework Core 9 + SQL Server
- MassTransit 8 + RabbitMQ
- JWT Bearer Authentication
