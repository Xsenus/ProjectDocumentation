using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace Core.NET.Controllers
{
    public static class SessionController
    {
        private static string _nameDataBase = "xml.s3db";
        private static string _connectionString => SQLiteConnectionProvider.GetConnectionString(_nameDataBase);
        private static string _connectionPoolString => XpoDefault.GetConnectionPoolString(_connectionString);
        private static IDataStore _iDataStore => XpoDefault.GetConnectionProvider(_connectionPoolString, AutoCreateOption.DatabaseAndSchema);
        private static XPDictionary _xPDictionary = new ReflectionDictionary();

        private static Session _session;

        public static Session GetSession()
        {
            if (_session is null || _session.IsConnected is false)
            {
                CreateDataBase();

                var threadSafeDataLayer = new ThreadSafeDataLayer(_xPDictionary, _iDataStore);
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
            var simpleDataLayer = new SimpleDataLayer(_xPDictionary, _iDataStore);
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
