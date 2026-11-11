# Справочник по коду

Справочник описывает исходную версию приложения, по которой составлен [отчёт проверки](review.md). Изменения последующих версий отражены в связанных PR.

Описано фактическое поведение каждого метода по исходному коду. Ссылки `D-NN` ведут к дефектам в [review.md](review.md).

## Core.NET

### Controllers/SessionController

| Член | Поведение |
|---|---|
| `GetSession()` | Возвращает общий `Session` XPO поверх `ThreadSafeDataLayer`. Если сессии нет или она отключена — вызывает `CreateDataBase()` и создаёт новую. `LockingOption.None`, `TrackPropertiesModifications = true` |
| `CreateDataBase()` | `async void`: во временной сессии `SimpleDataLayer` вызывает `CreateObjectTypeRecordsAsync` и `UpdateSchemaAsync`. Вызывающий код не ждёт завершения (D-14) |
| `ConnectionString` | `SQLiteConnectionProvider.GetConnectionString("xml.s3db")` — относительный путь (D-15) |

### Controllers/ConclusionController

| Метод | Поведение |
|---|---|
| `CreateConclusionByPath(path, name)` | Читает файл через `GetConclusionByPath`. При неудаче возвращает `null`. Создаёт `ConclusionXPO` с именем `name`; GUID берёт из `ConclusionGuid`, иначе из имени файла, иначе оставляет сгенерированный. Сохраняет XML в `Obj` и пишет в базу |
| `GetGuidByFile(path)` | Делит имя файла без расширения по `_` и возвращает первую часть, которая парсится как GUID |
| `GetConclusionByPath(path)` | Если файл существует — `Utf8StringSerialize.DeSerializeObject<Conclusion>`, иначе `null`. Пустой путь — `ArgumentNullException` |
| `GetConclusionByString(xml)` | Пустая строка — `null`, иначе десериализация из строки |
| `GetSerializeConclusion(this Conclusion)` | `Utf8StringSerialize.SerializeObject`; при ошибке возвращает `null` (D-11) |

### Controllers/ExplanatoryNoteController

То же для `ExplanatoryNote`. Отличие: GUID берётся только из имени файла, иначе `Guid.NewGuid()`. Атрибут `id` записки не используется.

### Controllers/FileController

`CheckFile(path)` — `ArgumentNullException` для пустого пути, `true` если файл существует.

### Controllers/DataBaseController

`FillingDataBaseAsync()` → `GeneralizedDictionaryXPOAsync()`. Загружает весь справочник одним запросом. Для каждого из 35 видов, у которого нет ни одной записи, сохраняет предопределённые строки — каждую отдельным `SaveAsync`.

### Models/XPO

| Класс | Ключевое |
|---|---|
| `BaseObjectXPO` | `Guid` (nullable, приватный setter, `SetGuid`), `DateCreate` (приватный setter). Оба задаются в `AfterConstruction`. `ToString()` — GUID |
| `XmlObjXPO` | `Name`; `Obj` — `byte[]` UTF-8. `SetObj(string)` (пустая строка → `null`), `GetObj()`. `NameXML` — имя или `"NameXML"` |
| `ConclusionXPO`, `ExplanatoryNoteXPO` | Наследники `XmlObjXPO` без собственных полей; нужны, чтобы различать тип документа |
| `GeneralizedDictionaryXPO` | `GeneralizedDictionary?`, `Value`, `Name`, вычисляемое `ValueInt`. `ToString()` = `"[Value] Name"` |
| `SettingXPO` | `Name`, `Value` до 2048 символов |
| `GeneralizedDictionary` | enum из 35 видов справочников |
| `TypeOrganization` | enum: ЮЛ, иностранное ЮЛ, ИП, ФЛ (с `Description`) |

### AutoModels

