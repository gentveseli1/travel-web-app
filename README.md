Travel Web App – ASP.NET MVC

Ky projekt është zhvilluar si pjesë e lëndës Arkitektura e Uebit në Kolegjin AAB, nën udhëheqjen e profesorit Arber Parduzi.

Web aplikacioni shërben si një platformë për një agjension turistik, ku përdoruesit mund të shfletojnë destinacione, ndërsa administratori mund të menaxhojë përmbajtjen përmes panelit të administrimit.

👨‍🎓 Ekipi i Projektit

Ky projekt është realizuar nga tre studentë të Kolegjit AAB:

Gent Veseli gent.veseli@universitetiaab.com

Endi Makolli endi.makolli@universitetiaab.com

Rilind Gashi rilind8.gashi@universitetiaab.com

📌 Përshkrimi i Projektit

Travel Web App është një aplikacion i ndërtuar me ASP.NET MVC, duke përdorur:

ASP.NET MVC Framework

Entity Framework (Code First / Database First – sipas implementimit tuaj)

SQL Server për menaxhimin e databazës

Razor Views (.cshtml) për pjesën e dizajnit të frontend-it

Bootstrap & CSS për stilizim shtesë

Aplikacioni përfshin funksionalitete si:

Listimi i destinacioneve turistike

Detaje të ofertave

Menaxhim për oferta, destinacione, përdorues etj. (në panelin e adminit)

Login & autentifikim bazik për administratorin

🔑 Kredencialet e Administratorit

Për t’u futur në panelin e administratorit, përdorni kredencialet:

Email: admin@example.com

Password: admin132

🛠️ Teknologjitë e Përdorura

ASP.NET MVC 

Entity Framework

SQL Server (MSSQL)

HTML5 / CSS3 / Bootstrap

C#

Razor (.cshtml)

🚀 Si të startohet projekti

Klononi projektin:

git clone https://github.com/gentveseli1/travel-web-app.git


Hapeni projektin në Visual Studio.

Rregulloni string-un e lidhjes në appsettings.json ose Web.config (varësisht nga versioni):

Vendosni emrin e serverit tuaj lokal të SQL Server.

Sigurohuni që databaza ekziston ose përdorni migrimet e Entity Framework.

Bëni run projektin:

Me IIS Express

Ose me Kestrel server (varësisht konfigurimit)

Qasuni admin panelit duke përdorur email-in dhe password-in e mësipërm.

📁 Struktura e Projektit

/Controllers – Controller-at për logjikën e aplikacionit

/Models – Modelet e databazës

/Views – Pamjet cshtml

/wwwroot ose /Content & /Scripts – File statike (CSS, JS, images)

/Migrations – Migrimet e Entity Framework

🎯 Qëllimi i Projektit

Qëllimi i këtij projekti është të demonstrojë:

Implementimin e një aplikacioni web me arkitekturë MVC

Përdorimin e Entity Framework për menaxhim të databazës

Organizim profesional të strukturës së një aplikacioni ueb

Puna në ekip dhe ndarja e detyrave në një projekt praktik

📄 Licenca

Ky projekt është krijuar për qëllime akademike dhe demonstrative.
