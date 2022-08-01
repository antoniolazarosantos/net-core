using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action; 

    public static IResult Action([FromRoute] Guid Id,CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var c = context.Categories.Where(g => g.Id == Id).FirstOrDefault();
        c.Name = categoryRequest.Name;
        c.Active = categoryRequest.Active;
        context.SaveChanges();
        return Results.Ok();

    }
}
