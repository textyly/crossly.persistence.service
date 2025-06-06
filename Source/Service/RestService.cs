using Persistence.Handler;

namespace Persistence.Service
{
   public class RestService(IRequestHandler requestHandler)
   {
      public void RegisterMethods(WebApplication app)
      {
         var group = app.MapGroup("/api/v1/patterns");

         group.MapGet(string.Empty, requestHandler.GetAll)
            .WithName("GetAllPatterns");

         group.MapGet("{id}", requestHandler.GetById)
            .WithName("GetPatternById");

         group.MapPost(string.Empty, (Delegate)requestHandler.Create)
            .WithName("CreatePattern");

         group.MapPut("{id}", requestHandler.Replace)
            .WithName("ReplacePattern");

         group.MapPatch("{id}/rename", requestHandler.Rename)
            .WithName("RenamePattern");

         group.MapDelete("{id}", requestHandler.Delete)
            .WithName("DeletePattern");
      }
   }
}