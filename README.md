appsettings.json not included for security reasons, use the template below for reference,

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=ServerNamePlaceholder;Database=DatabaseNamePlaceholder;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JWT": {
    "Key": "YourJWTKeyPlaceholder",
    "Issuer": "IssuerPlaceholder",
    "Audience": "AudiencePlaceholder",
    "Subject": "SubjectPlaceholder"
  },
  "EmailSettings": {
    "FromEmail": "EmailPlaceholder",
    "SmtpHost": "SmtpPlaceholder",
    "SmtpPort": "SmtpPortPlaceholder", 
    "Username": "UsernamePlaceholder",
    "Password": "PasswordPlaceholder"
  }
}
