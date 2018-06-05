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
