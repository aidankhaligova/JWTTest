namespace JWTTest.Authentication
{
    public interface IJWTAuth
    {
        string GenerateToken();
        bool ValidatedToken(string incomingToken);
    }
}
