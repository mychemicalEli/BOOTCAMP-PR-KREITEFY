# 🎵 Kreitekfy

**Kreitekfy** es una aplicación web inspirada en Spotify que simula una experiencia de usuario personalizada para escuchar y explorar música. Incluye autenticación, reproducción simulada, historial de escuchas y un diseño similar a la interfaz de Spotify.

## ✨ Funcionalidades principales

- 🔐 Autenticación con JWT
- 🏠 Panel de usuario con:
  - Canciones más reproducidas
  - Novedades
  - Sección "Para ti"
- 🎧 Exploración de canciones:
  - Filtros por título, artista, género, álbum...
  - Detalle de cada canción
  - Simulación de reproducción (aumenta el contador de reproducciones)
  - Sistema de puntuación con estrellas
- 🧠 Consultas con LINQ en el backend
- 👤 Perfil del usuario:
  - Visualización y edición de datos personales
  - Historial de canciones reproducidas
- 💻 Interfaz basada en el diseño de Spotify (logo y landing page diseñadas con Figma)
- 🚀 Plan futuro: panel de administración para gestionar canciones, artistas, álbumes...

---

## 🛠 Tecnologías utilizadas

| Parte       | Tecnologías                         |
|------------|--------------------------------------|
| **Frontend** | Angular, Bootstrap                  |
| **Backend**  | .NET (ASP.NET Core Web API)         |
| **Autenticación** | JWT                           |
| **Consultas** | LINQ                              |
| **Diseño UI** | Figma, Bootstrap                  |
| **Documentación API** | Swagger UI                |

---

## 🚀 Instalación y ejecución local

### 1. 📦 Clonar el repositorio

```bash
git clone https://github.com/mychemicalEli/BOOTCAMP-PR-KREITEFY.git
cd BOOTCAMP-PR-KREITEFY
````

### 2. 🔧 Backend (.NET)

```bash
cd api/api
dotnet restore
dotnet run
````

Accede a la documentación Swagger:
👉 http://localhost:5282/swagger/

### 3. 💻 Frontend (Angular + Bootstrap)
```bash
cd ../../site
npm install
ng serve
````
Abre la aplicación en tu navegador:
👉 http://localhost:4200/

## 🔮 Futuras mejoras

- 🛠 Implementación de un **panel de administración (backoffice)** para usuarios con rol de administrador.
- ➕ Funcionalidad para **añadir, editar y eliminar**:
  - Canciones
  - Álbumes
  - Artistas
  - Géneros musicales
- 👥 Gestión de usuarios:
  - Asignar y revocar roles de administrador


