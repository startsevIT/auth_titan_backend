namespace auth_titan_backend.Data
{
	public class DbInitializer
	{
		public static void Initialize(AuthDbContext context)
		{ 
			context.Database.EnsureCreated();
		}
	}
}
