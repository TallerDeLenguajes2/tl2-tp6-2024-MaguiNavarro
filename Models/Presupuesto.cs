using System;
using System.Collections.Generic;


public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private List <PresupuestosDetalle> detalle;
       DateTime fechaCreacion;
 
   public Presupuesto(){
       
       fechaCreacion = DateTime.Now;
       detalle= new List<PresupuestosDetalle>();

   }
    public List<PresupuestosDetalle> Detalle { get => detalle;   }
    public string NombreDestinatario { get => nombreDestinatario; set=>nombreDestinatario=value ; }
    public int IdPresupuesto { get => idPresupuesto; set=>idPresupuesto =value;   }
     public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }


    public int MontoPresupuesto( ){
       int total= 0;
       foreach (PresupuestosDetalle pd in detalle)
       {
          total +=  pd.Prod.Precio * pd.Cantidad ;
       }

       return total;

    }
    public double MontoPresupuestoConIva(){
         int cantSinIva = MontoPresupuesto();
        return cantSinIva * 1.21;
    }
      public int CantidadProductos()
    {
        return Detalle.Sum(d => d.Cantidad);
    }
   
     public void añadirDetalle(PresupuestosDetalle detalle)
    {
        Detalle.Add(detalle);
    }
     public void añadirDetalle(List<PresupuestosDetalle> detalles)
    {
        foreach(var det in detalles)
        {
            añadirDetalle(det);
        }
    }
}
