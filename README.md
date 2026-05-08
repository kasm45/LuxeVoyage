# ✨ LuxeVoyage

<p align="center">
  Premium Travel Reservation & Management Platform
</p>

<p align="center">
  Built with ASP.NET Core MVC, EF Core, SQLite, Identity & Tailwind CSS
</p>

<p align="center">
  <img src="https://img.shields.io/badge/ASP.NET_Core_MVC-8.0-512BD4?style=for-the-badge&logo=.net" />
  <img src="https://img.shields.io/badge/Entity_Framework_Core-8.0-68217A?style=for-the-badge" />
  <img src="https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite" />
  <img src="https://img.shields.io/badge/TailwindCSS-06B6D4?style=for-the-badge&logo=tailwindcss" />
  <img src="https://img.shields.io/badge/Status-Active-success?style=for-the-badge" />
</p>

---

## 🌍 Overview

LuxeVoyage is a premium luxury travel reservation and management platform focused on:

* curated travel experiences
* approval-based reservation workflows
* admin/personnel management
* simulated payment lifecycle
* responsive premium UI/UX
* analytics & booking operations

The project was designed as a portfolio-quality full-stack ASP.NET Core MVC application.

---

# ✨ Core Features

| Module                | Features                                         |
| --------------------- | ------------------------------------------------ |
| 🌎 Catalog            | Destinations, Experiences, Tours, Hotels/Stays   |
| 🧳 Basket System      | Saved trips, pending requests, approved payments |
| 💳 Payments           | Approval-based demo payment workflow             |
| 👤 Identity           | ASP.NET Identity authentication & roles          |
| 🛠 Admin Panel        | Reservation management, analytics, revenue       |
| 👨‍💼 Personnel Panel | Accept / Reject booking requests                 |
| 📊 Analytics          | Revenue, reservations, user statistics           |
| 🔒 Security           | Removed-user anonymization, ownership checks     |
| 📱 UI/UX              | Responsive premium interface with Tailwind CSS   |

---

# 🔄 Reservation Lifecycle

```text
Traveler submits reservation request
        ↓
Booking becomes Pending
        ↓
Admin / Personnel reviews request
        ↓
Booking accepted
        ↓
Payment unlocked
        ↓
Traveler completes payment
        ↓
Trip appears in My Trips
        ↓
Revenue appears in Admin Dashboard
```

---

# 👥 Roles

## Traveler

* Browse luxury travel catalog
* Save trips for later
* Submit reservation requests
* Pay approved bookings
* Track trip history

## Personnel

* Review reservation requests
* Approve / reject reservations
* Manage concierge workflow

## Admin

* Manage users & personnel
* View analytics dashboard
* Track revenue
* Manage catalog content
* Control reservation lifecycle

---

# 💳 Demo Payment Cards

Use demo card numbers only:

| Type      | Card                  |
| --------- | --------------------- |
| ✅ Success | `4242 4242 4242 4242` |
| ❌ Failure | `4000 0000 0000 0002` |

> No real payment processing occurs.

---

# 🛠 Tech Stack

## Backend

* ASP.NET Core MVC
* Entity Framework Core
* ASP.NET Identity
* SQLite

## Frontend

* Razor Views
* Tailwind CSS
* Responsive UI Components

## Architecture

* MVC Pattern
* ViewModels
* ViewComponents
* Service/Helper utilities
* EF Core Migrations

---

# 🚀 Getting Started

## Clone Repository

```bash
git clone https://github.com/kasm45/LuxeVoyage.git
cd LuxeVoyage/LuxeVoyage.Mvc
```

## Restore Packages

```bash
dotnet restore
```

## Apply Database Migrations

```bash
dotnet ef database update
```

## Run Application

```bash
dotnet run
```

Open:

```text
http://localhost:5020
```

---

# 📦 Useful Commands

```bash
dotnet build
dotnet ef migrations has-pending-model-changes
dotnet run
```

---

# 🔐 Security Highlights

* Ownership checks on bookings/payments
* Approval-gated payment flow
* Removed-user anonymization
* Anti-forgery protection
* Role-based authorization
* Demo payment isolation
* No CVV/full PAN storage

---

# 📈 Portfolio Highlights

✅ Full booking lifecycle
✅ Admin / Personnel role separation
✅ Approval-based payments
✅ Analytics dashboard
✅ Basket + My Trips integration
✅ Responsive premium UI
✅ EF Core migrations
✅ Removed-user audit-safe handling

---

# 🧠 Future Improvements

* Stripe / iyzico integration
* Automated integration tests
* Docker support
* CI/CD pipeline
* Email notifications
* Cloud deployment
* Real-time admin notifications

---

# 📄 License

This project is intended for educational and portfolio purposes.
