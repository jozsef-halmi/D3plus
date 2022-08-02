﻿namespace Catalog.GraphQL.GraphQL.Types;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Uri? ImageUrl { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public decimal Price { get; set; }

    public int Amount { get; set; }
}