- `ConclusionObj/Conclusion.cs` — около 8 000 строк, сгенерированных XmlSchemaClassGenerator 3.0.1185: 72 типа (`Conclusion`, `TOrganization`, `TDeclarant`, `TDocument`, `TObject`, `TExpert`, `TSummary` и т. д.) с атрибутами `MaxLength`/`RegularExpression` из XSD. Корень — `<Conclusion>` с атрибутами `ConclusionGUID`, `SchemaVersion`, `SchemaLink`. Коллекции инициализируются в конструкторах, setter у них приватный.
- `ExplanatoryNoteObj/ExplanatoryNote.cs` — около 3 300 строк, около 90 типов (`tIndustrialObject`, `tLinearObject`, `tAuthor`, `tDocument`, `tTextBlock` …). Корень `ExplanatoryNote`, `SchemaVersion = "01.03"` в конструкторе. Коллекции — массивы.
- `ItemChoiceType` — `EGRZ` / `noEGRZ`; `ExplanatoryNoteObjType` — производственный, непроизводственный, линейный.

Сгенерированные файлы вручную не правят: при обновлении XSD их перегенерируют. Исключение — `ExplanatoryNote.cs`, в котором уже есть ручные правки (например, закомментированный `UsedNorms`); при перегенерации их нужно перенести.

### Extensions/Utf8StringSerialize

| Метод | Поведение |
|---|---|
| `SerializeObject<T>(obj)` | `XmlSerializer` в `StringWriter` с UTF-8. Любое исключение → `null` |
| `DeSerializeObjectByString<T>(xml, lenient = true)` | Строгая десериализация. При ошибке и `lenient` — `NormalizeProblematicEnums` и повтор. Любая ошибка → `default` |
| `DeSerializeObject<T>(path, lenient = true)` | Строгая попытка из `FileStream`, где кодировку определяет XML-декларация. Повтор — через `File.ReadAllText(path, UTF8)` с нормализацией (D-10) |
| `NormalizeProblematicEnums(xml)` | Для восьми элементов-выводов (`EngineeringSurveySummary`, `EstimateSummary`, `ProjectDocumentsSummary` …) удаляет элемент, если значение пустое или не входит в `XmlEnum` соответствующего типа. Словарь синонимов пуст: все примеры закомментированы |

### Extensions/EnumExtension

| Метод | Поведение |
|---|---|
| `GetEnumXmlAttributeValue(this Enum)` | Имя из `[XmlEnum]` или `ToString()`; для `null` — `null` |
| `GetEnumXmlAttributeValueInt` | То же, распарсенное в `int?` |
| `GetEnumDescription` | Текст `[Description]` или `ToString()` |
| `GetDefaultEnum<T>(text)` | Ищет значение по описанию; не нашёл — `default(T)` |

### Extensions — прочее

| Метод | Поведение |
|---|---|
| `StringExtension.StringToByte` | UTF-8 байты; для пустой строки или строки из пробелов — `null` |
| `StringExtension.SplitLenght(str, n)` | Разбивает строку на куски по `n` символов через перевод строки; пустая строка или `n < 1` → `ArgumentException` |
| `ByteExtension.ByteToString` | UTF-8 строка; `null` → `null` |
| `ObjectExtension.GetCounterPartyDescription` | Через рефлексию собирает «Наименование / ОГРН / ИНН / КПП / Адрес / E-mail» (или ФИО и ОГРНИП) для ЮЛ и ИП. Для типов со свойством, в имени которого есть `item`, возвращает пустую строку |
| `AppExtensions.ReadStream` | Копирует поток в массив. Нужен поток с `Length` (D-22) |
| `AppExtensions.GetXORSum` | XOR массивов байт; изменяет первый массив на месте и глотает исключения (D-22) |
| `AppExtensions.ConsoleWriteLine*` | Цветной вывод в консоль (в WinExe не используется) |

### Algorithms

`CRC32 : IAlgorithm`. Методы `ComputeHashFile(path|Stream)` используют собственную таблицу (полином `0xEDB88320`), методы `byte[]`/`string` — HashLib. В интерфейсе не используется. `ValueString` у HashLib-вариантов формируется неверно (D-22).

