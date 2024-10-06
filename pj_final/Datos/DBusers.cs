using pj_final.Models;
using Npgsql; 

namespace pj_final.Datos
{
    public class DBusers
    {
        private static string CadenaSQL = "server=localhost;username=postgres;database=sexshop;password=23042004";

        public static bool registrar(users usuario)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "insert into users(full_name,email,password,restablecer,confirmado,token)";
                    query += " values(@full_name,@email,@password,@restablecer,@confirmado,@token)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@full_name", usuario.full_name);
                    cmd.Parameters.AddWithValue("@email", usuario.email);
                    cmd.Parameters.AddWithValue("@password", usuario.password);
                    cmd.Parameters.AddWithValue("@restablecer", usuario.restablecer);
                    cmd.Parameters.AddWithValue("@confirmado", usuario.confirmado);
                    cmd.Parameters.AddWithValue("@token", usuario.token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;


                }

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static users validar(string email, string password)
        {
            users usuario = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select full_name,restablecer,confirmado from users";
                    query += " where email= @email and password = @password";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new users()
                            {
                                full_name = dr["full_name"].ToString(),
                                restablecer = (bool)dr["restablecer"],
                                confirmado = (bool)dr["confirmado"]
                            };
                        }
                    }


                }


            }
            catch (Exception)
            {

                throw;
            }

            return usuario;
        }
        public static users obtener(string email)
        {
            users usuario = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select full_name,password,restablecer,confirmado,token from users";
                    query += " where email= @email";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new users()
                            {
                                full_name = dr["full_name"].ToString(),
                                password = dr["password"].ToString(),
                                token = dr["token"].ToString(),
                                restablecer = (bool)dr["restablecer"],
                                confirmado = (bool)dr["confirmado"]
                            };
                        }
                    }


                }


            }
            catch (Exception)
            {

                throw;
            }

            return usuario;
        }
        public static bool restablecerActualizar(int restablecer, string password, string token)
        {
			bool respuesta = DBusers.restablecerActualizar(1, password, token);
			try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"update users set " +
                        "restablecer = @restablecer, " +
                        "password = @password " +
                        "where token=@token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@restablecer", restablecer);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;


                }

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool confirmar(string token)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"update users set " +
                        "confirmado = 1, " +
                        "where token=@token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;


                }

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
