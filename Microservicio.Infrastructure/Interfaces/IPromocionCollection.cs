using Microservicio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservicio.Infrastructure.Interfaces
{
    public interface IPromocionCollection
    {
        Task insertPromocion(Promocion promocion);
        Task UpdatePromocion(Promocion promocion);        
        Task<List<Promocion>> getallPromociones();
        Task<Promocion> GetPromocionById(Guid id);
        Task<List<Promocion>> getPromocionesVigentes();
        Task<List<Promocion>> getPromocionesVigentesxFecha(DateTime? fecha);
        Task<List<Promocion>> getPromocionesVigentesxVenta(Promocion promocion);
        Task ModificarPromocion(Promocion promocion);
        Task UpdateVigencia(Promocion promocion, Guid id);
        Task EliminarPromocion(Guid id);
    }
}
