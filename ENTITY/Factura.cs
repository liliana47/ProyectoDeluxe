using ENTITY;
using System.Collections.Generic;
using System;

public class Factura
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public string LugarEmision { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime HoraEmision { get; set; }
    public List<Producto> Productos { get; set; }
    public double Total { get; set; }

    // Constructor definido
    public Factura()
    {

    }

    public Factura(Cliente cliente)
    {
        Cliente = cliente;
        FechaEmision = DateTime.Now;
        HoraEmision = DateTime.Now;
        Productos = new List<Producto>();
        Total = 0.0;
    }
}
