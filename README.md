# API de Calidad del Aire (.NET 10 + JWT + EF Core)

API RESTful para gestionar lecturas de calidad del aire en una planta industrial.  
Permite registrar lecturas desde sensores, generar alertas automáticas según umbrales definidos y consultar lecturas (incluyendo una vista enriquecida con datos de clima externo).

Nota:Si no le aparece el proyecto api para ejecutar clik derecho y establecer como proyecto de inicio para ejecutar la app

## Tecnologías

- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core (Code First)
- SQL Server / MySQL (según conexión configurada)
- JWT (System.IdentityModel.Tokens.Jwt)
- BCrypt.Net-Next (hash de contraseñas)
- OpenWeather API (clima actual para San Salvador)
- Swagger / OpenAPI

## Arquitectura por capas

Solución organizada en proyectos:

- `ParcialFJCO.Domain`  
  Entidades (`Usuario`, `SensorCalidadAire`, `LecturaAire`, `AlertaAire`) y lógica de negocio pura  
  (`AlertaAireFactory` para reglas de alertas).

- `ParcialFJCO.Application`  
  Interfaces de servicios (`ILoginService`, `ITokenService`, `ILecturasService`, `IAlertasService`, `IWeatherService`)  
  y DTOs para requests/responses.

- `ParcialFJCO.Infraestructure`  
  `AppDbContext` (EF Core), servicios concretos (`AuthService`, `TokenService`, `LecturasService`,  
  `AlertasService`, `WeatherService`), migraciones y seeding de datos.

- `ParcialFJCO.API`  
  Controladores (`Login`, `Lecturas`, `Alertas`), configuración de JWT, Swagger y DI en `Program.cs`.

## Configuración

### 1. Connection string

Editar `appsettings.json` (o `appsettings.Development.json`) en el proyecto `ParcialFJCO.API`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ParcialFJCO;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

- Cambia `Server=.;` y `Database=ParcialFJCO` según tu entorno.
- Si usas MySQL, cambia también el provider en `AppDbContext`/`Program.cs` (UseMySql en lugar de UseSqlServer).

### 2. Configuración de JWT

En el mismo `appsettings`:

```json
"JwtSettings": {
  "SecretKey": "clave-super-secreta-para-el-parcial",
  "ExpiryMinutes": 60,
  "Issuer": "ParcialFJCO",
  "Audience": "ParcialFJCOUsers"
}
```

- `SecretKey`: cámbiala por una cadena suficientemente larga.
- `ExpiryMinutes`: tiempo de vida del token en minutos.

### 3. API Key de OpenWeather

Registrar una API key gratuita en OpenWeather y añadirla:

```json
"OpenWeather": {
  "ApiKey": "TU_API_KEY"
}
```

Se usa para enriquecer lecturas con clima actual de **San Salvador, SV** a través de `WeatherService`.

## Inicializar la base de datos

Desde la raíz de la solución, ejecutar:

```bash
dotnet ef database update -p ParcialFJCO.Infraestructure -s ParcialFJCO.API
```

Esto:

- Crea tablas:
  - `Usuarios`
  - `SensoresCalidadAire`
  - `LecturasAire`
  - `AlertasAire`
- Inserta datos iniciales (seeding).

### Datos seed creados

**Usuario base para login**

Tabla `Usuarios`:

- `Username`: `usuarioAdmin`
- `Password`: `12345`
- `Role`: `Admin`
- La contraseña se guarda como hash BCrypt, pero puedes loguearte con `12345`.

**Sensores de prueba**

Tabla `SensoresCalidadAire`:

- Id = 1, Ubicacion = `Planta 1 - Zona A`, TipoGas = `PM2.5/PM10/CO2`, Estado = `Activo`
- Id = 2, Ubicacion = `Planta 1 - Zona B`, TipoGas = `PM2.5/CO2`, Estado = `Activo`

**Lecturas de prueba**

Tabla `LecturasAire`:

- Id = 1, SensorId = 1 → valores sin alerta (lectura normal)
- Id = 2, SensorId = 1 → valores que generan alerta **Moderada**
- Id = 3, SensorId = 2 → valores que generan alerta **Extrema**

**Alertas de prueba**

Tabla `AlertasAire`:

- Id = 1 → alerta Moderada asociada a Lectura 2 / Sensor 1  
- Id = 2 → alerta Extrema asociada a Lectura 3 / Sensor 2  

Estas filas permiten probar `GET /api/alertas` sin registrar lecturas manualmente.

## Ejecutar la API

Desde la raíz:

```bash
dotnet run --project ParcialFJCO.API
```

Por defecto:

- API disponible en `https://localhost:<puerto>`  
- Swagger en `https://localhost:<puerto>/swagger`

## Flujo básico de uso

1. **Login y obtención de JWT**

   - Endpoint: `POST /api/login`
   - Body:

     ```json
     {
       "username": "usuarioAdmin",
       "password": "12345"
     }
     ```

   - Respuesta: objeto con `success = true` y el `token` JWT.

2. **Registrar una lectura**

   - Endpoint protegido: `POST /api/lecturas`
   - Header: `Authorization: Bearer <tu_token>`
   - Body ejemplo:

     ```json
     {
       "sensorId": 1,
       "pm2_5": 80,
       "pm10": 120,
       "co2": 900
     }
     ```

   - Comportamiento:
     - Valida sensor y valores no negativos.
     - Guarda la lectura.
     - Genera alerta automática según umbrales (Leve / Moderada / Crítica / Extrema) usando `AlertaAireFactory`.
     - Respuesta: objeto con `lectura` y `alerta` (o `alerta = null` si no aplica).

3. **Consultar alertas**

   - Endpoint protegido: `GET /api/alertas`
   - Devuelve la lista de alertas (`AlertaAireDto`) ordenadas por fecha descendente.

4. **Consultar lecturas con filtros**

   - Endpoint protegido: `GET /api/lecturas`
   - Parámetros de query opcionales:
     - `fechaDesde` (ej. `2026-05-19`)
     - `fechaHasta`
     - `sensorId`
     - `contaminante` (`PM2_5`, `PM10`, `CO2`) – usado para ordenar.
   - Ejemplos:

     - Todas las lecturas:

       `/api/lecturas`

     - Lecturas del sensor 1 entre dos fechas:

       `/api/lecturas?fechaDesde=2026-05-19&fechaHasta=2026-05-21&sensorId=1`

5. **Consultar lecturas enriquecidas con clima**

   - Endpoint protegido: `GET /api/lecturas/enriquecidas`
   - Mismos parámetros de query que el endpoint anterior.
   - Devuelve `LecturaAireEnriquecidaDto` con:
     - Datos de la lectura.
     - `temperatura`, `humedad`, `descripcionClima` obtenidos de OpenWeather para San Salvador.

## Notas para el docente

- Autenticación implementada con JWT sin ASP.NET Identity: tabla `Usuarios` propia + BCrypt para contraseñas.
- Reglas de negocio de alertas centralizadas en `AlertaAireFactory` (Domain).
- Migraciones EF Core incluidas para crear todo el esquema y los datos de prueba.
- Swagger/OpenAPI habilitado para inspeccionar y probar todos los endpoints desde el navegador.
