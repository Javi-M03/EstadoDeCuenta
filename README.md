# EstadoDeCuenta

Aplicación web para visualizar el **estado de cuenta de tarjetas de crédito**: detalle de movimientos, cálculo de cuota mínima, pago de contado, interés bonificable, y saldo utilizado y disponible de la tarjeta.

El proyecto está compuesto por una **REST API** (ASP.NET Core Web API) que expone la lógica de negocio sobre una base de datos **SQL Server**, y un **frontend en Razor Pages** que consume esa API.

## Características

- **Estado de cuenta** de una tarjeta: titular, número, saldo actual, límite y saldo disponible.
- **Compras del mes actual y del mes anterior** (totales) y detalle de movimientos.
- **Interés bonificable**, **cuota mínima**, **pago de contado** y **pago de contado con intereses**, calculados con porcentajes configurables.
- **Registro de movimientos** (compras y pagos) con validaciones de negocio (una compra no puede superar el saldo disponible; un pago no puede superar el saldo actual).
- **Historial de transacciones** con filtros por período (mes actual / todos) y por tipo (compra / pago).
- **Pantalla de movimientos** global con filtro por período (mes actual / mes anterior / todos).
- **Exportar el estado de cuenta a PDF** y **exportar movimientos a Excel**.
- **Configuración editable** de los porcentajes (interés y pago mínimo) desde el frontend, persistida en base de datos.
- Manejo global de excepciones y **health check**.


## Tecnologías y patrones

- **.NET 6**
- **ASP.NET Core Web API** (REST) + **Swagger**
- **Razor Pages**
- **Entity Framework Core 6**
- **SQL Server**
- **Procedimientos almacenados** para las lecturas de movimientos y tarjetas
- **AutoMapper**
- **FluentValidation**
- **CQRS** (Commands / Queries con sus handlers)
- **Unit of Work + Repository**
- **DTOs** para la API y **View Models** para el frontend
- **GlobalExceptions**
- **Health Checks** (incluye verificación de base de datos)
- **QuestPDF** para exportación a PDF  y **ClosedXML** para exportación a Excel

---

## Arquitectura de la solución

La solución (`EstadoDeCuenta.slnx`) está dividida en proyectos por responsabilidad, siguiendo una arquitectura en capas:

 Proyecto: Estado de Cuenta

 **EstadoDeCuenta.API** : Web API REST. Controladores, configuración (DI, Swagger, middleware, health checks). 
 **EstadoDeCuenta.Services** : Lógica de negocio: CQRS (commands/queries + handlers), validadores, cálculo del estado de cuenta, perfiles de AutoMapper. 
 **EstadoDeCuenta.Domain** : Entidades del dominio e interfaces (repositorios, unit of work). 
 **EstadoDeCuenta.Infrastructure** : Acceso a datos con EF Core: 'DbContext', configuraciones, repositorios, unit of work, migraciones. 
 **EstadoDeCuenta.DTOs** : Objetos de transferencia de datos (request/response) de la API. 
 **CardsFrontend** : Frontend en Razor Pages que consume la API mediante un 'HttpClient' tipado. 



## Requisitos previos

- [.NET SDK 6.0] (o superior con compatibilidad para `net6.0`)
- **SQL Server** 
- **Visual Studio 2022**
- **Postman** (para probar la API)
- Herramienta EF Core (ya declarada como *local tool* en `dotnet-tools.json`):

```bash
dotnet tool restore
```

---

## Configuración

### Cadena de conexión (API)

