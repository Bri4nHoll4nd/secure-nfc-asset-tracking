# Secure NFC Asset Tracking

A secure equipment and asset tracking system built around NFC identification, an ESP32-based reader, and a central ASP.NET Core backend.

The project is intended to explore how embedded hardware, backend services, cryptographic tag verification, and asset-management workflows can be combined into a complete tracking system.

> [!IMPORTANT]
> This project is currently under active development. The repository contains an early working prototype, and several planned components are not yet implemented.

## Project Overview

The system uses NFC tags to identify assets and operators. An ESP32-S3 connected to a PN532 NFC module reads or writes tag data and communicates with a central API over Wi-Fi.

Each programmed tag can contain:

* A unique entity ID
* A data-format version
* A 16-byte cryptographic signature
* The hardware UID provided by the NFC tag

The signature is intended to allow the backend to detect modified or incorrectly programmed tags instead of trusting the stored values by themselves.

The finished system is planned to support equipment checkout, returns, maintenance tracking, access control, and a complete audit history.

## Current Status

The current prototype supports:

* Detecting NFC tags with a PN532 reader
* Reading the UID and UID length of a tag
* Authenticating MIFARE Classic data blocks
* Writing an entity ID to an NFC tag
* Writing a tag-format version to an NFC tag
* Writing a 16-byte signature to an NFC tag
* Connecting the ESP32-S3 to Wi-Fi
* Sending tag UIDs from the ESP32 to the backend through HTTP
* Receiving basic NFC scan requests in the ASP.NET Core API
* Returning test asset data from the API
* Simulating NFC scans through a .NET console application
* Generating a truncated HMAC-SHA256 signature in the simulator for testing

The most recently completed milestone is writing structured test data and a 16-byte signature to a physical NFC tag.

## Planned Features

The project is expected to include:

* PostgreSQL database integration
* Entity Framework Core
* Asset and operator registration
* Secure tag provisioning
* Backend-generated HMAC signatures
* Verification of scanned tag signatures
* Equipment checkout and return workflows
* Asset states such as:

  * Available
  * Checked out
  * Under maintenance
  * Retired
* Operator roles and access control
* Immutable audit logging
* Maintenance history
* Live frontend updates
* A browser-based management dashboard
* Offline handling and synchronization
* Unit and integration tests
* Docker-based local development
* Architecture and system documentation

## System Architecture

```text
┌─────────────────────┐
│      NFC Tag        │
│                     │
│ UID                 │
│ Entity ID           │
│ Format Version      │
│ Signature           │
└──────────┬──────────┘
           │ NFC
           ▼
┌─────────────────────┐
│ ESP32-S3 + PN532    │
│                     │
│ Reads/writes tags   │
│ Connects over Wi-Fi │
└──────────┬──────────┘
           │ HTTP / JSON
           ▼
┌─────────────────────┐
│ ASP.NET Core API    │
│                     │
│ Validation          │
│ Asset workflows     │
│ Audit logging       │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ PostgreSQL Database │
│     Planned         │
└─────────────────────┘
```

The virtual NFC reader provides an alternative input source for backend development:

```text
Virtual NFC Reader ────── HTTP / JSON ──────► ASP.NET Core API
```

This makes it possible to test API workflows without requiring the physical ESP32 and NFC reader for every development task.

## Repository Structure

```text
secure-nfc-asset-tracking/
├── backend/
│   └── SecureNfc.Api/
│       ├── Controllers/
│       ├── DTOs/
│       ├── Models/
│       └── Program.cs
│
├── firmware/
│   ├── include/
│   │   ├── ApiManager.h
│   │   ├── AppConfig.h
│   │   ├── NfcReader.h
│   │   ├── TagData.h
│   │   └── WiFiManager.h
│   │
│   ├── src/
│   │   ├── ApiManager.cpp
│   │   ├── NfcReader.cpp
│   │   ├── WiFiManager.cpp
│   │   └── main.cpp
│   │
│   └── platformio.ini
│
├── simulator/
│   └── VirtualNfcReader/
│       └── Program.cs
│
├── LICENSE
└── README.md
```

