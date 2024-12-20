using Microsoft.Data.Sqlite;
using System.ComponentModel;
using SQLitePCL;

public class PresupuestoRepository
{
    private const string cadenaConexion = "Data source=DB/Tienda.db;Cache=Shared";

    public void create(Presupuesto presupuesto)
    {
        var querystring = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion)";

        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(querystring, connection);

            command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
             command.Parameters.Add(new SqliteParameter("@FechaCreacion", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))); // Ajusta la fecha como corresponda

            command.ExecuteNonQuery();

            connection.Close();
        }

    }
     public List<Presupuesto> listarPresupuestos()
    {
        var querystring = "SELECT * FROM Presupuestos";
        var listaPresupuestos = new List<Presupuesto>();
 
        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(querystring, connection);

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var presupuesto = new Presupuesto();
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.FechaCreacion =  Convert.ToDateTime(reader["FechaCreacion"]);
                    listaPresupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }

        return listaPresupuestos;
    }
     public List<Presupuesto> GetPresupuestos()
    {
       

        string query = @"SELECT * FROM  Presupuestos;";
         List<Presupuesto> listaPresupuestos = new List<Presupuesto>();
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var presupuesto = new Presupuesto();

                    presupuesto.IdPresupuesto=  Convert.ToInt32(reader["idPresupuesto"]);
                     presupuesto.NombreDestinatario =  reader["NombreDestinatario"].ToString();
                     presupuesto.FechaCreacion= Convert.ToDateTime(reader["FechaCreacion"]);
                         presupuesto.añadirDetalle(GetDetalles(presupuesto.IdPresupuesto));
                    listaPresupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return listaPresupuestos;
    }
       public List<PresupuestosDetalle> GetDetalles(int id)
    {
        var querystring = "SELECT * FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto";
        var detalles = new List<PresupuestosDetalle>();

        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(querystring, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var detalle = new PresupuestosDetalle();
                    detalle.asignarProd(Convert.ToInt32(reader["idProducto"]));
                    detalle.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    detalles.Add(detalle);
                }
            }
        }
        return detalles;
    }
     // Obtener detalles de un Presupuesto por su ID
     public Presupuesto GetPresupuesto(int id)
    {
        string querystring = "SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
        var presupuesto = new Presupuesto();

        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(querystring, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));

            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString());
                    presupuesto.añadirDetalle(GetDetalles(presupuesto.IdPresupuesto));
                }
            }
            connection.Close();
        }
        return presupuesto;
    }
        public void agregarDetalle(PresupuestosDetalle detalle, int idPresupuesto)
    {
        var querystring = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";

        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(querystring, connection);

            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@idProducto", detalle.Prod.IdProducto));
            command.Parameters.Add(new SqliteParameter("@Cantidad", detalle.Cantidad));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
      public void agregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        var querystring = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";

        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(querystring, connection);

            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
      public void delete(int id)
    {

         string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @Id";
                 var querystring = @"DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
         connection.Open();
        //Primero se eliminan los detalles
        SqliteCommand commandDetalle = new SqliteCommand(query2, connection);
        commandDetalle.Parameters.AddWithValue("@Id", id);
        commandDetalle.ExecuteNonQuery();
        // segundo el presupuestos
        SqliteCommand command = new SqliteCommand(querystring, connection);
        command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
        command.ExecuteNonQuery();

        connection.Close();

        }
    }

       public void modificar(Presupuesto presupuesto)
    {
        string querystring = "UPDATE Presupuestos SET NombreDestinatario = @destinatario, FechaCreacion = @fecha WHERE idPresupuesto = @id";
        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(querystring, connection);

            command.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
            command.Parameters.Add(new SqliteParameter("@destinatario", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

}    
