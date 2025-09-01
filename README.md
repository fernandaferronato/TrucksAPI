# Gerenciamento de Caminhões 

## ✅ Pré-requisitos

Antes de rodar, certifique-se de ter instalado:

[SDK .NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

[Node.js LTS (18.x ou 20.x)](https://nodejs.org/en)

[Angular CLI](https://v17.angular.io/cli)

1. Em qualquer diretório, instalar o Angular CLI:
```bash
npm install -g @angular/cli
```

2. Clonar o repositório

```bash
git clone https://github.com/fernandaferronato/TrucksAPI
```

## 🔹 Como rodar a API (Backend)

1. No repositório clonado, restaurar os pacotes
```bash
cd TrucksAPI\src\TrucksAPI.Api
dotnet restore
```

2. Rodar a API
```bash
dotnet run
```

3. A API estará disponível no Swagger:
```bash
http://localhost:5249/swagger/
```

## 🔹 Como rodar o Frontend (Angular)

1. No repositório clonado, acessar o diretório do frontend
```bash
cd TrucksAPI\frontend\angular-trucks
```

2. Instalar dependências
```bash
npm install
```

3. Rodar o servidor Angular
```bash
ng serve
```

4. A aplicação ficará disponível em:
```bash
http://localhost:4200
```

## 🧪 Rodando os Testes

Para rodar os testes:

1. Abra o terminal na raiz do projeto clonado (pasta TrucksAPI, onde está o .sln).

2. Execute o comando:

```bash
dotnet test
```
