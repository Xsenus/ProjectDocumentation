using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.IO;

namespace Core.NET.Controllers
{
    public static class SessionController
    {
        static readonly string _nameDataBase = "xml.s3db";

        /// <summary>
        /// База лежит рядом с программой, а не в текущей рабочей папке процесса:
        /// иначе запуск из другого каталога создавал бы новую пустую базу.
        /// </summary>
        private static string DataBasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _nameDataBase);
        private static string ConnectionString => SQLiteConnectionProvider.GetConnectionString(DataBasePath);
        private static string ConnectionPoolString => XpoDefault.GetConnectionPoolString(ConnectionString);
        private static IDataStore IDataStore => XpoDefault.GetConnectionProvider(ConnectionPoolString, AutoCreateOption.DatabaseAndSchema);
        private static readonly XPDictionary _xPDictionary = new ReflectionDictionary();

        private static Session _session;

        public static Session GetSession()
        {
            if (_session is null || _session.IsConnected is false)
            {
                CreateDataBase();

                var threadSafeDataLayer = new ThreadSafeDataLayer(_xPDictionary, IDataStore);
                _session = new Session(threadSafeDataLayer, null)
                {
                    LockingOption = LockingOption.None,
                    TrackPropertiesModifications = true
                };
            }

            return _session;
        }

        /// <summary>
        /// Создаёт базу и обновляет схему синхронно: первые запросы приложения
        /// не должны выполняться раньше, чем схема готова.
        /// </summary>
        private static void CreateDataBase()
        {
            var simpleDataLayer = new SimpleDataLayer(_xPDictionary, IDataStore);
            using (var session = new Session(simpleDataLayer, null)
            {
                LockingOption = LockingOption.None,
                TrackPropertiesModifications = true
            })
            {
                session.CreateObjectTypeRecords();
                session.UpdateSchema();
            }
        }
    }
}