En `EstadoDeCuenta.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EstadoDeCuentaDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Ajusta 'Server' según tu instancia de SQL Server (por ejemplo 'localhost\\SQLEXPRESS' o '(localdb)\\MSSQLLocalDB').

### URL de la API (frontend)

El frontend apunta a la API mediante 'ApiSettings:BaseUrl':

- 'CardsFrontend/appsettings.Development.json' → 'https://localhost:7125/'
- 'CardsFrontend/appsettings.json' → 'https://localhost:7263/'

Asegúrate de que ese valor coincida con el puerto en el que corre la API.

### Porcentajes del estado de cuenta

Los porcentajes de interés y de **pago mínimo** se guardan en la base de datos (tabla 'AccountStatementSettings') y se editan desde la pantalla **Settings** del frontend.

---

## Base de datos

El proyecto usa **EF Core Code First** con migraciones. Para crear la base de datos con todas las tablas, datos semilla y **procesos almacenados**:

```bash
dotnet ef database update --project EstadoDeCuenta.Infrastructure --startup-project EstadoDeCuenta.API
```

### Procesos almacenados

Los scripts también están disponibles de forma independiente en ['Database/StoredProcedures.sql'](Database/StoredProcedures.sql) (idempotentes, 'CREATE OR ALTER') para ejecutarlos manualmente en SSMS o 'sqlcmd':

 |Procedimiento|Descripción|
|-|-|
|`usp\_GetMovementsByCard`|Movimientos de una tarjeta, ordenados por fecha desc.|
|`usp\_GetCardsByClient`|Tarjetas de un cliente.|
|`usp\_GetMovementsByCardFiltered`|Movimientos de una tarjeta con filtro opcional por rango de fechas y tipo.|
|`usp\_GetMovementsFiltered`|Todos los movimientos con filtro opcional por rango de fechas.|


---

## Cómo ejecutar

Necesitas ejecutar **la API** y **el frontend** (en terminales separadas, o como *multiple startup projects* en Visual Studio).

### API

```bash
dotnet run --project EstadoDeCuenta.API
```

- HTTPS: 'https://localhost:7125'
- HTTP: 'http://localhost:5143'
- Swagger UI: 'https://localhost:7125/swagger'

### Frontend

```bash
dotnet run --project CardsFrontend
```

- HTTPS: 'https://localhost:5101'
- HTTP: 'http://localhost:5100'

> Desde Visual Studio puedes iniciar ambos a la vez con la opción **Multiple startup projects** (perfil de inicio de la solución).

---

## Endpoints de la API

Base URL: 'https://localhost:7125'

### Clients

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/clients` | Lista todos los clientes. |
| GET | `/api/clients/{clientId}` | Cliente por Id. |
| GET | `/api/clients/{clientId}/cards` | Tarjetas de un cliente. |
| POST | `/api/clients` | Crea un cliente. |

### Cards

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/cards` | Lista todas las tarjetas. |
| GET | `/api/cards/{cardId}` | Tarjeta por Id. |
| GET | `/api/cards/{cardId}/statement` | Estado de cuenta calculado de la tarjeta. |
| GET | `/api/cards/{cardId}/movements` | Movimientos de la tarjeta. Filtros opcionales: `fromDate`, `toDate`, `movementType`. |
| POST | `/api/clients/{clientId}/cards` | Crea una tarjeta para un cliente. |

### Movements

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/movements` | Lista movimientos. Filtros opcionales: `fromDate`, `toDate`. |
| GET | `/api/movements/{movementId}` | Movimiento por Id. |
| POST | `/api/cards/{cardId}/movements` | Crea un movimiento (compra o pago) para una tarjeta. |

### Settings

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/settings/account-statement` | Porcentajes configurables (interés y pago mínimo). |
| PUT | `/api/settings/account-statement` | Actualiza los porcentajes (0–100). |

### Health

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/health` | Health check (incluye base de datos). |


> `movementType`: `0` = Compra, `1` = Pago. En el cuerpo JSON (POST) se envía como número; en el filtro de query se acepta `Compra`/`Pago`.

---

## Cómo probar

### Swagger

Con la API en ejecución, abre `https://localhost:7125/swagger` para explorar y probar todos los endpoints desde el navegador.

### Postman

En la carpeta [`Postman/`](Postman) se incluye la colección [`EstadoDeCuenta.postman_collection.json`](Postman/EstadoDeCuenta.postman_collection.json).

1. Abre Postman → **File → Import** → selecciona el archivo (o arrástralo a la ventana).
2. Ajusta la variable `baseUrl` si tu API usa otro puerto.
3. Envía cualquier petición. Los POST/PUT ya traen un cuerpo de ejemplo.

> Si Postman muestra un error de certificado SSL con `localhost`, desactiva **Settings → SSL certificate verification**, o usa la URL HTTP.

### Frontend

Con la API y el frontend en ejecución, abre `https://localhost:5101` y navega por **Clientes → Tarjetas → Estado de cuenta / Movimientos / Historial / Settings/ Estado**.

---

## Cálculo del estado de cuenta

Con los porcentajes configurables (por defecto **Interés = 25%**, **Pago mínimo = 5%**):

- **Saldo Total** = Total compras − Total pagos
- **Saldo disponible** = Límite de la tarjeta − Saldo total
- **Interés bonificable** = Saldo total × % interés
- **Cuota mínima** = Saldo total × % pago mínimo
- **Pago de contado** = Saldo total
- **Pago de contado con intereses** = Saldo total + Interés bonificable

Ejemplo (Saldo Total = $114.47, Interés = 25%, Pago mínimo = 5%):

- Interés bonificable = 114.47 × 25% = **$28.61**
- Cuota mínima = 114.47 × 5% = **$5.72**
- Pago de contado con intereses = 114.47 + 28.61 = **$143.08**
