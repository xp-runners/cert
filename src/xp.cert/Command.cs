namespace Xp.Cert
{
    public interface Command
    {
        /// <summary>Execute this command</summary>
        int Execute(string[] args);
    }
}