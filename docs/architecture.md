# Архитектура

## Состав решения

`ProjectDocumentation.sln` содержит два проекта на .NET Framework 4.7.2.

| Проект | Тип | Назначение |
|---|---|---|
| `Core.NET` | библиотека | Модели XML-схем, модели базы данных (XPO), контроллеры загрузки и сериализации, справочники, вспомогательные расширения, клиент DaData |
| `ProjectDocumentation.NET` | WinExe | Интерфейс на WinForms + DevExpress: главное окно, формы редактирования, обёртки для отображения в таблицах |

```
Core.NET/
  AutoModels/
    ConclusionObj/Conclusion.cs        модель заключения (сгенерирована XmlSchemaClassGenerator по XSD)
    ExplanatoryNoteObj/ExplanatoryNote.cs  модель пояснительной записки (xsd.exe + ручные правки)
    ExplanatoryNoteObj/tWorkPerson.cs, ExplanatoryNoteObjType.cs
    ItemChoiceType.cs                  выбор «номер в формате ЕГРЗ / произвольный»
  Models/XPO/                          таблицы базы и справочники
  Controllers/                         сессия БД, загрузка XML, заполнение справочников
  Extensions/                          сериализация UTF-8, enum, строки, байты
  Algorithms/                          CRC32 (не используется интерфейсом)
  DaDataObj/                           клиент сервиса DaData
ProjectDocumentation.NET/
  Program.cs                           точка входа
  Forms/MainForm, FileLoad, ViewForm   главное окно, импорт, предпросмотр
  Forms/ObjectChange/*Edit             формы редактирования разделов
  Models/ConclusionObj, ExplanatoryNoteObj  обёртки строк для таблиц (PersonObj, DocumentObj …)
  Others/ProjectDocumentation.NET_(v01.02).zip  архив старой сборки (в коде не используется)
```

## Запуск приложения

1. `Program.Main` вызывает `SessionController.GetSession()`: создаётся или открывается SQLite-файл `xml.s3db`, XPO обновляет схему (`AutoCreateOption.DatabaseAndSchema`).
2. Открывается `MainForm`. В `MainForm_Load` последовательно:
   - `DataBaseController.FillingDataBaseAsync()` — синхронизирует справочники `GeneralizedDictionaryXPO` со встроенным списком кодов;
   - `UpdateGridDataSourceAsync()` — грузит все документы (`XmlObjXPO`), отсортированные по дате создания (новые сверху);
   - `InitialSetupGridControl()` — показывает в таблице только колонки «Наименование», «ID», «Дата создания»;
   - `CheckUpdates()` — читает настройку `UpdatePath` (по умолчанию FTP-адрес) и запускает обязательное обновление через AutoUpdater.NET.

## Хранение данных

База хранится в файле SQLite `xml.s3db` рядом с исполняемым файлом. При запуске XPO синхронно создаёт и обновляет схему по классам модели.

| Класс (таблица) | Поля | Назначение |
|---|---|---|
| `BaseObjectXPO` | `Oid` (XPObject), `Guid`, `DateCreate` | База для всех таблиц; `Guid` и `DateCreate` выставляются в `AfterConstruction` |
| `XmlObjXPO` : `BaseObjectXPO` | `Name`, `Obj` (`byte[]`) | Документ: имя и весь XML в UTF-8 |
| `ConclusionXPO` : `XmlObjXPO` | — | Заключение экспертизы |
| `ExplanatoryNoteXPO` : `XmlObjXPO` | — | Пояснительная записка |
| `GeneralizedDictionaryXPO` : `BaseObjectXPO` | `GeneralizedDictionary` (вид), `Value`, `Name` | Строка справочника: `Value` — код из XSD, `Name` — подпись в интерфейсе |
| `SettingXPO` : `BaseObjectXPO` | `Name`, `Value` (до 2048 символов) | Настройки; сейчас используется только `UpdatePath` |

Документ хранится целиком как сериализованный XML. Отдельных таблиц под разделы заключения нет: при открытии XML десериализуется в объект `Conclusion` / `ExplanatoryNote`, формы меняют объект, при сохранении он сериализуется обратно.

### Справочники

`GeneralizedDictionary` перечисляет 35 справочников: результат, форма, вид, стадия экспертизы, вид работ, предмет экспертизы, вид объекта, источник финансирования и уровень бюджета, климатические, сейсмические, ветровые, снеговые районы, инженерно-геологические условия, регионы РФ (90 записей), направления деятельности экспертов (183), виды документов (190), виды инженерных изысканий, «да/нет», расположение, опасность, уникальность, класс опасности, категории земель, пожарная опасность, уровень ответственности и девять видов выводов.

`DataBaseController.FillingDataBaseAsync` проверяет каждую категорию отдельно. Записи добавляются, только если в категории нет ни одной строки. Новые значения, добавленные в код позже, в существующую базу не попадут.

Связь «справочник ↔ XML»: `GeneralizedDictionaryXPO.Value` совпадает с `XmlEnumAttribute` нужного enum модели. Перевод выполняют `ConclusionEdit.GetEnumByAttributeValue<T>` (код → enum) и `EnumExtension.GetEnumXmlAttributeValue` (enum → код).

## Поток данных

```
Импорт:  XML-файл ──Utf8StringSerialize.DeSerializeObject──▶ Conclusion/ExplanatoryNote
                    ──SerializeObject──▶ XmlObjXPO.Obj (byte[]) ──▶ xml.s3db

Правка:  XmlObjXPO.Obj ──GetConclusionByString──▶ объект ──▶ ConclusionEdit / ExplanatoryNoteEdit
                    ──btnSave──▶ объект ──GetSerializeConclusion──▶ XmlObjXPO.Obj ──▶ xml.s3db

Выгрузка: XmlObjXPO.Obj ──строковые замены (схема, xsl)──▶ {conclusion|explanatoryNote}_{Guid}.xml
```

## Внешние сервисы

| Сервис | Где | Зачем |
|---|---|---|
| DaData (Suggest `FindParty`, Clean `Address`) | `Core.NET/DaDataObj/DaDataObj.cs` | Заполнение реквизитов организации по ИНН/ОГРН и разбор адреса |
| FTP-сервер обновлений | `MainForm.CheckUpdates`, настройка `UpdatePath` | Файл `ProjectDocumentation.NET.Update.xml` для AutoUpdater.NET |

Ключи DaData сейчас зашиты в исходный код — см. [review.md](review.md).

## Версии

- `ProjectDocumentation.NET`: `AssemblyVersion 3.0.0.7`.
- `Core.NET`: `AssemblyVersion 3.0.0.5`.
- Версия XML-схем по умолчанию: `01.03` (её можно поменять в поле «Версия» формы).
