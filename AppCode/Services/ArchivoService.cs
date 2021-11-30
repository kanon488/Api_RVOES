using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_RVOES.AppCode.Services
{
    public interface IArchivoService
    {
        string AlmacenarArchivo(IFormFile file, string sRutaDestino, string sNombreArchivo, string ExtensionArchivo);
        MemoryStream RecuperarArchivo(string sRutaDestino);
        bool bExisteArchivo(string sRutaCompletaDelArchivo);
        bool bExisteDirectorio(string sRutaDelDirectorio);
        bool bCrearDirectorio(string sRutaDelDirectorio);
        string QuitarCaracteresEspeciales(string sCadena);

    }
    public class ArchivoService : IArchivoService
    {
        private readonly IConfiguration _configuration;

        public ArchivoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string AlmacenarArchivo(IFormFile file, string sRutaDestino, string sNombreArchivo, string ExtensionArchivo)
        {
            try
            {
                string sUrlBase = _configuration["FileServer:UrlBase"]; ;
                string sFileServerUser = _configuration["FileServer:User"];
                string sFileServerUserPwd = _configuration["FileServer:Pwd"];
                string sFileServerDomain = _configuration["FileServer:Domain"];

                //string sFolderPath = sUrlBase + "\\" + sNumSolicitud;

                string sFolderPath = sUrlBase + "\\" + sRutaDestino;

                //Gestor de autenticación
                NtlmPasswordAuthentication AuthUser = new NtlmPasswordAuthentication(sFileServerDomain, sFileServerUser, sFileServerUserPwd);

                //Carpeta destino contenedora
                SmbFile RemoteFolder = new SmbFile(sFolderPath, AuthUser);

                if (!bExisteDirectorio(sFolderPath))
                {
                    bCrearDirectorio(sFolderPath);
                }
                var sDestinationFile = Path.Combine(sFolderPath, sNombreArchivo + ExtensionArchivo);
                using (FileStream fs = System.IO.File.Create(sDestinationFile))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                return sDestinationFile;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public MemoryStream RecuperarArchivo(string sRutaDestino)
        {
            try
            {
                var ms = new MemoryStream();
                string sFileServerUser = _configuration["FileServer:User"];
                string sFileServerUserPwd = _configuration["FileServer:Pwd"];
                string sFileServerDomain = _configuration["FileServer:Domain"];

                //Gestor de autenticación
                NtlmPasswordAuthentication AuthUser = new NtlmPasswordAuthentication(sFileServerDomain, sFileServerUser, sFileServerUserPwd);

                SmbFile fileRemoto = new SmbFile(sRutaDestino, AuthUser);
                var stream = System.IO.File.OpenRead(sRutaDestino);
                stream.CopyTo(ms);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool bExisteArchivo(string sRutaCompletaArchivo)
        {
            bool bExisteArchivo = false;
            try
            {
                if (File.Exists(sRutaCompletaArchivo))
                {
                    bExisteArchivo = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return bExisteArchivo;
        }

        public bool bExisteDirectorio(string sRutaDirectorio)
        {
            bool bExiste = false;
            try
            {
                if (Directory.Exists(sRutaDirectorio))
                {
                    bExiste = true;
                }
            }
            catch (Exception ex)
            {

            }
            return bExiste;
        }

        public bool bCrearDirectorio(string sRutaDirectorio)
        {
            bool bExiste = false;
            try
            {
                if (!Directory.Exists(sRutaDirectorio))
                {
                    Directory.CreateDirectory(sRutaDirectorio);
                    if (bExisteDirectorio(sRutaDirectorio))
                        bExiste = true;
                }
            }
            catch (Exception ex)
            {

            }
            return bExiste;
        }

        public string QuitarCaracteresEspeciales(string sNombreArchivo)
        {
            var charsToRemove = new string[] { "\\", "/", ":", "*", "?", "<", ">", "|", "[", "]", "}", "{", "+", "#", "$", "%", "(", ")", " " };
            foreach (var c in charsToRemove)
            {
                sNombreArchivo = sNombreArchivo.Replace(c, string.Empty);
            }
            return sNombreArchivo;
        }
    }
}
