# Windows CI

Release-сборка выполняется на Windows-раннере `User16-ProjectDocumentation-ci-Windows` с Visual Studio, .NET Framework 4.7.2 и DevExpress 24.2.5. Восстановление пакетов использует отдельный кэш NuGet и `nuget.org`. Сборка не запускает приложение и не открывает базы данных.

Приватный репозиторий [ProjectDocumentation-ci](https://github.com/Xsenus/ProjectDocumentation-ci) проверяет `master` и готовые PR владельца из веток этого репозитория. Задание проверяет SHA обеих веток перед сборкой временного merge-коммита GitHub. Статус `Windows build (private CI)` публикуется на головном коммите PR с SHA проверенной базы. Внешние форки и черновики пропускаются.

Проверка запускается по расписанию каждые 15 минут, когда ПК доступен. Для ручного запуска владелец может указать номер PR. Без номера проверяются `master` и открытые PR, которым нужна сборка.

```powershell
gh workflow run windows-build.yml --repo Xsenus/ProjectDocumentation-ci --ref main -f pull_request=42
```

Журнал сборки доступен владельцу в [GitHub Actions](https://github.com/Xsenus/ProjectDocumentation-ci/actions/workflows/windows-build.yml).
