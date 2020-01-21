# CollectionNavigationSample
Demonstrates expansion with top issue in DotNet Core 3.1 with OData 7.3. 

See EmployerController.cs for controller setup.

## Usage

 1. Create CollectionNavigationSample in LocalDB
 2. Apply migration to setup database: `dotnet ef database update`
 3. Run server in VS or `dotnet watch run`
 4. Open in browser `http://localhost:5000/employer?$expand=employees($expand=department;$top=100)&$top=100`

## Output

### Generated SQL
```sql
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
```

### Stacktrace
```csharp
Microsoft.Data.SqlClient.SqlException (0x80131904): Invalid column name 'DepartmentId'.
Invalid column name 'DepartmentId'.
Invalid column name 'EmployerId'.
   at Microsoft.Data.SqlClient.SqlCommand.<>c.<ExecuteDbDataReaderAsync>b__164_0(Task`1 result)
   at System.Threading.Tasks.ContinuationResultTaskFromResultTask`2.InnerInvoke()
   at System.Threading.Tasks.Task.<>c.<.cctor>b__274_0(Object obj)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
--- End of stack trace from previous location where exception was thrown ---
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Threading.Tasks.Task.ExecuteWithThreadLocal(Task& currentTaskSlot, Thread threadPoolThread)
--- End of stack trace from previous location where exception was thrown ---
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryingEnumerable`1.AsyncEnumerator.InitializeReaderAsync(DbContext _, Boolean result, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryingEnumerable`1.AsyncEnumerator.MoveNextAsync()
   at Microsoft.AspNetCore.Mvc.Infrastructure.AsyncEnumerableReader.ReadInternal[T](IAsyncEnumerable`1 value)
   at Microsoft.AspNetCore.Mvc.Infrastructure.AsyncEnumerableReader.ReadInternal[T](IAsyncEnumerable`1 value)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor.ExecuteAsyncEnumerable(ActionContext context, ObjectResult result, IAsyncEnumerable`1 asyncEnumerable)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResultFilterAsync>g__Awaited|29_0[TFilter,TFilterAsync](ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResultExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.ResultNext[TFilter,TFilterAsync](State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeResultFilters>g__Awaited|27_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|19_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
   at Microsoft.AspNetCore.Builder.RouterMiddleware.Invoke(HttpContext httpContext)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)
ClientConnectionId:8fc06ccd-60d2-4c94-8ee5-05ba2167728f
Error Number:207,State:1,Class:16
```