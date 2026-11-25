# MealApp

Die App hilft beim Planen und Kochen von Gerichten. Man plant die Woche, und die Einkaufsliste entsteht automatisch.

## Funktionen

**Gerichte**
- Eigene Gerichte mit Zutaten und Zubereitungsschritten erstellen
- Kochmodus, der Schritt für Schritt durch die Zubereitung führt

**Kalender**
- Gerichte im Kalender planen, mit Uhrzeit und Personenanzahl
- Frühstück, Mittag- und Abendessen haben jeweils eine eigene Farbe
- Tagesansicht mit allen Mahlzeiten des Tages
- Beim Planen zeigt die App an, wann man mit dem Kochen anfangen sollte

**Einkaufsliste**
- Die Zutaten aller geplanten Gerichte landen automatisch auf der Einkaufsliste der Woche
- Die Mengen werden an die Personenanzahl angepasst, gleiche Zutaten werden zusammengefasst
- Zwischen den Wochen blättern, mit Anzeige von Kalenderwoche und Zeitraum
- Eigene Haushaltsartikel hinzufügen (zum Beispiel „Seife“), mit Stückzahl und Wiederholungsrate (zum Beispiel „alle 2 Wochen“)

**Einstellungen**
- Helles und dunkles Design
- Backup: alle Daten als Datei exportieren und über das Teilen-Menü speichern, z. B. in Google Drive, und später wieder importieren


## Screenshots
<img width="220" alt="Einkaufsliste" src="https://github.com/user-attachments/assets/91011c59-7a58-4b4a-a314-14cb19f95bed" />
<img width="220" alt="Tagesansicht" src="https://github.com/user-attachments/assets/0a3a69be-2c4a-468e-9471-18b8bb1c7d01" />
<img width="220" alt="Kalender" src="https://github.com/user-attachments/assets/a211d161-0d0a-4e32-a471-c724028b9636" />
<img width="220" alt="Gerichte" src="https://github.com/user-attachments/assets/a1ec16dc-da5d-4f55-9d38-84b9f3078c17" />


## Technik

**Architektur**
- .NET MAUI mit MVVM (CommunityToolkit.Mvvm)
- Aufgeteilt in Models, Views, ViewModels, Services und Controls
- Dependency Injection über den MAUI-Container
- Services hinter Interfaces (`ICalendarService`, `IShoppingService`), damit die ViewModels unabhängig von der Datenbank getestet werden können

**Daten**
- Lokale SQLite-Datenbank über einen zentralen `DatabaseService`
- Durchgehend asynchron mit async/await, der `DatabaseService` arbeitet mit `ConfigureAwait(false)`
- Thread-sichere Initialisierung der Datenbank mit `SemaphoreSlim`
- Kalendereinträge und Gerichte werden mit zwei Abfragen geladen statt mit einer Abfrage pro Eintrag

**Oberfläche**
- Eigene Steuerelemente: Kalender (`CustomCalendar`) und Tagesansicht (`DayTimelineView`)
- Eigenes Popup-System
- Converter und Behaviors für Data Binding

## Tests

Das Projekt `MealApp.Tests` enthält Unit-Tests mit **xUnit**, aufgebaut nach dem Muster Arrange, Act, Assert.

- **`CustomShoppingItemTests`:** prüft, in welchen Wochen ein Haushaltsartikel abhängig von Wiederholungsrate und letztem Kaufdatum auf der Liste erscheint, inklusive Randfällen
- **`ShoppingListViewModelTests`:** prüft mit Fake-Services, dass die Zutaten der geplanten Gerichte richtig und mit der Personenanzahl hochgerechnet auf der Einkaufsliste landen

## Projekt starten

**Voraussetzungen:** Visual Studio 2022 mit dem Workload „.NET MAUI“

1. Repository klonen
2. `MealApp.sln` öffnen
3. Einen Android-Emulator als Ziel auswählen
4. Mit F5 starten

**Tests ausführen:** In Visual Studio über **Test → Test-Explorer** und dann **Alle Tests ausführen**.

## Status

In Entwicklung, Ziel: Veröffentlichung im Google Play Store.
