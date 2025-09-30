namespace ResumeGenerator.Data.Interfaces
{
    public interface IJWTTokenCreator
    {
        string CreateToken(string email);
    }
}
