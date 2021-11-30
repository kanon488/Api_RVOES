using API_RVOES.Models.StoredProcedures.AreasOpinion;
using API_RVOES.Models.StoredProcedures.EstadoAsignacion;
using API_RVOES.Models.StoredProcedures.EstadoOpinion;
using API_RVOES.Models.StoredProcedures.Municipios;
using API_RVOES.Models.StoredProcedures.NivelEducativo;
using API_RVOES.Models.StoredProcedures.RVOE;
using API_RVOES.Models.StoredProcedures.Solicitudes;
using API_RVOES.Models.StoredProcedures.Usuarios;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.Data
{
    public class RVOERepository
    {
        private readonly string _connectionString;

        public RVOERepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RVOES");
        }

        public async Task<List<SolicitudesViewModel>> GetAllSolicitudes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetAllSolicitudes", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<SolicitudesViewModel> result = new List<SolicitudesViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSolicitudes(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<SolicitudViewModel> GetSolicitud(int idSolicitud)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetSolicitud", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolicitud", idSolicitud));
                        SolicitudViewModel result = new SolicitudViewModel();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = _MapSolicitud(reader);
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SolicitudesViewModel>> GetSolicitudesNivel(int IdNivelEd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetSolicitudesXNivel", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdNivelEd", IdNivelEd));
                        List<SolicitudesViewModel> result = new List<SolicitudesViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSolicitudes(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<DetalleSolicitudViewModel> ObtDetalleSolicitud(int idSolicitud)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtDetalleSolicitud", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdSolicitud", idSolicitud));
                    DetalleSolicitudViewModel result = new DetalleSolicitudViewModel();
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result = _MapDetalleSolicitud(reader);
                        }
                    }
                    return result;
                }
            }
        }

        public async Task<List<UsuarioViewModel>> GetAllUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetAllUsers", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<UsuarioViewModel> result = new List<UsuarioViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapUsuario(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> CreateUsuario(CrearViewModel usuario, string sPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_CreateUsuario", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClaveUsuario", usuario.ClaveUsuario));
                        cmd.Parameters.Add(new SqlParameter("@Contraseña", sPassword));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", usuario.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@ApellidoPat", usuario.ApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@ApellidoMat", usuario.ApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@CorreoAlterno", usuario.CorreoAlterno));
                        cmd.Parameters.Add(new SqlParameter("@CorreoInst", usuario.CorreoInstitucional));
                        cmd.Parameters.Add(new SqlParameter("@Tel", usuario.TelefonoContacto));
                        cmd.Parameters.Add(new SqlParameter("@UsuarioReg", usuario.UsuarioRegistro));
                        cmd.Parameters.Add(new SqlParameter("@Perfil", usuario.Perfil));
                        cmd.Parameters.Add(new SqlParameter("@AreaPerfil", usuario.AreaAsignada));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UsuarioViewModel> GetUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetUser", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                        UsuarioViewModel result = new UsuarioViewModel();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = _MapUsuario(reader);
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> ExisteUsuario(string sClaveUsuario)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ExistUsuario", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClaveUsuario", sClaveUsuario));
                        await conn.OpenAsync();
                        int result = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = Int32.Parse(reader["REGISTROS"].ToString());
                            }
                        }
                        return result;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> UpdateUsuario(ActualizarViewModel usuario)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_UpdateUsuario", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdUsuario", usuario.idUsuario));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", usuario.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@ApellidoPat", usuario.ApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@ApellidoMat", usuario.ApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@CorreoAlterno", usuario.CorreoAlterno));
                        cmd.Parameters.Add(new SqlParameter("@CorreoInst", usuario.CorreoInstitucional));
                        cmd.Parameters.Add(new SqlParameter("@Tel", usuario.TelefonoContacto));
                        cmd.Parameters.Add(new SqlParameter("@Perfil", usuario.Perfil));
                        cmd.Parameters.Add(new SqlParameter("@AreaPerfil", usuario.AreaAsignada));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> EliminarAsignacionSolicitud(int idSolicitudAsignada) 
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString)) 
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_DesactivateSolicitudArea", conn)) 
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolicitudArea", idSolicitudAsignada));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<string> DesactivaUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_DesactivateUsuario", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ActivaUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ActivateUsuario", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SelectViewModel>> GetAreasOpinion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtSelectAreasOpinion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<SelectViewModel> result = new List<SelectViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSelectAreasOpinion(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EdoSelectViewModel>> GetEstadosAsignacion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtSelectEstadosAsignacion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<EdoSelectViewModel> result = new List<EdoSelectViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSelectEdoAsignacion(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EdoSelectViewModel>> getEdoByPerfil(int idPerfil)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtSelectEstadosAsignacionByPerfil", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdPerfil", idPerfil));
                        List<EdoSelectViewModel> result = new List<EdoSelectViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSelectEdoAsignacion(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EdoOpinionSelectViewModel>> GetEstadosDeOpinion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtSelectEstadosDeOpinion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<EdoOpinionSelectViewModel> result = new List<EdoOpinionSelectViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSelectEstadosOpinion(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SolicitudAsignadaViewModel>> GetAllSolicitudesAsignadas()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetAllSolicitudesAsignadas", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<SolicitudAsignadaViewModel> result = new List<SolicitudAsignadaViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSolicitudesAsignadas(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SolicitudAsignadaViewModel>> GetSolicituAreasAsignadas(int nIdSolicitud)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetAreasAsignadasXSolicitud", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolicitud", nIdSolicitud));
                        List<SolicitudAsignadaViewModel> result = new List<SolicitudAsignadaViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSolicitudesAsignadas(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<SolicitudAsignadaViewModel>> GetSolicitudesAsignadasByArea(int idArea)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_GetSolicitudesAsignadasByArea", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdArea", idArea));
                    List<SolicitudAsignadaViewModel> result = new List<SolicitudAsignadaViewModel>();
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(_MapSolicitudesAsignadas(reader));
                        }
                    }
                    return result;
                }
            }
        }

        public async Task<string> ObtSigDiaHabil(string FechaAsignacion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtFechaLimiteOpinion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FechaActual", FechaAsignacion));
                        string result = "";
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = reader["FECHALIMITE"].ToString();
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetAreaOpinionById(int idAreaOpinion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtAreaById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdArea", idAreaOpinion));
                    string result = "";
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result = reader["AreaAbrev"].ToString();
                        }
                    }
                    return result;
                }
            }
        }

        public async Task<string> GetEstatusasignadoById(int idEstatus)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtEstatusById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEstatus", idEstatus));
                    string result = "";
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result = reader["EdoAbrev"].ToString();
                        }
                    }
                    return result;
                }
            }
        }

        public async Task<string> GetEstatusOpinionById(int idEstatus)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtEstatusOpinionById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEstatus", idEstatus));
                    string result = "";
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result = reader["EdoAbrev"].ToString();
                        }
                    }
                    return result;
                }
            }
        }

        public async Task<string> AsignarSolicitudArea(AsignarAreaViewModel model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_AsignarSolicitudArea", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolicitud", model.idsolicitud));
                        cmd.Parameters.Add(new SqlParameter("@IdArea", model.idarea));
                        cmd.Parameters.Add(new SqlParameter("@RutaOficio", model.rutaOficioAsignacion));
                        cmd.Parameters.Add(new SqlParameter("@IdEstatusAsig", model.idestatusasignado));
                        cmd.Parameters.Add(new SqlParameter("@FechaAsig", Convert.ToDateTime(model.fechaAsignacion)));
                        cmd.Parameters.Add(new SqlParameter("@FechaLimOp", Convert.ToDateTime(model.fechalimiteopinion)));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> EditarSolicitudAreaAsignada(EditarAsignarAreaViewModel model)
        {
            try
            {
                string sOutput = "";
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_UpdateSolicitudAreaAsignada", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolArea", model.idSolArea));
                        cmd.Parameters.Add(new SqlParameter("@IdSolicitud", model.idsolicitud));
                        cmd.Parameters.Add(new SqlParameter("@IdArea", model.idarea));
                        cmd.Parameters.Add(new SqlParameter("@RutaOficio", model.rutaOficioAsignacion));
                        cmd.Parameters.Add(new SqlParameter("@IdEstatusAsig", model.idestatusasignado));
                        cmd.Parameters.Add(new SqlParameter("@FechaAsig", Convert.ToDateTime(model.fechaAsignacion)));
                        cmd.Parameters.Add(new SqlParameter("@FechaLimOp", Convert.ToDateTime(model.fechalimiteopinion)));

                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        sOutput = outPutParameter.Value.ToString();
                    }
                }
                return sOutput;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SolicitudAsignadaViewModel> GetSolicitudAsignadaById(int IdSolicitudArea)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_GetSolicitudAsignadasById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdSolicitudArea", IdSolicitudArea));
                    SolicitudAsignadaViewModel result = new SolicitudAsignadaViewModel();
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result = _MapSolicitudesAsignadas(reader);
                        }
                    }
                    return result;
                }
            }
        }
        public async Task<UsuarioViewModel> getUserByUsername(string sUsername)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetUserByUserName", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Username", sUsername));
                        UsuarioViewModel result = new UsuarioViewModel();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = _MapUsuario(reader);
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<OpinionViewModel>> GetAllOpiniones()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetAllOpiniones", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<OpinionViewModel> result = new List<OpinionViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapOpinion(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OpinionViewModel>> GetOpinionesBySolAsignada(int IdSolicitudAsignada)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetOpinionesBySolAsig", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolArea", IdSolicitudAsignada));
                        List<OpinionViewModel> result = new List<OpinionViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapOpinion(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<EdoOpinionSelectViewModel> GetEdoOpinionById(int IdEdoOpinion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetEdoOpinionById", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdEdoOpinion", IdEdoOpinion));
                        EdoOpinionSelectViewModel result = new EdoOpinionSelectViewModel();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result = _MapSelectEstadosOpinion(reader);
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> InsertarOpinion(CrearOpinionViewModel model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_CreateOpinion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdSolArea", model.IdSolicitudArea));
                        cmd.Parameters.Add(new SqlParameter("@IdEdoOpinion", model.IdEdoOpinion));
                        cmd.Parameters.Add(new SqlParameter("@RutaOficioNotificacion", model.RutaOficioNotificacion));
                        cmd.Parameters.Add(new SqlParameter("@UsrRegistro", model.UsuarioRegistra));
                        //Add the output parameter to the command object
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@output";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(outPutParameter);
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                        string sOutput = outPutParameter.Value.ToString();
                        return sOutput;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> UpdateOpinionRegistrada(ActualizarOpinionViewModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RVOE2021_UpdateOpinion", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdOpinion", model.IdOpinion));
                    cmd.Parameters.Add(new SqlParameter("@IdSolArea", model.IdSolicitudArea));
                    cmd.Parameters.Add(new SqlParameter("@IdEdoOpinion", model.IdEdoOpinion));
                    cmd.Parameters.Add(new SqlParameter("@RutaOficioNotificacion", model.RutaOficioNotificacion));
                    cmd.Parameters.Add(new SqlParameter("@UsrRegistro", model.UsuarioRegistra));
                    //Add the output parameter to the command object
                    SqlParameter outPutParameter = new SqlParameter();
                    outPutParameter.ParameterName = "@output";
                    outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                    outPutParameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(outPutParameter);
                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                    string sOutput = outPutParameter.Value.ToString();
                    return sOutput;
                }
            }
        }

        public async Task<List<RVOEViewModel>> GetRVOESByNivelMunicipio(int idNivelEd, int idMunicipio)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_GetRVOES_Aprovados", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdNivelEd", idNivelEd));
                        cmd.Parameters.Add(new SqlParameter("@IdMunicipio", idMunicipio));
                        List<RVOEViewModel> result = new List<RVOEViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapRVOE(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SelectNivelEdViewModel>> GetNivelesEducativos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtSelectNivelesEd", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<SelectNivelEdViewModel> result = new List<SelectNivelEdViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSelectNiveleEducativo(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SelectMunicipioViewModel>> GetMunicipios()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("RVOE2021_ObtSelectMunicipios", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        List<SelectMunicipioViewModel> result = new List<SelectMunicipioViewModel>();
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(_MapSelectMunicipio(reader));
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private SelectMunicipioViewModel _MapSelectMunicipio(SqlDataReader reader)
        {
            return new SelectMunicipioViewModel()
            {
                IdMunicipio = reader["IdMun"].ToString(),
                Nombre = reader["NombreMun"].ToString()
            };
        }
        private SelectNivelEdViewModel _MapSelectNiveleEducativo(SqlDataReader reader)
        {
            return new SelectNivelEdViewModel()
            {
                IdNivelEd = Int32.Parse(reader["IdNivel"].ToString()),
                descripcion = reader["Descr"].ToString()
            };
        }

        private RVOEViewModel _MapRVOE(SqlDataReader reader)
        {
            return new RVOEViewModel()
            {
                NumRVOE = reader["NumRVOE"].ToString(),
                ClaveCT = reader["ClaveCT"].ToString(),
                NombreInstitucion = reader["NombreInst"].ToString(),
                IdSolicitante = reader["IdSolicitante"].ToString(),
                NombreSolicitante = reader["NombreSolicitante"].ToString(),
                Programa = reader["Programa"].ToString(),
                Modalidad = reader["Modalidad"].ToString(),
                CorreoInstitucion = reader["CorreoInstitucional"].ToString(),
                CorreoAlterno = reader["CorreoAlterno"].ToString(),
            };
        }
        private OpinionViewModel _MapOpinion(SqlDataReader reader)
        {
            return new OpinionViewModel()
            {
                IdOpinion = Int32.Parse(reader["IdOpinion"].ToString()),
                IdSolicitudArea = Int32.Parse(reader["IdSolArea"].ToString()),
                IdEdoOpinion = Int32.Parse(reader["IdEdoOpinion"].ToString()),
                EdoOpDescripcion = reader["EdoOpDescr"].ToString(),
                RutaOficioNotificacion = reader["RutaOficioNot"].ToString(),
                RutaOficioSemsys = reader["RutaOficioSemsys"].ToString(),
                FechaRegistro = reader["FechaReg"].ToString()
            };
        }
        private SolicitudAsignadaViewModel _MapSolicitudesAsignadas(SqlDataReader reader)
        {
            return new SolicitudAsignadaViewModel()
            {
                idsolicitud = Int32.Parse(reader["IdSol"].ToString()),
                idsolicitudArea = Int32.Parse(reader["IdSolArea"].ToString()),
                numsolicitud = reader["NumSol"].ToString(),
                idarea = Int32.Parse(reader["IdAreaOp"].ToString()),
                AbrevArea = reader["AbreviaturaArea"].ToString(),
                rutaOficioAsignacion = reader["RutaOficio"].ToString(),
                idestatusasignado = Int32.Parse(reader["IdEstatusAsg"].ToString()),
                descripcionEstatus = reader["EstatusAsignado"].ToString(),
                fechaasignacion = reader["FechaAsignacion"].ToString(),
                fechalimiteopinion = reader["FechaLimiteOp"].ToString(),
                IndOpinionReg = reader["IndOpinionReg"].ToString()
            };
        }
        private EdoOpinionSelectViewModel _MapSelectEstadosOpinion(SqlDataReader reader)
        {
            return new EdoOpinionSelectViewModel()
            {
                idEdoOpinion = Int32.Parse(reader["IdEstadoOp"].ToString()),
                Abrev = reader["Abrev"].ToString(),
                descripcion = reader["Descr"].ToString()
            };
        }

        private EdoSelectViewModel _MapSelectEdoAsignacion(SqlDataReader reader)
        {
            return new EdoSelectViewModel()
            {
                idEdoAsignado = Int32.Parse(reader["IdEstadoAsig"].ToString()),
                descripcion = reader["Descr"].ToString()
            };
        }
        private SelectViewModel _MapSelectAreasOpinion(SqlDataReader reader)
        {
            return new SelectViewModel()
            {
                idAreaOpinion = Int32.Parse(reader["IdAreaOp"].ToString()),
                Abreviatura = reader["Abreviatura"].ToString()
            };
        }
        private UsuarioViewModel _MapUsuario(SqlDataReader reader)
        {
            return new UsuarioViewModel()
            {
                IdUsuario = Int32.Parse(reader["IdUsuario"].ToString()),
                ClaveUsuario = reader["ClaveUsuario"].ToString(),
                Nombre = reader["Nombre"].ToString(),
                ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                ApellidoMaterno = reader["ApellidoMaterno"].ToString(),
                CorreoInstitucional = reader["CorreoInst"].ToString(),
                TelefonoContacto = reader["Tel"].ToString(),
                Password = reader["Passw"].ToString(),
                Activo = Int32.Parse(reader["Activo"].ToString()),
                IdPerfil = Int32.Parse(reader["IdPerfil"].ToString()),
                NombrePerfil = reader["DescPerfil"].ToString(),
                AreaAsignada = reader["AreaAsignada"].ToString()
            };
        }

        private DetalleSolicitudViewModel _MapDetalleSolicitud(SqlDataReader reader)
        {
            return new DetalleSolicitudViewModel()
            {
                IdSolicitud = reader["IDSOLICITUD"].ToString(),
                fechaSolicitud = reader["FECSOL"].ToString(),
                NumSolicitud = reader["NUMSOLICITUD"].ToString(),
                PlanDeEstudios = reader["PLANESTUDIO"].ToString(),
                TipoEducativo = reader["TIPOEDU"].ToString(),
                NivelEducativo = reader["NIVELEDU"].ToString(),
                Modalidad = reader["MODALIDAD"].ToString(),
                Turno = reader["TURNO"].ToString(),
                DatosdelInmueble = reader["DIREINMUEBLE"].ToString(),
                Localidad = reader["LOCALIDAD"].ToString(),
                Municipio = reader["MUNICIPIO"].ToString(),
                Estado = reader["ESTADO"].ToString(),
                Telefono = reader["TELEFONO"].ToString(),
                Email = reader["CORREOE"].ToString(),
                PersonasAuthorizadas = reader["PERSONASAUTO"].ToString(),
                DomicilioRecibieNotificaciones = reader["DOMNOTI"].ToString(),
                TipoPersona = reader["TIPOPERSONA"].ToString(),
                Solicitante = reader["SOLICITANTE"].ToString(),
                RepresentanteLegal = reader["REPLEGAL"].ToString(),
                OpcionEducativa = reader["OPEDUCA"].ToString()
            };
        }

        private SolicitudViewModel _MapSolicitud(SqlDataReader reader)
        {
            return new SolicitudViewModel()
            {
                IdSolicitud = Int32.Parse(reader["Id"].ToString()),
                NumSolicitud = reader["NumSolicitud"].ToString(),
                IdSolicitante = reader["IdSolicitante"].ToString(),
                NombreInstEducativa = reader["NombreInstEduc"].ToString(),
                Estatus = reader["Estatus"].ToString(),
                FechaSolicitud = reader["FechaSolicitud"].ToString(),
                Nivel = Int32.Parse(reader["IdNivelEd"].ToString()),
                NivelDescripcion = reader["DescNivelEd"].ToString(),
                PlanyPrograma = reader["PlanProg"].ToString(),
            };
        }
        private SolicitudesViewModel _MapSolicitudes(SqlDataReader reader)
        {
            return new SolicitudesViewModel()
            {
                IdSolicitud = Int32.Parse(reader["Id"].ToString()),
                NumSolicitud = reader["NumSolicitud"].ToString(),
                IdSolicitante = reader["IdSolicitante"].ToString(),
                NombreInstEducativa = reader["NombreInstEduc"].ToString(),
                Estatus = reader["Estatus"].ToString(),
                FechaSolicitud = reader["FechaSolicitud"].ToString(),
                Nivel = Int32.Parse(reader["IdNivelEd"].ToString()),
                NivelDescripcion = reader["DescNivelEd"].ToString(),
                PlanyPrograma = reader["PlanProg"].ToString(),
            };
        }
    }
}
