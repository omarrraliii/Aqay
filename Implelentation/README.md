Sure, here's the content converted into Markdown format with the necessary instructions:

---

## Setting Up and Running the Project Locally

To run the project locally, follow these steps:

1. **Update Database**: 
    - Open Package Manager Console (PMC) in Visual Studio.
    - Run the command: `Update-Database`.

2. **Change Connection String**:
    - Open the `appsettings.json` file.
    - Update the `ConnectionStrings` section with your local database connection string.

3. **Create API Application**:
    - Open Visual Studio.
    - Create a new project.
    - Choose "ASP.NET Core Web Application".
    - Name the application "aqay_apis".
    - Select ASP.NET Core 6 as the target framework.
    - Choose "API" template.
    - Select "No Authentication".
    - Uncheck the option for configuring HTTPS.

4. **Install Packages**:
   - Install the following packages from NuGet Package Manager:
     ```powershell
     Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 6.0.0
     Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore.Design -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore.Tools -Version 6.0.0
     Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 6.0.0
     Install-Package System.IdentityModel.Tokens.Jwt -Version 6.0.0
     ```

   - Alternatively, you can install these packages using Package Manager Console:
     ```powershell
     Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 6.0.0
     Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore.Design -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.0
     Install-Package Microsoft.EntityFrameworkCore.Tools -Version 6.0.0
     Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 6.0.0
     Install-Package System.IdentityModel.Tokens.Jwt -Version 6.0.0
     ```

5. **Generate Key**:
   - Go to [8gwifi.org/jwsgen.jsp](https://8gwifi.org/jwsgen.jsp).
   - Generate a key with algorithm HS256.
   - Copy the generated key. You'll need it in the `appsettings.json`.

6. **Configure `appsettings.json`**:
   - Open `appsettings.json`.
   - Add the generated key to the `appsettings.json` under the appropriate section.

7. **Configure CORS**:
   - In the `ConfigureServices` method of `Startup.cs`, add and use CORS:
     ```csharp
     builder.Services.AddCors();
     ```

   - In the `Configure` method of `Startup.cs`, add the following line:
     ```csharp
     app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
     ```

---

These steps should guide you through setting up and running the project locally in Visual Studio. Let me know if you need further assistance!
