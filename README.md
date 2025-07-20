~~~~# AI-Powered Laptop Support System

This monorepo contains the complete source code for the AI-Powered Laptop Support System, a multi-technology project designed to provide automated technical assistance.

## Overview

The system is composed of three main applications:
- **Angular Frontend (`apps/angular-frontend`):** A real-time chat interface for user interaction.
- **Django Backend (`apps/django-backend`):** An API layer that handles user requests, manages WebSocket connections, and communicates with the gRPC service.
- **.NET gRPC Service (`apps/dotnet-grpc-service`):** A high-performance C# service that interacts directly with the operating system to perform tasks like installing applications and querying hardware.

## Tech Stack

- **Frontend:** Angular 20, TypeScript, RxJS, WebSockets
- **Backend:** Django 4.2+, Django REST Framework, Django Channels
- **AI Model:** TensorFlow, HuggingFace Transformers (`bert-base-multilingual-cased`)
- **gRPC Service:** C# on .NET 8, ASP.NET Core gRPC
- **Database:** PostgreSQL
- **Containerization:** Docker

## Getting Started

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    ```
2.  **Navigate to the project root:**
    ```bash
    cd <project-directory>
    ```
3.  **Set up environment variables:**
    - Copy the `config/.env.example` file to `config/.env`.
    - Fill in the required environment variables.
4.  **Run the development environment:**
    ```bash
    docker-compose up --build
    ```

## Project Structure

- **`apps/`**: Contains the primary, deployable applications (Angular, Django, .NET).
- **`libs/`**: Houses shared libraries, code, or type definitions used across multiple applications.
- **`config/`**: Stores all configuration files, including environment variables and the JSON schemas for the AI.
- **`tools/`**: Includes development and operational scripts (e.g., deployment scripts, database migration tools).
