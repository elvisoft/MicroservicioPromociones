using Microservicio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservicio.Api.Validators
{
    public class PromocionValid
    {
        public static bool validarAlAgregar(Promocion promocion )
        {
            //controlar repetidos
            if (promocion.mediosDePago.Any())
            {
                var anyDuplicateMediosPago = promocion.mediosDePago.GroupBy(rr => rr).Any(g => g.Count() > 1);
                if (anyDuplicateMediosPago)
                    return false;
            }

            if (promocion.bancos.Any())
            {
                var anyDuplicatebancos = promocion.bancos.GroupBy(rr => rr).Any(g => g.Count() > 1);
                if (anyDuplicatebancos)
                    return false;
            }

            if (promocion.categoriasProductos.Any())
            {
                var anyDuplicatecategorias = promocion.categoriasProductos.GroupBy(rr => rr).Any(g => g.Count() > 1);
                if (anyDuplicatecategorias)
                    return false;
            }

            if (!promocion.maximaCantidadDeCuotas.HasValue && !promocion.porcentajeDeDescuento.HasValue)
                  return false; //no pueden ser los dos vacios
            
            if(promocion.valorInteresCuotas.HasValue && !promocion.maximaCantidadDeCuotas.HasValue)
                  return false; //solo puede tener valor interes de cuotas si tiene cantidad de cuotas


            //Porcentaje descuento en caso de tener valor, debe estar comprendido    entre 5 y 80
            if (promocion.porcentajeDeDescuento.HasValue)
                if (!(promocion.porcentajeDeDescuento.Value >= 5 && promocion.porcentajeDeDescuento.Value <= 80))
                    return false;
            
            // Fecha inicio no puede ser mayor que fecha fin
            if(promocion.fechaFin.HasValue && promocion.fechaInicio.HasValue)
                if(promocion.fechaFin.Value.Date<promocion.fechaInicio.Value.Date)
                    return false;

            return true;
        }
        public static bool validarxValores(Promocion promocion)
        {

            IEnumerable<string> MediosdepagoOK = new List<string>() { "TARJETA_CREDITO", "TARJETA_DEBITO", "EFECTIVO", "GIFT_CARD" };

            foreach (var item in promocion.mediosDePago)
                if (!MediosdepagoOK.Any(rr => rr.Contains(item)))
                    return false;

            IEnumerable<string> BancosOK = new List<string>() { "Galicia", "Santander Rio", "Ciudad", "Nacion", "ICBC", "BBVA", "Macro" };
            foreach (var item in promocion.bancos)
                if (!BancosOK.Any(ss => ss.Contains(item)))
                    return false;

            IEnumerable<string> CategoriasOK = new List<string>() { "Hogar", "Jardin", "ElectroCocina", "GrandesElectro", "Colchones", 
                "Celulares", "Tecnologia", "Audio" };
            foreach (var item in promocion.categoriasProductos)
                if (!CategoriasOK.Any(rr => rr.Contains(item)))
                    return false;

            return true;

        }
    }
}
