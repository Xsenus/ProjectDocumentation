# Windows CI

## Что запускается

Workflow `.github/workflows/windows-build.yml` (`name: Windows build`, job `Build (Release)`):

1. checkout (`persist-credentials: false`);
2. поиск MSBuild через `vswhere.exe` (Visual Studio или Build Tools на ПК);
3. восстановление NuGet-пакетов из `packages.config`: `MSBuild ProjectDocumentation.sln -t:Restore -p:RestorePackagesConfig=true`;
4. сборка: `MSBuild ProjectDocumentation.sln -p:Configuration=Release -m`.

Триггеры: push в `master`, любой pull request из этого репозитория, ручной запуск (`workflow_dispatch`). Новый push в PR отменяет незавершённую сборку этого же PR; сборки `master` не отменяются. Тестов в проекте нет, поэтому CI проверяет сборку.

## Где выполняется

Только Windows-ПК владельца: repository-scoped runner `User16-ProjectDocumentation-Windows` с метками `self-hosted`, `Windows`, `X64`, `github-pc` (инфраструктура описана в `Xsenus/github-runner-onboarding`, `AGENT-ONBOARDING.txt`).

**Резервного запуска на GitHub-hosted runner нет — намеренно.** Решение ссылается на DevExpress 24.2 из GAC (не через NuGet) и требует установленной лицензии DevExpress. На образах GitHub этого нет, поэтому резервная сборка гарантированно упала бы и не означала бы ничего полезного. По той же причине нет входа `runner_target` и независимого observer, как у проектов без лицензированных зависимостей (тот же подход, что у desktop-проверки RentalAccounting).

Код PR из форков на ПК не выполняется (условие `if` у job).

## Требования к ПК

- Windows x64, пользователь `User16` вошёл в систему (после перезагрузки нужен вход; блокировка экрана допустима, сон и выключение — нет);
- Visual Studio или Build Tools с MSBuild 16.5+ и пакетом разработчика .NET Framework 4.7.2;
- DevExpress 24.2 (WinForms) установлен и лицензирован для пользователя `User16` — сборка проектов с `licenses.licx` проверяет лицензию;
- доступ к nuget.org для восстановления пакетов;
- PowerShell 7 (`pwsh`) — supervisor добавляет его в `PATH`.

## Если ПК недоступен

Job остаётся в очереди, пока runner не появится online, и через 24 часа отменяется GitHub. Обойти ПК нельзя (см. выше). Зелёный check в PR означает только успешную сборку на ПК; ожидающий job — не результат проверки.

## Ручной запуск

```bash
gh workflow run windows-build.yml --repo Xsenus/ProjectDocumentation --ref master
```

Ручной запуск доступен после того, как workflow попал в `master`.

## Откат

Чтобы отключить CI, удалите workflow отдельным PR. Регистрацию runner удаляйте после этого, по разделу «Удаление нового экземпляра» в `AGENT-ONBOARDING.txt`; иначе новые запуски будут бесконечно ждать runner.
