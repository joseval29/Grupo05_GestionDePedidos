# Sistema de Gestión de Pedidos — Grupo 5 (Arquitectura Monolítica)

**Integrantes:** Jose Valeriano, Camila Beltrán, Jorge Fortich
**Stack:** ASP.NET Core MVC / C# / MS-SQL Server — Septiembre 2026

## Contenido de este paquete

```
├── Documento_Tecnico_Grupo5_Monolito.pdf   ← Documento técnico completo (28 páginas)
├── Documento_Tecnico_Grupo5_Monolito.md    ← El mismo documento en Markdown editable
├── diagramas/                              ← Las 8 imágenes de los diagramas C4 usadas en el documento
└── Codigo_Fuente/
    └── SistemaGestionPedidos/              ← Proyecto ASP.NET Core MVC completo (caso práctico)
        ├── Controllers/, Services/, Data/, Models/, Views/, Exceptions/
        ├── Dockerfile, docker-compose.yml
        └── README.md                       ← Instrucciones de despliegue del proyecto
```

## Documento Técnico — contenido

1. Investigación del Estilo Arquitectónico: Monolítico
2. Investigación del Stack Tecnológico (ASP.NET Core MVC, C#, SQL Server, Docker, Protocolo de Integración)
3. Análisis Arquitectónico — Matriz de Calidad (ISO/IEC 25010:2023, 9 características), Matriz de Principios, Matriz de Tácticas por Atributo, Matriz de Tácticas/ADR, Matriz de Mercado Laboral con proyección
4. Diseño — Modelo C4 (HLD, Contexto, Contenedores, Dinámico, Despliegue, Modelo de Datos, Componentes)
5. Implementación
6. Lecciones Aprendidas
7. Conclusiones
8. Referencias

## Caso Práctico (Código)

Proyecto ASP.NET Core MVC funcional con Docker Compose (app + SQL Server 2022). Ver el `README.md`
dentro de `Codigo_Fuente/SistemaGestionPedidos/` para los pasos exactos de despliegue
(`docker-compose up --build`).


