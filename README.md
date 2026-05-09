
# Sample Microservices with RabbitMQ

A simple ASP.NET Core (.NET 9) microservices demo application using RabbitMQ for asynchronous communication between services.

This project demonstrates how OrderService communicates with ProductService using RabbitMQ message broker and BackgroundService consumers.

---

# Technologies Used

* ASP.NET Core .NET 9
* RabbitMQ
* BackgroundService
* Minimal APIs
* Visual Studio 2022

---

# Project Structure

```bash
OrderService
ProductService
```

---

# How It Works

```text
Client
   |
   v
OrderService
   |
   v
RabbitMQ Queue
   |
   v
ProductService
```

* OrderService publishes messages to RabbitMQ
* RabbitMQ stores and forwards messages
* ProductService continuously listens to the queue
* ProductService receives the order message and updates stock

---

# Install RabbitMQ (Local PC)

For this project, install RabbitMQ directly on your local machine.

Download RabbitMQ:

* https://www.rabbitmq.com/download.html

Install Erlang first:

* https://www.erlang.org/downloads

After installation, RabbitMQ runs locally on:

```text
localhost:5672
```

RabbitMQ Management Dashboard:

```text
http://localhost:15672
```

Default Login:

```text
Username: guest
Password: guest
```

---

# Clone Repository

```bash
git clone https://github.com/muizali25/rabbitmq-microservices-sample.git
```

---

# Open in Visual Studio 2022

Open solution file in Visual Studio 2022.

Since both services are inside one solution, Visual Studio can start both services together.

---

# Run Both Services from Visual Studio

* Right click Solution
* Configure Startup Projects
* Select:

  * OrderService
  * ProductService
* Choose:

  * Multiple Startup Projects
* Click Run

Both services will start together.

---

# Run Services Separately Using Terminal

## Run OrderService

```bash
cd OrderService
dotnet run
```

---

## Run ProductService

```bash
cd ProductService
dotnet run
```

---

# Test Using Postman

## POST Request

```http
POST /orders
```

Example URL:

```text
[https://localhost:7001/orders](http://localhost:5094/orders)
```

---

# Request Body

```json
{
  "productId": 1,
  "quantity": 2
}
```

---

# Expected Flow

1. Client sends order request to OrderService
2. OrderService publishes message to RabbitMQ
3. RabbitMQ forwards message to ProductService
4. ProductService receives message from queue
5. ProductService updates stock
6. Updated stock is printed in console

---

# Example Console Output

```text
Message Received:
{
  "productId":1,
  "quantity":2
}

Stock Updated: 8
```

---

# Important Concepts

## Publisher

OrderService acts as Publisher.

It sends messages to RabbitMQ.

---

## Consumer

ProductService acts as Consumer.

It continuously listens to RabbitMQ queue using BackgroundService.

---

## Queue

RabbitMQ queue stores messages temporarily until Consumer receives them.

---

## BackgroundService

BackgroundService runs continuously in the background and keeps listening for new messages from RabbitMQ.

---

# Why Use RabbitMQ?

RabbitMQ helps services communicate asynchronously.

Instead of direct API calls:

```text
OrderService ---> ProductService
```

we use:

```text
OrderService ---> RabbitMQ ---> ProductService
```

This creates loose coupling between services.

Benefits:

* Better scalability
* Independent services
* Queue-based communication
* Reliable message delivery
* Better microservices architecture

---

# Learning Goals

This project helps understand:

* Microservices basics
* RabbitMQ basics
* Producer / Consumer pattern
* BackgroundService
* Queue messaging
* Asynchronous communication
* Event-driven architecture

---
