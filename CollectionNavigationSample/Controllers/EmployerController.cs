using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectionNavigationSample.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollectionNavigationSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployerController : ControllerBase
    {

        private readonly ILogger<EmployerController> _logger;
        private readonly AppDbContext _dbContext;

        public EmployerController(ILogger<EmployerController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /*
          Does not work: http://localhost:5000/employer?$expand=employees($expand=department;$top=100)&$top=100

          Error: Msg 207, Level 16, State 1, Line 15
           Invalid column name 'DepartmentId'.
           Msg 207, Level 16, State 1, Line 8
           Invalid column name 'DepartmentId'.
           Msg 207, Level 16, State 1, Line 8
           Invalid column name 'EmployerId'.

          exec sp_executesql N'SELECT [t].[Id], [t].[Name], [t1].[c], [t1].[Id], [t1].[DepartmentId], [t1].[EmployerId], [t1].[Name], [t1].[c0], [t1].[c1], [t1].[c2], [t1].[Id0], [t1].[Name0], [t1].[c3]
            FROM (
                SELECT TOP(@__TypedProperty_4) [e].[Id], [e].[Name]
                FROM [Employer] AS [e]
                ORDER BY [e].[Id]
            ) AS [t]
            OUTER APPLY (
                SELECT @__TypedProperty_2 AS [c], [t].[Id], [t].[DepartmentId], [t].[EmployerId], [t].[Name], CAST(1 AS bit) AS [c0], N''Department'' AS [c1], @__TypedProperty_3 AS [c2], [d].[Id] AS [Id0], [d].[Name] AS [Name0], CAST(0 AS bit) AS [c3]
                FROM (
                    SELECT TOP(@__TypedProperty_1) [e0].[Id], [e0].[DepartmentId], [e0].[EmployerId], [e0].[Name]
                    FROM [Employee] AS [e0]
                    WHERE [t].[Id] = [e0].[EmployerId]
                    ORDER BY [e0].[Id]
                ) AS [t0]
                INNER JOIN [Department] AS [d] ON [t].[DepartmentId] = [d].[Id]
            ) AS [t1]
            ORDER BY [t].[Id], [t1].[Id], [t1].[Id0]',N'@__TypedProperty_4 int,@__TypedProperty_2 nvarchar(4000),@__TypedProperty_3 nvarchar(4000),@__TypedProperty_1 int',@__TypedProperty_4=100,@__TypedProperty_2=N'e981a185-886b-4d22-9614-151280e31234',@__TypedProperty_3=N'e981a185-886b-4d22-9614-151280e31234',@__TypedProperty_1=100
         */
        [HttpGet]
        [EnableQuery]
        public IQueryable<Employer> Get()
        {
            return _dbContext.Employer;
        }

    }
}
