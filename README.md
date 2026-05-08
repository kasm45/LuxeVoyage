\# LuxeVoyage



LuxeVoyage is a premium travel reservation and management web application built with ASP.NET Core MVC.



The project focuses on a modern luxury travel experience with role-based reservation workflows, approval-based payments, analytics dashboards, and responsive UI/UX.



\---



\## Features



\### Public Catalog



\* Destinations

\* Experiences

\* Tours

\* Stays / Hotels

\* Points of Interest

\* Responsive premium UI



\### Traveler Features



\* Account registration \& login

\* Save trips for later

\* Basket management

\* Reservation requests

\* Pending / Approved / Paid trip lifecycle

\* My Trips dashboard

\* Demo payment checkout

\* Reservation history



\### Admin Features



\* Dashboard analytics

\* Revenue tracking

\* User \& personnel management

\* Removed/anonymized account handling

\* Reservation management

\* Accept / Reject booking requests

\* Catalog CRUD management

\* Destination visibility management



\### Personnel Features



\* Reservation review workflow

\* Accept / Reject operations

\* Limited admin access



\---



\## Booking \& Payment Lifecycle



```text

Traveler submits request

→ Booking becomes Pending

→ Admin / Personnel reviews request

→ Accepted booking unlocks payment

→ Traveler completes demo payment

→ Trip moves to My Trips

→ Admin revenue updates

```



\---



\## Tech Stack



\* ASP.NET Core MVC

\* Entity Framework Core

\* SQLite

\* ASP.NET Core Identity

\* Razor Views

\* Tailwind CSS

\* Git \& GitHub



\---



\## Roles



\### Traveler



Can browse the catalog, save items, submit reservation requests, pay approved bookings, and manage trips.



\### Personnel



Can review reservation requests and approve/reject them.



\### Admin



Can manage users, reservations, analytics, payments, and catalog content.



\---



\## Demo Payment Cards



Use demo card numbers only:



```text

Success Card:

4242 4242 4242 4242



Failure Card:

4000 0000 0000 0002

```



No real payment processing occurs.



\---



\## Getting Started



\### Clone Repository



```bash

git clone https://github.com/kasm45/LuxeVoyage.git

cd LuxeVoyage/LuxeVoyage.Mvc

```



\### Restore Packages



```bash

dotnet restore

```



\### Apply Database Migrations



```bash

dotnet ef database update

```



\### Run Application



```bash

dotnet run

```



Open in browser:



```text

http://localhost:5020

```



\---



\## Useful Commands



```bash

dotnet build

dotnet ef migrations has-pending-model-changes

dotnet run

```



\---



\## Demo Notes



This project uses a simulated payment workflow for portfolio and educational purposes.



No real banking transaction occurs.



Seeded demo accounts should be changed or disabled before any public production deployment.



\---



\## Portfolio Highlights



\* Approval-based payment workflow

\* Admin / Personnel / Traveler role separation

\* Premium responsive UI

\* Analytics dashboard

\* Reservation lifecycle management

\* Safe removed-user handling

\* EF Core migrations

\* SQLite integration

\* Payment gating after approval

\* Cart + My Trips integration



\---



\## Future Improvements



\* Stripe / iyzico integration

\* Automated tests

\* Docker support

\* CI/CD pipeline

\* Email notifications

\* Real-time admin notifications

\* Cloud deployment



\---



\## License



This project is intended for educational and portfolio purposes.



