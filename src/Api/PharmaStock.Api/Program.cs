using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Validation;
using PharmaStock.Modules.Product.Presentation;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuditUserAccessor();
builder.Services.AddAuditableEntityInterceptor();

builder.Services.AddMediator(options =>
{
    options.Assemblies = [typeof(CreateProductCommand).Assembly];
    options.PipelineBehaviors = [typeof(FluentValidationMediatorPipelineBehavior<,>)];
});

builder.Services.AddProductModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.MapEndpointsFromAssembly(typeof(ProductModuleServiceCollectionExtensions).Assembly);



app.Run();
