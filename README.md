# Product Store

Sistema para gerenciamento de produtos desenvolvido com ASP.NET Core Web API, SQL Server e React + Vite.

---

# Tecnologias utilizadas

## Back-end
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger
- Serilog

## Front-end
- React
- Vite
- Axios
- CSS

---

# Funcionalidades

- Listagem de produtos
- Cadastro de produtos
- Edição de produtos
- Remoção de produtos
- Paginação
- Validação de regras de negócio
- Exibição de mensagens de erro
- Integração com SQL Server

---

# Regras de negócio

- O estoque do produto não pode ser negativo.
- Produtos da categoria `Eletronicos` devem possuir preço mínimo de 50.
- O SKU deve ser único.

---

# Estrutura do projeto

```txt
productStore/
├── backend-productStore/
│   └── ProductStore.Api/
│       ├── Controllers/
│       ├── Data/
│       ├── DTOs/
│       ├── Entities/
│       ├── Middleware/
│       ├── Services/
│       └── Program.cs
│
└── frontend-productstore/
    └── src/
        ├── api/
        ├── components/
        ├── pages/
        ├── App.jsx
        └── App.css
```

---

# Pré-requisitos

Antes de executar o projeto, é necessário possuir instalado:

- .NET SDK
- Node.js
- SQL Server
- SQL Server Management Studio (SSMS) ou Azure Data Studio

---

# Como executar o Back-end

## Acesse a pasta da API

```bash
cd backend-productStore/ProductStore.Api
```

## Restaurar dependências

```bash
dotnet restore
```

## Configurar connection string

No arquivo `appsettings.json`:

### SQL Server padrão

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ProductStoreDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### SQL Express

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ProductStoreDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## Executar migrations

```bash
dotnet ef database update
```

---

## Executar a API

```bash
dotnet run
```

A API ficará disponível em:

```txt
http://localhost:5259
```

Swagger:

```txt
http://localhost:5259/swagger
```

---

# Como executar o Front-end

## Acesse a pasta do front-end

```bash
cd frontend-productstore
```

## Instalar dependências

```bash
npm install
```

## Executar aplicação

```bash
npm run dev
```

O front-end ficará disponível em:

```txt
http://localhost:5173
```

---

# Configuração da API no Front-end

Arquivo:

```txt
frontend-productstore/src/api/productsApi.js
```

Configuração:

```js
import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5259/api",
});

export default api;
```

---

# Endpoints principais

```txt
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

---

# Observações

- Certifique-se de que o back-end esteja em execução antes de iniciar o front-end.
- O banco de dados será criado automaticamente ao executar as migrations.
- O projeto utiliza Entity Framework Core com SQL Server.
- O Swagger pode ser utilizado para testar os endpoints da API.
