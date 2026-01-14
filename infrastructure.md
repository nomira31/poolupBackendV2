 
# 🧠 CLEAN ARCH + CQRS CHEAT SHEET (Your Project)

## 🎯 ONE RULE TO REMEMBER

> **Code depends inward.
> Details depend on abstractions.**

---

## 🧱 LAYERS (OUTER → INNER)

```
API → Application → Core ← Infrastructure
```

* API depends on Application
* Application depends on Core
* Infrastructure depends on Core
* **Core depends on NOTHING**

---

# 1️⃣ CORE — “What the business IS”

📍 `src/Core`

### Contains:

* **Entities** (domain objects)
* **Interfaces** (contracts / ports)
* **DTOs** (data shapes)

### Contains ❌:

* EF Core
* DbContext
* HTTP
* Redis
* Frameworks

---

### 🔹 Entity

📍 `Core/Entities`

```csharp
class User
```

**Rule:**

* Represents business concept
* No database logic
* No serialization logic

---

### 🔹 DTO

📍 `Core/DTOs`

```csharp
class UserProfileDto
```

**Rule:**

* Used to SEND or RECEIVE data
* Safe shape (no secrets)
* Not business logic

---

### 🔹 Interface (PORT)

📍 `Core/Interfaces`

```csharp
interface IUserRepository
```

**Rule:**

* Describes WHAT is needed
* Not HOW it’s done
* No EF, no SQL

---

# 2️⃣ APPLICATION — “What the system DOES”

📍 `src/Application`

### Contains:

* **Commands** (write)
* **Queries** (read)
* **Handlers**
* **Business workflows**

### Contains ❌:

* Controllers
* DbContext
* HTTP concerns

---

## 🟢 COMMAND (WRITE)

📍 `Application/Commands`

```csharp
RegisterUserCommand
RegisterUserCommandHandler
```

**Rule:**

* Changes state
* Uses interfaces
* May return IDs / status

---

## 🔵 QUERY (READ)

📍 `Application/Queries`

```csharp
GetUserProfileQuery
GetUserProfileQueryHandler
```

**Rule:**

* Read-only
* Returns DTOs
* No domain mutation

---

# 3️⃣ INFRASTRUCTURE — “How things are done”

📍 `src/Infrastructure`

### Contains:

* EF Core
* DbContext
* Repositories
* Redis
* Email
* External APIs

---

### 🔹 Adapter (IMPLEMENTATION)

```csharp
class UserRepository : IUserRepository
```

**Rule:**

* Implements Core interfaces
* Uses EF / Redis / SMTP
* Can be replaced anytime

---

# 4️⃣ API — “How the world talks to us”

📍 `src/Api`

### Contains:

* Controllers
* Routing
* HTTP stuff

### Contains ❌:

* Business logic
* EF queries
* Rules

---

### 🔹 Controller

```csharp
UsersController
```

**Rule:**

* Thin
* Calls MediatR
* Returns HTTP responses only

---

# 🔁 CQRS FLOW (THIS IS THE MAGIC)

### WRITE (Command)

```
HTTP → Controller
     → Command
     → Handler
     → Interface
     → Infrastructure
     → DB
```

---

### READ (Query)

```
HTTP → Controller
     → Query
     → Handler
     → DbContext / Read model
     → DTO
```

---

# 🚨 GOLDEN RULES (PRINT THESE)

1️⃣ Core has **zero dependencies**
2️⃣ Controllers are **dumb**
3️⃣ Handlers contain **logic**
4️⃣ Infrastructure is **replaceable**
5️⃣ Entities ≠ DTOs
6️⃣ Interfaces live in Core
7️⃣ MediatR is the traffic cop

 
ARCHITECTURE AND PLAN BELOW 


 
---

# 1️⃣ ERD: Entities & Relationships

