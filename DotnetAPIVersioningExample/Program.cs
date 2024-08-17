using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;//This option helps to for activating default api version, if API version is unspecified
    options.DefaultApiVersion = new ApiVersion(1);//If client doesn't select any version, API work on this version
    options.ReportApiVersions = true;//You can return supported version thanks to this.
    options.ApiVersionReader = ApiVersionReader.Combine(//We are setting API Version detection types below. 
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-API-VERSION"),
        new MediaTypeApiVersionReader("vdf"));
}).AddApiExplorer(expoptions =>
{
    expoptions.GroupNameFormat = "'v'V";
    expoptions.SubstituteApiVersionInUrl = true;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(
//            options =>
//            {
//                // resolve the IApiVersionDescriptionProvider service
//                // note: that we have to build a temporary service provider here because one has not been created yet
//                var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
//                // add a swagger document for each discovered API version
//                // note: you might choose to skip or document deprecated API versions differently
//                foreach (var description in provider.ApiVersionDescriptions)
//                {
//                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
//                }
//                // add a custom operation filter which sets default values
//                options.OperationFilter<SwaggerDefaultValues>();
//                // integrate xml comments
//                options.IncludeXmlComments(XmlCommentsFilePath);
//            });


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(options =>
    {

        options.DocumentTitle = "Test Title";
        options.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