### DaDataObj

| Метод | Поведение |
|---|---|
| `GetPartyAsync(query)` | `SuggestClientAsync.FindParty` — поиск организации по ИНН/ОГРН |
| `GetAddressAsync(query)` | `CleanClientAsync.Clean<Address>` — нормализация адреса (платный метод) |

Клиенты создаются лениво, ключи зашиты в коде (D-06).

## ProjectDocumentation.NET

### Program

`Main`: `SessionController.GetSession()` → `EnableVisualStyles` → `Application.Run(new MainForm())`.

### Forms/MainForm

| Обработчик | Поведение |
|---|---|
| `MainForm_Load` | Справочники → таблица → настройка колонок → обновления |
| `CheckUpdates` | Получает или создаёт `SettingXPO("UpdatePath")`, `AutoUpdater.Mandatory = true`, `AutoUpdater.Start`. Исключения глотаются |
| `UpdateGridDataSourceAsync(column, value)` | Перечитывает все `XmlObjXPO` по убыванию `DateCreate`, фокус на строке со значением |
| `UpdateGridDataSource(rowHandle)` | То же с фокусом по номеру строки |
| `gridView_PopupMenuShowing` | Включает или выключает пункты меню в зависимости от выбранной строки, показывает меню у курсора |
| `barBtnLoad_ItemClick` | `FileLoad`; если что-то загружено — обновить таблицу и выделить запись |
| `barBtnAddConclusion_ItemClick` | Открывает текущую форму заключения 01.04 |
| `barBtnAddExplanatoryNote_ItemClick` | Открывает текущую форму пояснительной записки 01.07 |
| «Классические формы 01.03» | Создаёт заключение в `ConclusionEdit` или записку в `ExplanatoryNoteEdit`. Сохранение использует отдельную запись XPO |
| `EditGridViewObjectAsync` («Изменить» и двойной клик) | Выбирает редактор по корню и версии XML. Для архивных заключений предлагает преобразование в 01.04 |
| `barBtnBaseSetting_ItemClick` | Только заключение: `ConclusionBaseSettingEdit(ExpertOrganization, Approver)` → запись обратно |
| `barBtnView_ItemClick` | Для поддерживаемых схем выводит предпросмотр по XSL. Для архивного заключения доступен просмотр исходного XML |
| `barBtnFill_ItemClick` | Выгружает XML выбранной записи. Для схем с комплектными файлами экспорт сохраняет соответствующие XSD и XSL |
| `barBtnDel_ItemClick` | Подтверждение → `DeleteAsync` → обновление таблицы |

### Forms/FileLoad

`btnFile_ButtonPressed` открывает выбор файла или очищает путь. `btnSave_Click` определяет семейство по корню XML, версию по атрибуту, проверяет формат и сохраняет запись. Исходный XML архивных заключений остаётся доступен для отдельной выгрузки.

### Актуальные редакторы и сервис XML

| Файл | Назначение |
|---|---|
| `Forms/Conclusion0104Form.cs` | `SchemaDocumentForm` управляет документом, режимами ввода, черновиком, диагностикой и предпросмотром. Производные формы выбирают семейство и версию |
| `Forms/NativeSchemaDocumentSections.cs` | Вкладки, список блоков и ввод в режиме «Разделы» на DevExpress. Правки попадают в тот же `XDocument`, что и правки из дерева |
| `Forms/NativeSchemaDocumentFields.cs` | Дерево XSD, ввод полей, повторения, вложения, переход по ошибке и подстановка реквизитов |
| `Services/ConclusionSchemaService.cs` | Обмен JSON с процессом XML-сервиса. `NormalizeLegacyConclusionAsync` исправляет порядок элементов после сериализации классической формы |
| `Services/SchemaXmlDocument.cs` | Сохранение XML и идентификатора через XPO и подготовка выгрузки со схемой |
| `schema_engine/schema.py`, `migration.py`, `note_migration.py`, `preview.py` | Профили XSD, проверка, преобразование и HTML-предпросмотр |
| `portal/src/main.jsx`, `SchemaForm.jsx`, `xml.js` | Документ в браузере, отрисовка полей XSD, операции с XML и сохранение черновиков |

