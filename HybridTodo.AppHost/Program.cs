using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

var hybridTodoApi = builder.AddProject<Projects.HybridTodo_Api>("hybridtodo-api");

builder.AddProject<Projects.HybridTodo_Web>("hybridtodo-web")
    .WithExternalHttpEndpoints()
    .WithReference(hybridTodoApi)
    .WaitFor(hybridTodoApi);


builder.Build().Run();
