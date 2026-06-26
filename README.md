# CourierMax API

API REST para la gestión de envíos de un courier: creación de envíos, asignación a conductores/vehículos, seguimiento del ciclo de estados, cálculo de tarifas, detección de envíos atrasados (SLA con días hábiles y festivos) y métricas de eficiencia por conductor.

Desarrollada en **.NET 8 / ASP.NET Core** siguiendo **Clean Architecture** y el patrón **CQRS** con MediatR.

> 🌐 **Swagger:** https://couriermaxapi.azurewebsites.net/swagger/index.html

---

## Tabla de contenido

- [Tecnologías utilizadas](#tecnologías-utilizadas)
- [Arquitectura](#arquitectura)
- [Estructura del repositorio](#estructura-del-repositorio)
- [Requisitos previos](#requisitos-previos)
- [Ejecución local paso a paso](#ejecución-local-paso-a-paso)
- [Pruebas automatizadas](#pruebas-automatizadas)
- [Endpoints de la API](#endpoints-de-la-api)
- [Ejemplos de llamadas (curl)](#ejemplos-de-llamadas-curl)
- [Colección de Postman](#colección-de-postman)
- [CI/CD](#cicd)

---

## Tecnologías utilizadas

| Categoría | Tecnología |
|---|---|
| Framework | .NET 8 / ASP.NET Core (Web API) |
| Lenguaje | C# |
| Base de datos | SQL Server |
| ORM | Entity Framework Core 8 |
| Mensajería interna (CQRS) | MediatR 12 |
| Validación | FluentValidation |
| Mapeo de objetos | AutoMapper |
| Resultado funcional de errores | ErrorOr |
| Autenticación | JWT Bearer |
| Documentación | Swagger / OpenAPI (Swashbuckle) |
| Gestión de secretos | Azure Key Vault |
| Pruebas | xUnit + FluentAssertions |
| CI/CD | GitHub Actions → Azure App Service |

---

## Arquitectura

El proyecto aplica **Clean Architecture** (arquitectura por capas con las dependencias apuntando hacia el dominio) combinada con **CQRS** (separación de comandos y consultas) mediante MediatR.

```
┌─────────────────────────────────────────────┐
│                   Api                         │  ← Controllers, JWT, Swagger, DI
│   (capa de presentación / entrada HTTP)       │
└───────────────────┬─────────────────────────┘
                    │ depende de
┌───────────────────▼─────────────────────────┐
│                Aplicacion                     │  ← Commands, Queries, Handlers,
│   (casos de uso / orquestación)               │    Validators, DTOs (CQRS)
└───────────────────┬─────────────────────────┘
                    │ depende de
┌───────────────────▼─────────────────────────┐
│                 Dominio                       │  ← Entidades, Value Objects,
│   (reglas de negocio puras)                   │    interfaces de repositorio
└───────────────────▲─────────────────────────┘
                    │ implementa
┌───────────────────┴─────────────────────────┐
│        Arquitectura (Infraestructura)         │  ← EF Core, repositorios,
│   (detalles técnicos: BD, servicios)          │    JwtTokenGenerator
└─────────────────────────────────────────────┘
```

### Capas

- **Dominio** — El núcleo. Contiene las entidades (`EnvioD`, `ConductorD`, etc.), value objects (`Remitente`, `Destinatario`, `Paquete`), la lógica de negocio (máquina de estados del envío, cálculo de costo, días hábiles) y las **interfaces** de los repositorios. No depende de nada externo.
- **Aplicacion** — Los casos de uso, organizados en *commands* (escritura) y *queries* (lectura), cada uno con su `Handler`. Aquí viven las validaciones (FluentValidation) y los DTOs de respuesta. Depende solo de Dominio.
- **Arquitectura (Infraestructura)** — Las implementaciones concretas: `ApplicationDbContext` (EF Core), los repositorios y el generador de JWT. Implementa las interfaces declaradas en Dominio.
- **Api** — La capa de entrada. Controllers delgados que solo reciben la petición, delegan al `MediatR` y traducen el resultado (`ErrorOr`) a códigos HTTP. Configura autenticación, Swagger y la inyección de dependencias.

### Justificación de las decisiones

- **¿Por qué Clean Architecture?** Aísla las reglas de negocio de los detalles técnicos (base de datos, framework web). El dominio no sabe que existe SQL Server ni HTTP, lo que lo hace fácil de **probar de forma unitaria** (ver la carpeta de tests) y permite cambiar de infraestructura sin tocar la lógica.
- **¿Por qué CQRS con MediatR?** Separar lectura y escritura mantiene cada caso de uso pequeño y enfocado (un `Handler` = una responsabilidad). Los controllers quedan triviales y se evita un "service" gigante. Además, el `ValidationBehavior` intercepta todos los comandos para validarlos antes de ejecutar la lógica.
- **¿Por qué ErrorOr?** Permite que los handlers devuelvan errores de negocio (no encontrado, validación, conflicto) sin lanzar excepciones para el flujo normal. El `ApiController` base traduce cada tipo de error al código HTTP correcto (400, 404, 409, etc.).
---

## Estructura del repositorio

```
CourierMaxApi/
├── Api/                      # Capa de presentación (ASP.NET Core Web API)
│   ├── Controllers/          # AuthController, EnviosController, MetricasController, ...
│   ├── Properties/           # DependencyInjection (JWT + Swagger)
│   ├── CourierMaxApi.sln     # Solución de Visual Studio
│   └── CourierMaxApi.csproj
├── Aplicacion/               # Casos de uso (CQRS: Commands/Queries/Handlers/Validators)
├── Dominio/                  # Entidades, Value Objects, interfaces, lógica de negocio
├── Arquitectura/             # Infraestructura: EF Core, repositorios, servicios
├── DiseñoBD/
│   └── ScriptCourierMaxSQL.sql   # Script de creación de BD + datos semilla
├── Tests/
│   └── CourierMaxApi.UnitTests/  # Pruebas unitarias (xUnit + FluentAssertions)
└── .github/workflows/        # Pipeline de CI/CD
```

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (LocalDB, Express o una instancia completa)
- Opcional: [Visual Studio 2022](https://visualstudio.microsoft.com/) o VS Code

---

## Ejecución local paso a paso

### 1. Clonar el repositorio

```bash
git clone https://github.com/Kuroko0512/CourierMaxApi.git
cd CourierMaxApi
```

### 2. Crear la base de datos

Ejecuta el script `DiseñoBD/ScriptCourierMaxSQL.sql` en tu instancia de SQL Server. El script crea las tablas (con prefijo `tbl_`) e inserta los **datos semilla** (ciudades, roles, usuarios, tipos de servicio/paquete, tarifas, festivos).

**Base de datos en Azure SQL** (entorno de la prueba):

| Parámetro | Valor |
|---|---|
| Servidor | `serverpruebajohan.database.windows.net` |
| Usuario | `adminsql` |
| Contraseña | `-------` |
| Base de datos | `BdCourierMax` |

Cadena de conexión equivalente:

```
Server=serverpruebajohan.database.windows.net;Database=BdCourierMax;User Id=adminsql;Password=-------;TrustServerCertificate=True;
```

### 3. Configurar la cadena de conexión y el JWT

La aplicación está configurada para leer sus secretos desde **Azure Key Vault**. Para correr **100% en local sin Azure**, desactiva Key Vault y provee los valores tú mismo.

Desactiva Key Vault en local editando `Api/appsettings.json`:

```json
"KeyVault": {
    "Uri": "<URI_DEL_KEY_VAULT>",
    "TenantId": "<TENANT_ID>",
    "ClientId": "<CLIENT_ID>",
    "ClientSecret": "<CLIENT_SECRET>"
  }
```

> Estos valores son secretos: no los subas al repositorio. Provéelos en local mediante *user-secrets* o variables de entorno.

### 4. Ejecutar la API

```bash
dotnet run --project Api/CourierMaxApi.csproj
```

La API quedará disponible y, en entorno *Development*, Swagger UI estará en:

```
https://localhost:<puerto>/swagger
```

(El puerto se muestra en la consola al arrancar; revisa también `Api/Properties/launchSettings.json`.)

> La API también está **desplegada en Azure App Service** y su Swagger es accesible en:
> https://couriermaxapi.azurewebsites.net/swagger/index.html

---

## Pruebas automatizadas

El proyecto incluye pruebas unitarias sobre la lógica de negocio del dominio y los validadores.

```bash
# Ejecutar todas las pruebas
dotnet test Tests/CourierMaxApi.UnitTests

# Filtrar por una clase
dotnet test Tests/CourierMaxApi.UnitTests --filter "FullyQualifiedName~EnvioDTests"
```

Cubren: la máquina de estados del envío, el cálculo de costos, la detección de atrasos (SLA), el cálculo de días hábiles (excluyendo fines de semana y festivos) y las reglas de validación de creación de envíos.

---

## Endpoints de la API

> Todos los endpoints requieren autenticación JWT **excepto** los marcados como _público_. Envía el token en la cabecera `Authorization: Bearer <token>`.

### Autenticación

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Inicia sesión y devuelve un JWT | Público |

### Envíos

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/envios` | Crea un envío | Sí |
| GET | `/api/envios` | Lista todos los envíos | Sí |
| GET | `/api/envios/{codigo}` | Consulta un envío por su código de rastreo | Público |
| PATCH | `/api/envios/{id}/estado` | Cambia el estado de un envío | Sí |
| GET | `/api/envios/atrasados?desde=&hasta=` | Envíos atrasados en un rango de fechas | Sí |

### Catálogos y métricas

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| GET | `/api/rol` | Lista de roles | Sí |
| GET | `/api/tipopaquete` | Tipos de paquete | Sí |
| GET | `/api/tiposervicio` | Tipos de servicio | Sí |
| GET | `/api/metricas/conductores?desde=&hasta=` | Métricas de eficiencia por conductor | Sí |

### Datos semilla útiles para probar

- **Usuarios**: `Operador 1`, `Sistema` — **contraseña**: `Courier123*`
- **Ciudades**: 1=Bogotá, 2=Medellín, 3=Cali, 4=Barranquilla
- **Tipos de servicio**: 1=ESTÁNDAR, 2=EXPRESS, 3=MISMO_DÍA
- **Tipos de paquete**: 1=DOCUMENTO, 2=PAQUETE, 3=FRÁGIL, 4=PERECEDERO

---

## Ejemplos de llamadas (curl)

> Reemplaza `https://localhost:7000` por el puerto real de tu entorno.

### 1. Login (obtener token)

```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usuario": "Operador 1",
    "password": "Courier123*"
  }'
```

Respuesta:

```json
{
  "usuarioId": 2,
  "nombre": "Operador 1",
  "rol": "OPERADOR",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```

### 2. Crear un envío

```bash
curl -X POST https://localhost:7000/api/envios \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <TOKEN>" \
  -d '{
    "tipoServicioId": 1,
    "remitenteNombre": "Ana Pérez",
    "remitenteTelefono": "3001234567",
    "remitenteDireccionRecogida": "Calle 1 #2-3",
    "destinatarioNombre": "Luis Gómez",
    "destinatarioTelefono": "3007654321",
    "destinatarioDireccionEntrega": "Calle 4 #5-6",
    "pesoKg": 2.5,
    "largoCm": 30,
    "anchoCm": 20,
    "altoCm": 10,
    "tipoPaqueteId": 2,
    "ciudadOrigenId": 1,
    "ciudadDestinoId": 2
  }'
```

### 3. Consultar un envío por código (público)

```bash
curl https://localhost:7000/api/envios/CM-00012345
```

### 4. Listar todos los envíos

```bash
curl https://localhost:7000/api/envios \
  -H "Authorization: Bearer <TOKEN>"
```

### 5. Cambiar el estado de un envío

```bash
curl -X PATCH https://localhost:7000/api/envios/1/estado \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <TOKEN>" \
  -d '{
    "nuevoEstado": "ASIGNADO",
    "motivo": null
  }'
```

> Estados válidos: `CREADO`, `ASIGNADO`, `EN_TRANSITO`, `ENTREGADO`, `CANCELADO`.
> Cancelar requiere un `motivo` de al menos 5 caracteres.

### 6. Envíos atrasados en un rango

```bash
curl "https://localhost:7000/api/envios/atrasados?desde=2026-01-01&hasta=2026-12-31" \
  -H "Authorization: Bearer <TOKEN>"
```

### 7. Métricas por conductor

```bash
curl "https://localhost:7000/api/metricas/conductores?desde=2026-01-01&hasta=2026-12-31" \
  -H "Authorization: Bearer <TOKEN>"
```

---

## Colección de Postman

En la raíz del repositorio encontrarás **`CourierMax.postman_collection.json`**. Para usarla:

1. Abre Postman → **Import** → selecciona el archivo.
2. Ajusta la variable de colección `baseUrl` al puerto de tu entorno (por defecto `https://localhost:7000`).
3. Ejecuta primero **Auth → Login**: el token se guarda automáticamente en la variable `token` y lo reutilizan el resto de peticiones.

---

## CI/CD

El repositorio incluye un workflow de GitHub Actions (`.github/workflows/main_couriermaxapi.yml`) que en cada push a `main`:

1. Restaura y **compila** la solución (`dotnet build`).
2. Ejecuta las **pruebas** (`dotnet test`); si fallan, se detiene el despliegue.
3. Publica y **despliega** a Azure App Service.

---
