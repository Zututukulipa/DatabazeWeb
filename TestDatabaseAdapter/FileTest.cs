using DatabaseAdapter.OracleLib;

namespace TestDatabaseAdapter
{
    public class FileTest
    {
        private OracleDatabaseControls Controls { get; } =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");
    }
}