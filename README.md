# LuxenDesktop

## Descripción
LuxenDesktop es una aplicación WPF diseñada para gestionar migraciones de bases de datos. Este proyecto está dividido en múltiples capas, incluyendo acceso a datos, lógica de negocio y la interfaz de usuario.

## Configuración
Para ejecutar este proyecto localmente, necesitarás:
- Visual Studio 2019 o superior.
- .NET Framework 4.7.2 o superior.
- Acceso a una instancia de MongoDB (ajustar la cadena de conexión en `App.config` que debe crearse en el proyecto del View).

## Estructura del proyecto
- `DBMigratePro/` - Lógica de negocio.
- `DBMigratePro.DataAccess/` - Capa de acceso a datos (aun en implementación).
- `DBMigratePro.Entities/` - Definiciones de entidades.
- `DBMigratePro.View/` - Interfaz de usuario de WPF.

## Cómo contribuir
Si deseas contribuir a este proyecto, considera seguir estas pautas:
1. Haz un fork del repositorio.
2. Crea una nueva rama para cada característica o mejora.
3. Envía un pull request desde tu fork.
