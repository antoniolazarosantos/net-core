﻿using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action; 

    public static IResult Action(ApplicationDbContext context)
    {
        var c = context.Categories.ToList();
        var r = c.Select(x => new CategoryResponse { Id = x.Id, Name = x.Name, Active = x.Active});
        return Results.Ok(r);

    }
}
