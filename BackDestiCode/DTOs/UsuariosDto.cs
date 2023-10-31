﻿using BackDestiCode.Data.Models;
using System.Collections;

namespace BackDestiCode.DTOs
{
    public class UsuariosDto
    {
        public Guid Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Token { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public bool Estatus { get; set; }

        //public IList<DatosPersonales> DatosPersonales { get; set; }
    }
}
