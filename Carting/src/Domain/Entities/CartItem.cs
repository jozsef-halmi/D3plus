﻿namespace Carting.Domain.Entities;

public class CartItem
{
    // Points to an Id in an external system
    public int Id { get; set; }

    public string Name { get; set; }

    public WebImage? WebImage { get; set; }

    public int Price { get; set; }

    public int Quantity { get; set; }
}