```
┌────────────┐       ┌───────────────┐       ┌──────────────┐
│   Users    │       │  DriverVerif  │       │  Clusters    │
├────────────┤       ├───────────────┤       ├──────────────┤
│ Id (PK)    │◄─────▶│ DriverId (FK) │       │ Id (PK)      │
│ FullName   │       │ IDDocUrl      │       │ Destination  │
│ Email      │       │ VehicleDocUrl │       │ TimeWindow   │
│ Phone      │       │ Badge         │       │ Status       │
│ Role       │       │ SeatCheck     │       │ MaxRiders    │
│ Password   │       │ ACCheck       │       │ CreatedAt    │
│ IsVerified │       │ VerifiedAt    │       └──────────────┘
└────────────┘       └───────────────┘
       │
       │
       ▼
┌───────────────┐
│ ClusterInterest│
├───────────────┤
│ Id (PK)       │
│ ClusterId (FK)│─────▶ Clusters
│ UserId (FK)   │─────▶ Users
│ CreatedAt     │
└───────────────┘
       │
       ▼
┌──────────────┐
│ ClusterDriver│
├──────────────┤
│ Id (PK)      │
│ ClusterId(FK)│─────▶ Clusters
│ DriverId(FK) │─────▶ Users
│ CreatedAt    │
└──────────────┘
       │
       ▼
┌────────────┐
│   Trips    │
├────────────┤
│ Id (PK)    │
│ ClusterId(FK)│─────▶ Clusters
│ DriverId(FK) │─────▶ Users
│ StartTime  │
│ EndTime    │
│ Status     │
└────────────┘
       │
       ▼
┌────────────┐
│ TripRiders │
├────────────┤
│ Id (PK)    │
│ TripId(FK) │─────▶ Trips
│ RiderId(FK)│─────▶ Users
│ Status     │
└────────────┘
       │
       ▼
┌──────────────┐
│ Notifications│
├──────────────┤
│ Id (PK)      │
│ UserId(FK)   │─────▶ Users
│ Type         │
│ Payload(JSON)│
│ SentAt       │
└──────────────┘
```

### ✅ Notes:

* **Users** table handles both Rider & Driver.
* **DriverVerif** only exists for drivers, keeps safety/trust info.
* **Clusters** = destination + time window; links riders and drivers.
* **Trips** = actual ride scheduled from cluster + driver + riders.
* **Notifications** = flexible JSON payload per user.

---

# 2️⃣ MVP User Flow → Commands & Queries

Here’s **how the CQRS flow maps**:

### **User registration**

```
HTTP POST /api/users/register
  → RegisterUserCommand
    → Handler
      → IUserRepository.AddAsync
      → Return userId
```

---

### **Driver verification**

```
HTTP POST /api/drivers/verify
  → VerifyDriverCommand
    → Handler
      → IDriverVerificationRepository.AddAsync
      → Update Users.IsVerified
```

---

### **Join a cluster (rider)**

```
HTTP POST /api/clusters/join
  → AddInterestCommand
    → Handler
      → ClusterInterestRepository.AddAsync
      → Check if cluster ready
        → If threshold met, ActivateClusterCommand
```

---

### **Activate a cluster**

```
Command: ActivateClusterCommand
Handler:
  → Change Cluster.Status = Active
  → Assign driver(s) (ClusterDrivers)
  → Create Trip
  → Notify riders
```

---

### **Get active clusters / trips**

```
Query: GetClustersQuery / GetTripQuery
Handler:
  → Return DTOs (Cluster info, Trip info)
```

---

### **Send notifications**

```
Command: SendNotificationCommand
Handler:
  → INotificationRepository.AddAsync
  → Push/Email/WhatsApp
```

---

# 3️⃣ Key CQRS Command/Query Mapping

| Action            | Type    | Repository/Interface                |
| ----------------- | ------- | ----------------------------------- |
| Register user     | Command | IUserRepository                     |
| Verify driver     | Command | IDriverVerificationRepository       |
| Join cluster      | Command | IClusterInterestRepository          |
| Activate cluster  | Command | IClusterRepository + ClusterDrivers |
| Schedule trip     | Command | ITripRepository + TripRiders        |
| Send notification | Command | INotificationRepository             |
| List clusters     | Query   | IClusterRepository                  |
| Get trip / ride   | Query   | ITripRepository                     |

---

# 4️⃣ Next Steps (“Cooking Plan”)

1. **Create Core entities & interfaces** (Users, Clusters, Trips, DriverVerif).
2. **Create Infrastructure** (DbContext, Repositories).
3. **Define Commands & Handlers** (MediatR) for MVP flows:

   * Register user
   * Verify driver
   * Join cluster
   * Activate cluster & schedule trip
4. **Define Queries & Handlers** for reading data.
5. **Create API Controllers** (thin, MediatR-only).
6. **Wire environment configs** (dev/prod DB, API keys).
7. **Test end-to-end flows locally**.

---
 