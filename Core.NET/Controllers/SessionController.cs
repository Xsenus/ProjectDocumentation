using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace Core.NET.Controllers
{
    public static class SessionController
    {
        static readonly string _nameDataBase = "xml.s3db";
        private static string ConnectionString => SQLiteConnectionProvider.GetConnectionString(_nameDataBase);
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

        private async static void CreateDataBase()
        {
            var simpleDataLayer = new SimpleDataLayer(_xPDictionary, IDataStore);
            using (var session = new Session(simpleDataLayer, null)
            {
                LockingOption = LockingOption.None,
                TrackPropertiesModifications = true
            })
            {
                await session.CreateObjectTypeRecordsAsync();
                await session.UpdateSchemaAsync();
            }
        }
    }
}
