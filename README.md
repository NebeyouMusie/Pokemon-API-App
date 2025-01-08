# Pokemon API

A simple Pokemon API built using ASP.NET Core with MongoDB Atlas integration. This API allows users to perform CRUD operations on a Pokemon collection.

---

## Prerequisites

- .NET 6 SDK installed
- MongoDB Atlas account and cluster setup
- A tool like Postman or Thunder Client for API testing

---

## Setup Instructions

### 1. Clone the Repository

```bash
# Clone the repository
git clone <repository-url>

# Navigate to the project folder
cd PokemonApi
```

### 2. Create a `.env` File

Create a `.env` file in the root directory with the following content:

```env
MONGODB_CONNECTION_STRING=mongodb+srv://<username>:<password>@<cluster-address>/?retryWrites=true&w=majority
```

- Replace `<username>` and `<password>` with your MongoDB Atlas credentials.
- Replace `<cluster-address>` with your MongoDB cluster address.

### 3. Update `appsettings.json`

Ensure the `appsettings.json` includes:

```json
{
  "DatabaseSettings": {
    "DatabaseName": "PokemonApi",
    "PokemonCollectionName": "Pokemon"
  }
}
```

### 4. Restore and Build

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build
```

### 5. Run the Application

```bash
# Run the app
dotnet run
```

The app will start and be available at `http://localhost:5000` (or another port as specified).

---

## API Endpoints and Usage

### Base URL

```
http://localhost:5048/api/Pokemon
```

### Example Usages

#### 1. Add a New Pokemon

Start by creating the following Pokemon, as the database is empty when the app initializes:

- **Endpoint**: `POST /api/Pokemon`
- **Headers**: `Content-Type: application/json`
- **Body**:

```json
{
  "id": 1,
  "name": "Pikachu",
  "type": "Electric",
  "ability": "Static",
  "level": 5
}
```

- **Response**:

```json
{
  "id": 1,
  "name": "Pikachu",
  "type": "Electric",
  "ability": "Static",
  "level": 5
}
```

#### 2. Add Another Pokemon

- **Endpoint**: `POST /api/Pokemon`
- **Headers**: `Content-Type: application/json`
- **Body**:

```json
{
  "id": 2,
  "name": "Charmander",
  "type": "Fire",
  "ability": "Blaze",
  "level": 10
}
```

- **Response**:

```json
{
  "id": 2,
  "name": "Charmander",
  "type": "Fire",
  "ability": "Blaze",
  "level": 10
}
```

#### 3. Get All Pokemon

- **Endpoint**: `GET /api/Pokemon`
- **Response**:

```json
[
  {
    "id": 1,
    "name": "Pikachu",
    "type": "Electric",
    "ability": "Static",
    "level": 5
  },
  {
    "id": 2,
    "name": "Charmander",
    "type": "Fire",
    "ability": "Blaze",
    "level": 10
  }
]
```

#### 4. Get Pokemon by ID

- **Endpoint**: `GET /api/Pokemon/{id}`
- **Example**: `GET /api/Pokemon/1`
- **Response**:

```json
{
  "id": 1,
  "name": "Pikachu",
  "type": "Electric",
  "ability": "Static",
  "level": 5
}
```

#### 5. Update an Existing Pokemon

- **Endpoint**: `PUT /api/Pokemon`
- **Headers**: `Content-Type: application/json`
- **Body**:

```json
{
  "id": 2,
  "name": "Charmander",
  "type": "Fire",
  "ability": "Solar Power",
  "level": 12
}
```

- **Response**: `204 No Content`

#### 6. Delete a Pokemon

- **Endpoint**: `DELETE /api/Pokemon/{id}`
- **Example**: `DELETE /api/Pokemon/2`
- **Response**: `204 No Content`

---

## Notes

- Ensure the MongoDB Atlas cluster is set up and accessible.
- The `DatabaseName` and `PokemonCollectionName` in `appsettings.json` should match your MongoDB configuration.
- Use the `.env` file to keep your MongoDB credentials secure.

---

## Troubleshooting

### Common Issues

1. **`MongoDB Connection Failed`**:

   - Verify the connection string in the `.env` file.
   - Ensure IP Whitelist and credentials in MongoDB Atlas are correctly configured.

2. **Port Already in Use**:

   - Use a different port by modifying the launch settings or passing a parameter to `dotnet run`.

---

## License

This project is licensed under the MIT License. See the LICENSE file for details.

