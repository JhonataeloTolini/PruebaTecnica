using Oracle.ManagedDataAccess.Client;
using PruebaTecnica.Interface;
using PruebaTecnica.Models;
using System.Data;

namespace PruebaTecnica.Repository
{
    public class MovimientoRepoAsync: IMovimientoRepoAsync
    {

        private readonly string _connection;

        public MovimientoRepoAsync(IConfiguration config)
        {

            _connection = config.GetConnectionString("OracleConnection");


        }

        public async Task<List<Movimiento>> ListMovimiento(string fechaInicio, string fechaFin, string tipoMovimiento, string NroDoc)
        {

            using var connection = new OracleConnection(_connection);
            List<Movimiento> rpta = new List<Movimiento>();

            using (connection)
            using (OracleCommand command = new OracleCommand("PKG_MOVIMIENTO.SP_MOVIMIENTO_LISTAR", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("PI_FECINI", OracleDbType.Varchar2, 10).Value = fechaInicio;
                command.Parameters.Add("PI_FECFIN", OracleDbType.Varchar2, 10).Value = fechaFin;
                command.Parameters.Add("PI_TIPOMOV", OracleDbType.Varchar2, 100).Value = tipoMovimiento;
                command.Parameters.Add("PI_NRODOC", OracleDbType.Varchar2, 100).Value = NroDoc;
                command.Parameters.Add("PI_NUM_PAGINA", OracleDbType.Int64).Value = 0;
                command.Parameters.Add("PI_TAMANO_PAGINA", OracleDbType.Int64).Value = 100;
                command.Parameters.Add(new OracleParameter("PO_TOTAL_REGISTROS", OracleDbType.Int64) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new OracleParameter("PO_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });
                command.Parameters.Add("PO_MENSAJE_RESPUESTA", OracleDbType.Varchar2, 3000);
                command.Parameters["PO_MENSAJE_RESPUESTA"].Direction = ParameterDirection.Output;
                command.Parameters.Add("PO_CODIGO_RESPUESTA", OracleDbType.Varchar2, 50);
                command.Parameters["PO_CODIGO_RESPUESTA"].Direction = ParameterDirection.Output;

                connection.Open();

                OracleDataReader reader = command.ExecuteReader();

                try
                {



                    while (reader.Read())
                    {


                        rpta.Add(new Movimiento()
                        {
                            codCIA = reader.IsDBNull(0) ? "" : reader.GetString(0),
                            companiaVenta = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            almacenVenta = reader.IsDBNull(2) ? "" : reader.GetString(2),

                        });



                    }
                }
                finally
                {
                    // always call Close when done reading.
                    reader.Close();
                }

                //var codRpta = command.Parameters["PO_CODIGO_RESPUESTA"].Value.ToString();

                //if (codRpta != "200")
                //{

                //    if (codRpta == "300")
                //    {

                //        objReturn.errorCodeHttp = "01";

                //    }

                //    objReturn.error.Add(command.Parameters["PO_MENSAJE_RESPUESTA"].Value.ToString());

                //}

                return rpta;


            }


        }

        public async Task<bool> RegistrarMovimiento(CabMovimiento modelo)
        {
            try
            {

                using (OracleConnection conn = new OracleConnection(_connection))
                {
                    using (OracleCommand cmd = new OracleCommand($"PKG_MOVIMIENTO.SP_MOVIMIENTO_CREAR", conn))
                    {
                        conn.Open();
                        using (OracleTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = trans;
                            cmd.Parameters.Add(new OracleParameter("PI_TIPOMOV", OracleDbType.Varchar2, ParameterDirection.Input) { Value = modelo.tipoMov });
                            cmd.Parameters.Add(new OracleParameter("PI_NRODOC", OracleDbType.Varchar2, ParameterDirection.Input) { Value = modelo.NroDoc });
                            cmd.Parameters.Add(new OracleParameter("PO_MENSAJE_RESPUESTA", OracleDbType.Varchar2, ParameterDirection.Output) { Size = 3000 });
                            cmd.Parameters.Add(new OracleParameter("PO_CODIGO_RESPUESTA", OracleDbType.Varchar2, ParameterDirection.Output) { Size = 50 });
                            int affectedRows = cmd.ExecuteNonQuery();

                            trans.Commit();

                            return cmd.Parameters["PO_CODIGO_RESPUESTA"].Value.ToString().Equals("200");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
                return false;
            };
        }

    }
}
