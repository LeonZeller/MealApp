# MealApp

Die App hilft einem, Gerichte zu kochen, zu planen. Man plant die Woche und die Einkaufsliste entsteht automatisch.

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



## Technik
- .NET MAUI
- MVVM
- Aufgeteilt in Models, Views, ViewModels, Service und Controls
- Eigene Steuerelemente: Kalender(CustomCalendar) und Tagesansicht(DayTimelineView)
- Lokale Datenbank über einen DatabaseService (SQLite)
- eigenes Popup-System
- Converter und Behaviors für Data Binding
- Services für Kalender, Mahlzeiten und Einkaufsliste
- Darstellungseinstellungen bzw. Themes und eine Backup-Funktion

## Projekt starten
**Voraussetzungen:** Visual Studio 2022 mit dem Workload ".NET MAUI"
1. Repository klonen
2. 'MealApp.sln' öffnen
3. Einen Android-Emulator als Ziel auswählen
4. Mit F5 starten

## Status
In Entwicklung, Ziel: Veröffentlichung im Google Play Store.
