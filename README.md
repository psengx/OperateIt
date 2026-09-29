# OperateIt

Микросервисная система складского учёта.

## Архитектура

Система состоит из двух независимых сервисов, общающихся по **gRPC**:
| Сервис | Назначение |
|--------|-----------|
| **ProductService** | Хранилище товаров и их количества на складе |
| **ReceptionService** | Создание и оприходование накладных приёмки, которые пополняют остатки в ProductService |

# Стек

- **.NET 10** (ASP.NET Core Web API)
- **gRPC** — межсервисное взаимодействие
- **Entity Framework Core** — ORM и миграции
- **PostgreSQL** — хранение товаров и накладных
- **Docker / Docker Compose** — контейнеризация

## Структура репозитория

```
OperateIt/
├── ProductService/  # сервис товаров и остатков
│ ├── Controllers/  # REST/gRPC-эндпоинты
│ ├── Data/  # DbContext, конфигурация EF Core
│ ├── Dto/  # DTO для API
│ ├── Grpc/  # gRPC-сервис
│ ├── Migrations/  # миграции EF Core
│ ├── Models/  # доменные модели (Product)
│ ├── Protos/  # .proto-контракты
│ └── Services/  # бизнес-логика
├── ReceptionService/  # сервис накладных
│ ├── Controllers/ 
│ ├── Data/
│ ├── Dto/
│ ├── Migrations/
│ ├── Models/  # Reception
│ ├── Protos/   # .proto-контракты
└──── Services/
```

## API

Все эндпоинты возвращают JSON. Базовые маршруты:

- **ProductService** — `http://localhost:5001`
- **ReceptionService** — `http://localhost:5002`

### ProductService — `/products`

Справочник товаров и их остатков на складе.

| Метод | Маршрут | Описание | Тело запроса | Ответ |
|-------|---------|----------|--------------|-------|
| `GET` | `/products` | Получить список всех товаров с остатками | — | `200 OK` + список товаров |
| `GET` | `/products/{id}` | Получить товар по `Guid` | — | `200 OK` + товар / `404 Not Found` |
| `POST` | `/products` | Создать новый товар | `CreateProductRequest` | `201 Created` |
| `DELETE` | `/products/{id}` | Удалить товар по `Guid` | — | `200 OK` / `404 Not Found` |

#### Пример: создание товара

```http
POST /products
Content-Type: application/json

{
  "name": "Ноутбук Ultra",
  "stock": 120,
  "price": 49000
}
```

#### Пример: получение товара
```http
GET /products/3fa85f64-5717-4562-b3fc-2c963f66afa6
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Ноутбук Ultra",
  "stock": 120,
  "price": 49000
}
```

### ReceptionService — /reception

Накладные приёмки. При оприходовании (GET /id) накладной по gRPC вызывается ProductService и остатки товара увеличиваются, накладная при этом удаляется из списка накладных.

| Метод | Маршрут | Описание | Тело запроса | Ответ |
|-------|---------|----------|--------------|-------|
| `GET` | `/products` | Получить список всех накладных | — | `200 OK` + список неоприходованных накладных |
| `POST` | `/products/{id}/accept` | Принять накладную по Guid — вызывает gRPC-метод AcceptReception в ProductService и пополняет остаток товара | — | `200 OK` + ответ gRPC / `404 Not Found` |
| `POST` | `/products` | Создать новую накладную приёмки | `CreateReceptionRequest` | `201 Created` |

#### Пример: создание накладной

```http
POST /reception
Content-Type: application/json

{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productQuantity": 10
}
```

#### Пример: оприходование накладной

```http
POST /reception/8c8f9f2e-1a2b-4c3d-9e4f-5a6b7c8d9e0f/accept
```

Внутри выполняется gRPC-вызов:
```protobuf
rpc AcceptReception (ReceptionRequest) returns (ReceptionReply);

message ReceptionRequest {
  string product_id = 1;
  int32  quantity   = 2;
}
```

ProductService увеличивает остаток товара product_id на quantity и возвращает ReceptionReply (статус запроса, обновленное число остатков, сообщение)
