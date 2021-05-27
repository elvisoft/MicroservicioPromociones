using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Core.Entities
{
    public class Promocion
    {
        [BsonId]
        public Guid id { get; set; }
        public IEnumerable<string> mediosDePago { get; set; }
        public IEnumerable<string> bancos { get; set; }
        public IEnumerable<string> categoriasProductos { get; set; }
        public int? maximaCantidadDeCuotas { get; set; }
        public decimal? valorInteresCuotas { get; set; }
        public decimal? porcentajeDeDescuento { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public bool activo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
    }
}