### Forms/ViewForm

`ViewForm` относится к классическому интерфейсу. Актуальные схемы используют штатные XSL в RichEdit. Поведение пользовательских команд описано в [руководстве](user-guide.md).

### Forms/ObjectChange/ConclusionEdit

Главная форма заключения. Ключевые члены:

| Член | Поведение |
|---|---|
| конструкторы | `()` — новое заключение (создаётся в `Load`); `(Conclusion)` — правка существующего |
| `ConclusionEdit_Load` | `FillFormControllersAsync` (списки из справочника), затем переносит значения модели в контролы. Коллекции модели копируются в приватные списки `_tDocuments`, `_experts` и т. д. |
| `btnSave_Click` | Собирает модель 01.03 из контролов и списков. Главное окно подключает сохранение с нормализацией порядка элементов и проверкой XML |
| `btnSearch_ButtonPressed` | DaData `FindParty` → реквизиты экспертной организации, ФИО и должность руководителя, адрес (D-04, D-05) |
| `btnAddress_ButtonPressed`, `cmbObjectAddress_ButtonClick` | «…» — `AddressEdit`, лупа — DaData Clean, «×» — очистка |
| `btnDeclarant_ButtonPressed` | `DeclarantEdit` |
| `barBtn*Add/Edit/Del_ItemClick` | Добавление, правка, удаление строк в списках через дочерние формы, перерисовка таблицы. Удаление — с подтверждением |
| `checkEEPDUse_CheckedChanged`, `radioGroupConclusionNumber_SelectedIndexChanged` | Видимость блока ЭЭПД и маска номера ЕГРЗ |
| `GetStringValue(value, control, message, isNullAble = true)` | Возвращает строку; если пусто и `isNullAble == false` — фокус на контрол и исключение с `message` (D-02) |
| `GetValueGeneralizedDictionary(sender, message, isNullAble = true)` | Достаёт `GeneralizedDictionaryXPO` из `ButtonEdit` или объекта; правило пустого значения то же (D-02) |
| `GetCheckComboBoxItems<T>(sender, message, isNullAble = true)` | Отмеченные пункты → список enum (D-02) |
| `GetEnumByAttributeValue<T>(code, isNullAble = true)` | Ищет значение enum по `XmlEnum`, вызывая `EnumExtension.GetEnumXmlAttributeValue` через рефлексию из повторно загруженного `Core.NET.dll`. Не нашёл — `default(T)`, то есть первое значение (D-02, D-03) |
| `GetMethodName(name)` | Загружает `Core.NET.dll` или `dll\Core.NET.dll` по относительному пути через `Assembly.LoadFile` и ищет метод-расширение для `Enum` (D-03) |
| `SetCheckComboBoxByItems<T>` | Отмечает в списке пункты, совпадающие с коллекцией enum |
| `SetFileName`, `Conclusion`, `FileName`, `IsSave` | Обмен данными с `MainForm` |

### Forms/ObjectChange/ExplanatoryNoteEdit

| Член | Поведение |
|---|---|
| конструктор | Заполняет список типов объекта описаниями `ExplanatoryNoteObjType` |
| `cmbExplanatoryNoteObjType_SelectedValueChanged` | Запоминает тип и показывает блок «Производственный объект» только для него |
| `ExplanatoryNoteEdit_Load` | Списки из справочника; перенос модели в контролы; производственный объект — полностью, остальные типы не загружаются (D-07) |
| `btnSave_Click` | Собирает модель; массивы **заменяются** целиком (дублей, как в D-01, нет). Для непроизводственного и линейного объекта записывается пустой объект (D-07). `ConstructionDuration` парсится как `int` (D-16) |
| `barBtn*` | Изменения, авторы, подписанты, застройщики, техзаказчики, нормы, документы — через дочерние формы. Застройщики не отображаются и не редактируются (D-08) |

