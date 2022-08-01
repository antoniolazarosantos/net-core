using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action; 

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var c = new Category
        {
            Name = categoryRequest.Name,
            CreatedBy = "Test",
            CreatedOn = "Test",
            EditedBy = DateTime.Now,
            EditedOn = DateTime.Now

        };
        context.Categories.Add(c);
        context.SaveChanges();
        return Results.Created($"/categories/{c.Id}",c.Id);

    }
}
