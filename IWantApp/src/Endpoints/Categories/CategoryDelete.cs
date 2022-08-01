using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryDelete
{
    public static string Template => "/categories/{id}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action; 

    public static IResult Action([FromRoute] Guid Id,ApplicationDbContext context)
    {
        var c = context.Categories.Where(g => g.Id == Id).First();
        context.Remove(c);
        context.SaveChanges();
        return Results.Ok();

    }
}
