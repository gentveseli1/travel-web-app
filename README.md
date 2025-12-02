🌍 Travel Web App – ASP.NET MVC
Projekt për lëndën Arkitektura e Uebit – Kolegji AAB

Ky projekt është zhvilluar si pjesë e lëndës Arkitektura e Uebit në Kolegjin AAB, nën udhëheqjen e profesorit Arber Parduzi.

Aplikacioni simulon funksionalitetet e një agjensioni turistik, ku përdoruesit mund të shfletojnë destinacione dhe oferta, ndërsa administratori menaxhon gjithë përmbajtjen.

👨‍🎓 Ekipi i Projektit
Emri	Email
Gent Veseli	gent.veseli@universitetiaab.com

Endi Makolli	endi.makolli@universitetiaab.com

Rilind Gashi	rilind8.gashi@universitetiaab.com

📌 Përshkrimi i Projektit

Travel Web App është zhvilluar duke përdorur:

ASP.NET MVC

Entity Framework

SQL Server

Razor View Engine

Bootstrap / CSS

Repository Pattern

Session Authentication

QuestPDF për generim të PDF-ve

Funksionalitetet kryesore:

Menaxhimi i Destinacioneve

Menaxhimi i Udhetimeve (Trips)

Menaxhimi i Klientëve (Customers)

Menaxhimi i Rezervimeve (Bookings)

Login & Autentifikim i Administratorit

Generim PDF për rezervime

🔑 Kredencialet e Administratorit (Seeded User)

Aplikacioni krijon automatikisht një admin kur starton:

Email: admin@test.com
Password: 123456

🐳 RUN PROJEKTIN ME DOCKER (Recommended)

Projekti ka Dockerfile dhe docker-compose të gatshëm.

1️⃣ Ndërtoni dhe startoni konteinerët
docker compose up --build

2️⃣ Hap aplikacionin

Në browser shkruaj:

http://localhost:8080

3️⃣ SQL Server është në portin:
localhost:1433


Default credentials:

User: sa

Password: YourStrong!Pass123

📦 Komandat e Docker (të gjitha së bashku)
Start + Build
docker compose up --build

Stop & Remove
docker compose down

Fshi edhe volumet (DB clean)
docker compose down -v

Shiko log-et e aplikacionit
docker logs travel-web-app --tail 200

⚙️ Nei që Docker krijon DB dhe admin-in

Në startup ekzekutohet automatikisht:

Migrimet e Entity Framework

Seed user-i admin

Nuk ka nevojë të bëni manualisht update-database.

🖥️ RUN LOKALISHT (pa Docker)
1️⃣ Klono projektin
git clone https://github.com/gentveseli1/travel-web-app.git
cd travel-web-app

2️⃣ Update appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TravelWebAppDb;Trusted_Connection=True;"
}

3️⃣ Starto migrimet (opsional)
dotnet ef database update

4️⃣ Run aplikacionin
dotnet watch run

📁 Struktura kryesore e Projektit
/Controllers        → Controller-at
/Models             → Modelet EF
/Repositories       → Repository Pattern
/Views              → Razor Views
/Data               → DbContext + Seeder
/Services           → QuestPDF, helper services
/wwwroot            → CSS, JS, Images
/Dockerfile         → Build i aplikacionit
/docker-compose.yml → DB + App Orchestration

🎯 Qëllimi i Projektit

Ky projekt demonstron:

✔ Arkitekturë të pastër MVC
✔ Përdorimin e Entity Framework
✔ Implementimin e Repository Pattern
✔ Punë në grup dhe organizim profesional
✔ Integrim me Docker dhe SQL Server
✔ Generim PDF dhe menaxhim rezervimesh

📄 Licenca

Projekti është realizuar për qëllime akademike dhe demonstrative në kuadër të lëndës Arkitektura e Uebit në Kolegjin AAB.