### Дочерние формы `Forms/ObjectChange`

Все формы устроены одинаково: конструктор `()` для нового объекта и `(T)` для правки, свойства `IsSave` и объект-результат, `btnSave_Click` собирает объект, `btnCancel_Click` закрывает.

| Форма | Результат | Особенности |
|---|---|---|
| `AddressEdit` | `TAddress` | Поля адреса, регион из справочника |
| `PostAddressEdit` | `TPostAddress` | Почтовый адрес |
| `EngineeringSurveyAddressEdit` | `TEngineeringSurveyAddress` | Регион и район изысканий |
| `DeclarantEdit` | `TDeclarant` | ЮЛ / иностранное ЮЛ / ИП / ФЛ, поиск в DaData |
| `DesignerEdit` | `TDesigner` | ЮЛ / иностранное ЮЛ / ИП, DaData |
| `DeveloperEdit` | `tDeveloper` (записка) | ЮЛ / иностранное ЮЛ / ИП / ФЛ, DaData |
| `TechnicalCustomerEdit` / `TechnicalCustomerENEdit` | `TTechnicalCustomer` (заключение / записка) | ЮЛ или иностранное ЮЛ, DaData |
| `AuthorEdit` | `tAuthor` | Автор ПД, СРО, DaData |
| `FundingInformationEdit` | `TFinance` | Вид финансирования, уровень бюджета, доля, организация (DaData) |
| `ConclusionBaseSettingEdit` | `TOrganization` + `TWorkPerson` | Экспертная организация и утверждающий |
| `DocumentEdit` / `DocumentENEdit` | `TDocument` / `tDocument` | Тип по справочнику, наименование, номер, дата, примечание, автор документа, файл |
| `PreviousConclusionEdit` | `TPreviousConclusion` | Наименование, дата, вид объекта и результат ранее выданного заключения |
| `ObjPartEdit` | `TObjectPart` | Наименование, вид и функциональное назначение части объекта |
| `ExpertEdit` | `TExpert` | ФИО, номер и срок действия аттестата, направление деятельности |
| `ExpertProjectDocumentsEdit` | `TExpertProjectDocuments` | Направление, замечания (`TProjectDocumentsMismatches`), «опасные решения». Поля «описание решений» и «оперативные изменения» есть на форме, но не загружаются и не сохраняются (D-17) |
| `ExpertEngineeringSurveyEdit` | `TExpertEngineeringSurveys` | Вид изысканий и замечания. Поля программы, методов, условий и изменений закомментированы: в модели схемы 01.03 их нет |
| `EstimateMismatchEdit` | `TEstimateMismatches` | Замечания по смете |
| `WorkPersonEdit` | `tWorkPerson` | ФИО, должность, признаки реестров |
| `TextBlockEdit` | `tTextBlock` | Текст, таблицы (`TableEdit`), рисунки (`ImageEdit`) |
| `TableEdit`, `ImageEdit` | `tTable`, `tImage` | Таблица из строк и ячеек; рисунок из файла |

### Models — обёртки для таблиц

`DocumentObj`, `DocumentObjEn`, `FinanceObj`, `ExpertProjectDocument`, `ExpertEngineeringSurvey`, `EngineeringSurveyAddress` оборачивают элементы модели для отображения. Подпись из справочника каждая обёртка получает в `async void SetEnumObj()` — отдельным запросом на строку (D-18). `PersonObj` определяет отображаемое имя для заявителя, проектировщика, застройщика и техзаказчика:

| Метод | Обрабатывает |
|---|---|
| `SetObj()` | `TDeclarant`, оба `TTechnicalCustomer` |
| `SetObjByDeclarator()` | `TDeclarant`; используется и для `tDeveloper`, который он не распознаёт (D-08) |
| `SetObjByDesigner()` | `TDesigner` |
| `SetObjByTechnicalCustomer()` | оба `TTechnicalCustomer` |
