🏦 PayPhoneApiChallenge
PayPhoneApiChallenge es una API REST desarrollada en .NET 8 que permite realizar transferencias de saldo entre billeteras virtuales. Implementa una arquitectura limpia (Clean Architecture), principios SOLID, pruebas unitarias e integración, y utiliza Entity Framework Core con SQLite como base de datos.

🚀 Características Principales
CRUD completo para Wallets (Billeteras).
CR para Transactions (Transacciones entre billeteras).
Transferencias en USD ($).
Soporte completo de manejo de errores.
Arquitectura escalable y mantenible (Clean Architecture).
Pruebas unitarias e integración.
Uso de ORM: Entity Framework Core.
Base de datos liviana: SQLite.
📂 Estructura del Proyecto (Clean Architecture)


PayPhoneApiChallenge/
├── PayPhoneApiChallenge/         → Capa de presentación (API REST)
├── PayPhoneApiChallenge.App/     → Capa de aplicación (Lógica de negocio)
├── PayPhoneApiChallenge.Domain/  → Capa de dominio (Entidades, interfaces)
├── PayPhoneApiChallenge.Infra/   → Capa de infraestructura (EF Core, Repositorios)
└── PayPhoneApiChallenge.Tests/   → Pruebas unitarias e integración


⚙️ Tecnologías Usadas
.NET 8
Entity Framework Core 8
SQLite (Base de datos liviana y portable)
Swagger/OpenAPI (Para documentación interactiva)
XUnit / Moq (Pruebas)
Principios SOLID aplicados

🛠️ Configuración y Ejecución
1. Clonar el repositorio:
bash
Copiar
Editar
git clone https://github.com/tuusuario/PayPhoneApiChallenge.git
cd PayPhoneApiChallenge
2. Restaurar dependencias:
bash
Copiar
Editar
dotnet restore
3. Compilar el proyecto:
bash
Copiar
Editar
dotnet build
4. Crear la base de datos:
bash
Copiar
Editar
dotnet ef database update --project PayPhoneApiChallenge.Infra --startup-project PayPhoneApiChallenge
5. Ejecutar la API:
bash
Copiar
Editar
dotnet run --project PayPhoneApiChallenge
API disponible en: https://localhost:5001

📑 Endpoints Principales
Método	Endpoint	Descripción
GET	/api/wallets	Lista todas las billeteras
POST	/api/wallets	Crea una nueva billetera
PUT	/api/wallets/{id}	Actualiza una billetera
DELETE	/api/wallets/{id}	Elimina una billetera
POST	/api/transactions	Realiza una transferencia
GET	/api/transactions	Lista todas las transacciones
🧪 Pruebas
Para ejecutar las pruebas:

bash
Copiar
Editar
dotnet test
📌 Notas
Las transacciones son entre 2 billeteras.
No se permite transferir si el saldo es insuficiente.
Manejo de fechas (CreatedAt, UpdatedAt) automático en el contexto.
Tipos de transacción (Type) definidos mediante un enum: Credit / Debit.
📄 Licencia
MIT License © 2025 — TuNombreDev
