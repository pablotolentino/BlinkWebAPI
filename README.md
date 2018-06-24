# BlinkWebAPI
Referencia para instalacion del SDK .NET CORE y de .NET Framework
<br/>
https://www.microsoft.com/net/download/windows


https://www.danysoft.com/free/csharp2.pdf

Scaffold-DbContext "Server=localhost;Database=BLINK;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities

Uninstall-Package Microsoft.EntityFrameworkCore.SqlServer<br/>
Uninstall-Package Microsoft.EntityFrameworkCore.Design<br/>
Uninstall-Package Microsoft.EntityFrameworkCore.Tools<br/>
Uninstall-Package Microsoft.EntityFrameworkCore.SqlServer.Design<br/>
<br/>
Install-Package Microsoft.EntityFrameworkCore.SqlServer<br/>
Install-Package Microsoft.EntityFrameworkCore.Design<br/>
Install-Package Microsoft.EntityFrameworkCore.Tools<br/>
Install-Package Microsoft.EntityFrameworkCore.SqlServer.Design<br/>

Instalar automaper

Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection -Version 4.0.0

Install-Package AutoMapper -Version 7.0.0

installar swagger 
https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml

Install-Package Swashbuckle.AspNetCore

public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<TodoContext>(opt =>
        opt.UseInMemoryDatabase("TodoList"));
    services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    // Register the Swagger generator, defining 1 or more Swagger documents
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
    });
}

using Swashbuckle.AspNetCore.Swagger;


public void Configure(IApplicationBuilder app)
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.UseMvc();
}
