namespace SpudPI.API
{
    public class PasswordInitialization : IHostedService
    {
        private readonly ILogger<PasswordInitialization> _logger;
        private readonly IPasswordService _passwordService;


        public PasswordInitialization(IPasswordService passwordService, ILogger<PasswordInitialization> logger)
        {
            _passwordService = passwordService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            string filePath = "hashedPassword.bin";
            var passwordHash = ReadHashedPasswordFromBinaryFile(filePath);

            while (string.IsNullOrEmpty(passwordHash))
            {
                _logger.LogInformation("Please enter a super secret Password:");
                var password = Console.ReadLine();
                var passwordErrors = GetPasswordErrors(password);

                if (passwordErrors.Length != 0)
                {
                    foreach (var error in passwordErrors)
                    {
                        _logger.LogError(error);
                    }
                }
                else
                {
                    passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    WriteHashedPasswordToBinaryFile(passwordHash, filePath);
                }

            }

            _passwordService.HashedPassword = passwordHash;
            return Task.CompletedTask;
        }

        private string[] GetPasswordErrors(string? password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                errors.Add("Your password must be at least 6 characters long");
            }
            else if (password.Length > 100)
            {
                errors.Add("The password must not be longer than 100 characters");
            }

            return errors.ToArray();
        }

        private string? ReadHashedPasswordFromBinaryFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using var binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open));
            return binaryReader.ReadString();
        }

        private void WriteHashedPasswordToBinaryFile(string hashedPassword, string filePath)
        {
            using var binaryWriter = new BinaryWriter(File.Open(filePath, FileMode.Create));
            binaryWriter.Write(hashedPassword);
        }



        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