## Components

### ESP32 Firmware

The firmware is written in C++ using the Arduino framework and PlatformIO.

Its responsibilities currently include:

* Connecting to Wi-Fi
* Initializing the PN532 over I²C
* Detecting NFC tags
* Reading tag UIDs
* Authenticating MIFARE Classic blocks
* Writing structured data to tags
* Sending data to the backend API

The firmware is separated into dedicated classes:

* `NfcReader` handles PN532 and NFC operations.
* `WiFiManager` handles network connectivity.
* `ApiManager` handles communication with the backend.
* `TagData` contains the data structures shared by NFC operations.

### Backend API

The backend is an ASP.NET Core Web API written in C#.

The current API contains early endpoints for:

* Receiving complete tag scan data
* Receiving UID-only test scans
* Returning sample asset information
* Viewing and testing endpoints through Swagger

The API currently uses in-memory test values. Persistent database storage and the complete domain model will be added later.

### Virtual NFC Reader

The virtual reader is a .NET console application that simulates data normally sent by the physical ESP32 device.

It is intended to:

* Speed up backend development
* Test scan requests without hardware
* Simulate operator and asset tags
* Test signature generation and verification
* Provide a software-only demonstration mode

## NFC Tag Layout

The prototype uses separate 16-byte MIFARE Classic blocks for each value.

| Value          | Purpose                                                      |
| -------------- | ------------------------------------------------------------ |
| Entity ID      | Identifies the asset or operator                             |
| Format version | Identifies the stored-data format                            |
| Signature      | Verifies the integrity and authenticity of the stored values |

String values are currently limited to 15 characters so the remaining byte can be left as a null terminator.

The signature occupies one complete 16-byte block.

> [!WARNING]
> MIFARE Classic security by itself should not be treated as strong protection. The project therefore plans to validate tag data using a backend-controlled secret and a cryptographic signature.

## Signature Design

The planned signature is based on HMAC-SHA256.

Conceptually, the backend signs a deterministic combination of:

```text
Tag UID + Entity ID + Format Version
```

The generated SHA-256 HMAC is truncated to 16 bytes before being written to the NFC tag.

During a scan, the backend can recreate the expected signature and compare it with the signature read from the tag.

The secret HMAC key must remain on the backend. It should not be stored in the ESP32 firmware, frontend, simulator, source control, or NFC tag in a production implementation.

The values and keys currently found in the prototype are development-only test data.

## Hardware

The physical prototype uses:

* ESP32-S3 development board
* PN532 NFC/RFID reader
* MIFARE Classic-compatible NFC tags
* I²C communication between the ESP32 and PN532
* Wi-Fi communication between the ESP32 and API

## Technology Stack

### Firmware

* C++17
* Arduino framework
* PlatformIO
* ESP32-S3
* Adafruit PN532
* ArduinoJson
* HTTP
* Wi-Fi
* I²C

### Backend

* C#
* .NET
* ASP.NET Core Web API
* Swagger / OpenAPI

### Simulator

* C#
* .NET console application
* `HttpClient`
* HMAC-SHA256

### Planned Infrastructure

* PostgreSQL
* Entity Framework Core
* Docker
* SignalR
* GitHub Actions

## Running the Backend

### Requirements

* A supported .NET SDK
* Visual Studio, Visual Studio Code, Rider, or the .NET CLI

From the repository root:

```bash
cd backend/SecureNfc.Api
dotnet restore
dotnet run
```

The terminal will display the local API address after startup.

Swagger is enabled in the development environment. Open the Swagger URL shown by the application to inspect and test the available endpoints.

## Running the Virtual NFC Reader

Start the backend first.

In another terminal:

