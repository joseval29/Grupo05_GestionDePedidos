# Sistema de Gestión de Pedidos — Grupo 5 (Estilo Monolítico)

## Descripción del sistema

Aplicación web para la gestión de **Clientes**, **Productos** y **Pedidos**, implementada con
**arquitectura monolítica**: toda la lógica de presentación (Vistas MVC), negocio (Services)
y acceso a datos (EF Core) vive dentro de un único proceso/despliegue, sin separación en
servicios independientes ni comunicación por red entre capas internas.

### Caso de uso principal (end-to-end)

1. El usuario selecciona un **Cliente** y agrega una o más líneas de **Producto** con cantidad.
2. El sistema valida que el cliente exista y que cada producto tenga **stock suficiente**.
3. Se crea el **Pedido** junto con sus líneas (`DetallePedido`), se descuenta el stock y se
   calcula el total — todo dentro de **una sola transacción de base de datos**.
4. Si cualquier validación falla (stock insuficiente, producto/cliente inexistente), se revierte
   toda la operación (rollback) y se muestra el error al usuario.

## Arquitectura del stack (Grupo 5)

| Capa | Tecnología |
|---|---|
| Frontend + Backend | ASP.NET Core MVC (C#) — monolito, sin API separada |
| Base de datos | Microsoft SQL Server |
| Integración interna | Llamado directo a función (inyección de dependencias .NET) |
| ORM | Entity Framework Core 8 |
| Contenedores | Docker / Docker Compose |

### Entidades de negocio

- **Cliente** (1) → (N) **Pedido**
- **Pedido** (1) → (N) **DetallePedido**
- **Producto** (1) → (N) **DetallePedido**

### Estructura de carpetas (capas dentro del monolito)

```
/Controllers    -> Reciben las peticiones HTTP y orquestan los Services
/Services       -> Lógica de negocio (validaciones, transacciones)
/Data           -> DbContext y seed de datos de ejemplo
/Models         -> Entidades de dominio y ViewModels
/Exceptions     -> Excepciones de negocio personalizadas
/Views          -> Razor Views (UI)
```

## Tecnologías usadas

- .NET 8 / ASP.NET Core MVC
- C#
- Entity Framework Core 8 (SQL Server provider)
- Microsoft SQL Server 2022
- Bootstrap 5 (CDN)
- Docker / Docker Compose

## Pasos para despliegue (Docker)

### Requisitos previos
- Docker y Docker Compose instalados.

La migración inicial de EF Core ya viene incluida en la carpeta `/Migrations`, por lo que no
es necesario tener el SDK de .NET instalado localmente. Si en el futuro se modifican las
entidades, se debe generar una nueva migración con `dotnet ef migrations add <Nombre>`.

### 1. Levantar la aplicación con Docker Compose

```bash
docker-compose up --build
```

Esto levanta dos contenedores:
- **db**: SQL Server 2022 (puerto `1433`)
- **app**: la aplicación ASP.NET MVC (puerto `8080`)

Al iniciar, la app aplica automáticamente las migraciones pendientes (`db.Database.Migrate()`)
y carga datos de ejemplo (2 clientes, 3 productos) mediante `DbSeeder`.

### 2. Acceder a la aplicación

Abrir en el navegador:

```
http://localhost:8080
```

### 3. Detener los contenedores

```bash
docker-compose down
```

Para borrar también los datos persistidos en el volumen de SQL Server:

```bash
docker-compose down -v
```

## Notas de diseño / decisiones arquitectónicas

- **Transacciones explícitas** en `PedidoService.CrearPedidoAsync`: se aprovecha que todo el
  sistema comparte una única base de datos y proceso, lo cual simplifica garantizar
  atomicidad (ventaja típica de un monolito frente a arquitecturas distribuidas).
- **Manejo de errores por excepciones de dominio** (`StockInsuficienteException`,
  `EntidadNoEncontradaException`, `PedidoInvalidoException`) capturadas en los Controllers
  para mostrar mensajes claros al usuario sin exponer detalles internos.
- **Separación en capas internas** (Controllers → Services → Data) para mantener buenas
  prácticas de diseño (SRP) aun dentro de un despliegue monolítico único.
