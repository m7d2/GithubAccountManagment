namespace Info
{
    public class Account
    {
        public string? UserName { get; private set; }
        public string? Email { get; private set; }
        public string? SSHFilePath { get; private set; }

        public Account(string Username, string Useremail, string SshFilePath)
        {
            if (string.IsNullOrWhiteSpace(Username)) throw new ArgumentException("Username can't be empty");
            if (string.IsNullOrWhiteSpace(Useremail)) throw new ArgumentException("Email can't be empty");
            if (string.IsNullOrWhiteSpace(SshFilePath)) throw new ArgumentException("The file path to SSH can't be empty");

            UserName = Username;
            Email = Useremail;
            SSHFilePath = SshFilePath;
        }
    }
}
