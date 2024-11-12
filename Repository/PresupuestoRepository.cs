using Microsoft.Data.Sqlite;

public class PresupuestoRepository
{
    private const string cadenaConexion = "Data source=db/Tienda.db;Cache=Shared";

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
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

        string query = @"SELECT 
            idPresupuesto,
            NombreDestinatario,
            FechaCreacion
        FROM 
            Presupuestos;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto=  Convert.ToInt32(reader["idPresupuesto"]);
                     presupuesto.NombreDestinatario =  reader["NombreDestinatario"].ToString();
                     presupuesto.FechaCreacion= Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }
     // Obtener detalles de un Presupuesto por su ID
    public Presupuesto GetPresupuesto(int id)
    {
        var presupuesto = new Presupuesto();
        var querystring = @" SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, 
        pd.idProducto, pd.Cantidad
        FROM Presupuestos p
        LEFT JOIN PresupuestosDetalle pd ON p.IdPresupuesto = pd.idPresupuesto
        WHERE p.IdPresupuesto = @IdPresupuesto";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(querystring, connection);
            command.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));

            using (var reader = command.ExecuteReader())
            {
                while(reader.Read())
                { 
                     var detalle = new PresupuestosDetalle();
                    detalle.asignarProd(Convert.ToInt32(reader["idProducto"]));
                    detalle.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    presupuesto.añadirDetalle(detalle);
                }
            }
            connection.Close();
        }
        return presupuesto;
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
        var querystring = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";

        using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();

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
