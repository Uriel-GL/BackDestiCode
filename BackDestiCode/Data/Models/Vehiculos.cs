﻿namespace BackDestiCode.Data.Models
{
    public class Vehiculos
    {
        public Guid Id_Unidad { get; set; }
        public Guid Id_Usuario { get; set; }
        public string Color { get; set; }
        public string Placa { get; set; }
        public string Imagen { get; set; }
        public string Modelo { get; set; }

        //Relaciones 
        public Usuarios Usuarios { get; set; }
        public ICollection<Rutas> Rutas { get; set; }
    }
}
