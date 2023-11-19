﻿using BackDestiCode.Data.Models;

namespace BackDestiCode.DTOs
{
    public class ReservacionesDto
    {
        public Guid Id_Reservacion { get; set; }
        //El Id Ruta se modifico a Guid
        public Guid Id_Ruta { get; set; }
        public Guid Id_Usuario { get; set; }
        public int Num_Asientos { get; set; }
        public DateTime Fecha_Reservacion { get; set; }
        public bool Estatus { get; set; }

        //Relaciones
        public Usuarios? Usuarios { get; set; }
        public Rutas? Rutas { get; set; }
    }
}
