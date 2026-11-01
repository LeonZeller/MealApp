# MealApp

Die App hilft beim Planen und Kochen von Gerichten. Man plant die Woche und die Einkaufsliste entsteht automatisch.

## Funktionen
- Gerichte selber erstellen 
- Gerichte im Kalender planen
- Gerichte für Frühstück, Mittag und Abend haben jeweils eine andere Farbe
- Wenn man ein Gericht plant kann man ebenfalls die Personenanzahl mit angeben
- Zu jedem geplantem Gericht werden die Zutaten mit auf die wöchentliche Einkaufsliste geschrieben
- Die Zutatenmengen werden an die Personenanzahl angepasst
- Auf die Einkaufsliste kann man auch Haushaltsartikel hinzufügen (zum Beispiel "Seife")
- Die Stückzahl der Haushaltsartikel lässt sich ebenfalls einstellen sowie die Wiederholungsrate (zum Beispiel "Alle 2 Wochen" etc.)  

## Screenshots
<img width="220" alt="Einkaufsliste" src="https://github.com/user-attachments/assets/91011c59-7a58-4b4a-a314-14cb19f95bed" />
<img width="220" alt="Tagesansicht" src="https://github.com/user-attachments/assets/0a3a69be-2c4a-468e-9471-18b8bb1c7d01" />
<img width="220" alt="Kalender" src="https://github.com/user-attachments/assets/a211d161-0d0a-4e32-a471-c724028b9636" />
<img width="220" alt="Gerichte" src="https://github.com/user-attachments/assets/a1ec16dc-da5d-4f55-9d38-84b9f3078c17" />


## Technik
- .NET MAUI
- MVVM
- Aufgeteilt in Models, Views, ViewModels, Services und Controls
- Eigene Steuerelemente: Kalender (`CustomCalendar`) und Tagesansicht (`DayTimelineView`)
- Lokale Datenbank über einen `DatabaseService` (SQLite)
- eigenes Popup-System
- Converter und Behaviors für Data Binding
- Services für Kalender, Mahlzeiten und Einkaufsliste
- Darstellungseinstellungen (helles und dunkles Design)
- Backup-Funktion (in Arbeit)

## Projekt starten
**Voraussetzungen:** Visual Studio 2022 mit dem Workload ".NET MAUI"
1. Repository klonen
2. `MealApp.sln` öffnen
3. Einen Android-Emulator als Ziel auswählen
4. Mit F5 starten

## Status
In Entwicklung, Ziel: Veröffentlichung im Google Play Store.
