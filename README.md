# 🏦 PayPhoneApiChallenge

**PayPhoneApiChallenge** es una API REST desarrollada en **.NET 8** que permite realizar **transferencias de saldo entre
billeteras virtuales**. 

Se pidio desarrollar bajo una arquitectura limpia (**Clean Architecture**), principios **SOLID**, pruebas unitarias e integración, y utiliza 
**Entity Framework Core** para la base de datos.

---

## 🚀 Características Principales

- ✅ CRUD completo para **Wallets** (Billeteras).
- ✅ CR para **Transactions** (Transacciones entre billeteras).
- ✅ Transferencias en **USD (\$)**.
- ✅ Manejo de errores personalizado mediante middleware.
- ✅ Arquitectura escalable y mantenible (**Clean Architecture**).
- ✅ Pruebas unitarias e integración. (**Por terminar**)
- ✅ ORM: Entity Framework Core 8. (**Implementado**)
- ✅ Base de datos liviana: **SQLite**.
- ✅ Documentación interactiva con **Swagger**. (**Esta de mejorar**)

---

## 📂 Estructura del Proyecto

```
PayPhoneApiChallenge/
├── PayPhoneApiChallenge/         → Capa de presentación (API REST)
├── PayPhoneApiChallenge.App/     → Capa de aplicación (Lógica de negocio)
├── PayPhoneApiChallenge.Domain/  → Capa de dominio (Entidades, interfaces)
├── PayPhoneApiChallenge.Infra/   → Capa de infraestructura (Entity Framework Core, Repositorios)
└── PayPhoneApiChallenge.Tests/   → Pruebas unitarias e integración
```

---

## ⚙️ Tecnologías Usadas

- **.NET 8**
- **Entity Framework Core 8**
- **SQLite** (Base de datos liviana y portable)
- **Swagger/OpenAPI** (Documentación interactiva)
- **XUnit / Moq** (Pruebas unitarias e integración)
- **SOLID** (Principios de diseño aplicados)

---

## 🛠️ Configuración y Ejecución

1. Clonar el repositorio:

```bash
git clone https://github.com/[tuusuario]/PayPhoneApiChallenge.git
cd PayPhoneApiChallenge
```

2. Restaurar dependencias:

```bash
dotnet restore
```

3. Compilar el proyecto:

```bash
dotnet build
```

4. Crear la base de datos:

```bash
dotnet ef database update --project PayPhoneApiChallenge.Infra --startup-project PayPhoneApiChallenge
```

5. Ejecutar la API:

```bash
dotnet run --project PayPhoneApiChallenge
```

La API estará disponible en: [https://localhost:5194](https://localhost:5194)

---

## 📑 Endpoints Principales

| Método | Endpoint             | Descripción                            |
| ------ | -------------------- | -------------------------------------- |
| POST   | /api/auth/login      | Obtiene un token JWT (autenticación)   |
| GET    | /api/wallets         | Lista todas las billeteras             |
| POST   | /api/wallets         | Crea una nueva billetera               |
| PUT    | /api/wallets/{id}    | Actualiza una billetera existente      |
| DELETE | /api/wallets/{id}    | Elimina una billetera                  |
| GET    | /api/transactions    | Lista todas las transacciones          |
| POST   | /api/transactions    | Realiza una transferencia              |

> 🔒 **Nota**: Todos los endpoints requieren autenticación Bearer Token, excepto `/api/auth/login` y `/api/transactions` historial de transaaciones.

---

## 👤 Usuario de Prueba (Swagger)

Swagger ya incluye un usuario de prueba precargado para facilitar las pruebas:

- **Usuario:** admin
- **Contraseña:** admin

1. Inicia sesión en `/api/auth/login` con las credenciales anteriores.
2. Copia el token JWT Bearer que se genera .
3. Haz clic en el botón "Authorize" en Swagger e ingresa el token como:

```
Bearer {tu_token}
```

Swagger disponible en: [https://localhost:5194/swagger](https://localhost:5194/swagger)

---

## 🧪 Pruebas

Para ejecutar las pruebas unitarias e integración:

```bash
dotnet test
```

---

## 📌 Notas

- Las transferencias son entre 2 billeteras existentes.
- No se permite transferir si el saldo es insuficiente.
- Manejo de fechas automático (**CreatedAt**, **UpdatedAt**) mediante EF Core.
- Tipos de transacción definidos por enum: `Credit` / `Debit`.
- Middleware personalizado maneja errores comunes como `404 Not Found`, `400 Bad Request` y errores de validación.

---

## 📄 Licencia

MIT License © 2025 - Walter Cun Bustamante

