using System;
using System.Collections.Generic;

namespace EjercicioEntity.Models
{
    public partial class Persona
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