```bash
cd simulator/VirtualNfcReader
dotnet restore
dotnet run
```

The simulator currently represents an early test interface and may require its API address or routes to be updated as the backend evolves.

## Building the Firmware

### Requirements

* Visual Studio Code
* PlatformIO extension
* ESP32-S3 development board
* PN532 NFC module
* USB data cable

Open the `firmware` directory as a PlatformIO project.

Build the firmware:

```bash
pio run
```

Upload it to the connected ESP32:

```bash
pio run --target upload
```

Open the serial monitor:

```bash
pio device monitor
```

The serial monitor is configured to use:

```text
115200 baud
```

## Firmware Configuration

Development settings are currently stored in `AppConfig.h`, including values such as:

* Wi-Fi SSID
* Wi-Fi password
* API base address
* PN532 pin configuration
* NFC block numbers
* MIFARE authentication keys

Before publishing or deploying the project, secrets should be moved out of tracked source files.

Possible approaches include:

* A local ignored configuration header
* PlatformIO build flags
* Environment-specific configuration
* Provisioned device credentials
* A secure secrets-management solution

Never commit real Wi-Fi credentials, API secrets, production NFC keys, or HMAC keys.

## Development Roadmap

### Phase 1 — Hardware Prototype

* [x] Initialize the PN532 reader
* [x] Detect NFC tags
* [x] Read tag UIDs
* [x] Connect the ESP32 to Wi-Fi
* [x] Send a test UID to the API
* [x] Authenticate MIFARE Classic blocks
* [x] Write an entity ID
* [x] Write a format version
* [x] Write a 16-byte signature
* [ ] Read all structured data back from the tag
* [ ] Replace hard-coded test data with backend-provided data

### Phase 2 — Backend and Database

* [x] Create the initial ASP.NET Core API
* [x] Add test scan endpoints
* [x] Create an early virtual NFC reader
* [ ] Add PostgreSQL
* [ ] Add Entity Framework Core
* [ ] Define asset, operator, tag, and audit entities
* [ ] Add tag-provisioning endpoints
* [ ] Generate signatures on the backend
* [ ] Verify signatures during scans

### Phase 3 — Asset Workflows

* [ ] Register assets
* [ ] Register operators
* [ ] Check out equipment
* [ ] Return equipment
* [ ] Record maintenance
* [ ] Retire assets
* [ ] Add role-based permissions
* [ ] Add immutable audit history

### Phase 4 — User Interface and Reliability

* [ ] Build a management dashboard
* [ ] Add live scan feedback
* [ ] Add SignalR updates
* [ ] Add offline-device handling
* [ ] Add automated tests
* [ ] Add Docker development setup
* [ ] Add continuous integration
* [ ] Add complete technical documentation

## Project Goals

This project is being developed as a portfolio project demonstrating experience with:

* Embedded C++ development
* Hardware integration
* NFC and RFID technology
* Network communication
* REST API design
* ASP.NET Core
* Secure data validation
* Cryptographic concepts
* Database-backed workflows
* System architecture
* Testable component separation
* Hardware and software simulation

## Limitations

The current repository is an early prototype.

At this stage:

* Test data is hard-coded in several places.
* Some routes and request formats may change.
* No persistent database is connected.
* Authentication and authorization are not implemented.
* Signature verification is not yet performed by the backend.
* NFC keys are prototype values.
* The virtual reader is incomplete.
* Production-grade secret handling is not implemented.
* The system has not undergone a formal security review.

The project should not currently be used to protect real mission-critical equipment or sensitive operational data.

## Documentation

A separate documentation section will be added as development progresses. It is planned to include:

* Complete system architecture
* Database design
* API contracts
* NFC memory layout
* Tag-provisioning workflow
* Signature-generation specification
* Threat model
* Hardware wiring diagram
* Setup instructions
* Testing strategy

## Author

Developed by [Bri4nHoll4nd](https://github.com/Bri4nHoll4nd).
