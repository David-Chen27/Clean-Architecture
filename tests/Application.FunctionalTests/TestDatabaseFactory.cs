namespace Clean_Architecture.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        // 使用docker建立容器測試資料庫
        var database = new TestcontainersTestDatabase();
        
        // 使用本機測試資料庫
        //var database = new SqlServerTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
