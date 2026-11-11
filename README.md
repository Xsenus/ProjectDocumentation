# ProjectDocumentation

Настольное приложение для Windows, в котором готовят XML-документы экспертизы проектной документации:

- **заключение экспертизы** (`Conclusion`, схема `01.03`) — сведения об экспертной организации, заявителе, объекте, документах, экспертах и выводах;
- **пояснительная записка** (`ExplanatoryNote`, схема `01.03`) — сведения об авторах, подписантах, застройщике, объекте и исходных документах.

Документы можно импортировать из XML, создавать и редактировать в формах, хранить в локальной базе и выгружать обратно в XML.

## Стек

| Компонент | Версия / назначение |
|---|---|
| .NET Framework | 4.7.2, WinForms |
| DevExpress | 24.2 (XtraGrid, XtraBars, XtraLayout, RichEdit) — нужна установленная лицензия |
| DevExpress XPO | ORM для локальной базы SQLite `xml.s3db` |
| System.Data.SQLite | 1.0.119 |
| Dadata | 23.6.1 — подсказки по организациям и нормализация адресов |
| Autoupdater.NET | 1.9.2 — обязательное автообновление |
| HashLib | CRC32 (вспомогательный код, в интерфейсе не используется) |

## Сборка

Собирается только на Windows с Visual Studio или Build Tools и установленным DevExpress 24.2:

```powershell
nuget restore ProjectDocumentation.sln
msbuild ProjectDocumentation.sln /p:Configuration=Release /m
```

Автоматических тестов в репозитории нет. Release-сборку для `master` и PR владельца выполняет [Windows CI](docs/WINDOWS_RUNNERS.md).

## Документация

| Документ | О чём |
|---|---|
| [docs/architecture.md](docs/architecture.md) | Устройство решения, запуск, хранение данных, поток данных |
| [docs/user-guide.md](docs/user-guide.md) | Работа в приложении: импорт, создание, редактирование, выгрузка |
| [docs/code-reference.md](docs/code-reference.md) | Справочник по классам и методам `Core.NET` и `ProjectDocumentation.NET` |
| [docs/review.md](docs/review.md) | Результаты проверки кода: найденные дефекты, риски, рекомендации |

## Правила разработки

Правила работы с git и PR для людей и агентов — в [AGENTS.md](AGENTS.md).
