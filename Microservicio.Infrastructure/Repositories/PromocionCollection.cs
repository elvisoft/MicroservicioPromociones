using Microservicio.Core.Entities;
using Microservicio.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservicio.Infrastructure.Repositories
{
    public class PromocionCollection : IPromocionCollection
    {
        internal PromocionRepository _repository = new PromocionRepository();
        private IMongoCollection<Promocion> Collection;
        public PromocionCollection()
        {
            Collection = _repository.db.GetCollection<Promocion>("Promociones");
        }

        //borrado fisico !no va
        public async Task DeletePromocion(string id)
        {
            var filter = Builders<Promocion>.Filter.Where(r => r.id.ToString()== id);
            await Collection.DeleteOneAsync(filter);
        }

        public async Task<List<Promocion>> getallPromociones()
        {
            var filter = Builders<Promocion>.Filter.Where(r => r.activo==true);
            return await Collection.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<Promocion> GetPromocionById(Guid id)
        {            
            //var filter2 = Builders<Promocion>.Filter.Eq("_id", new ObjectId(id));

            var filter = Builders<Promocion>.Filter
                   .Where(r => r.id==id && r.activo==true );
            return await Collection.FindAsync(filter).Result.FirstAsync();
        }

        public async Task<List<Promocion>> getPromocionesVigentes()
        {
            var today = DateTime.Now;
            var filter = Builders<Promocion>.Filter
                    .Where(r => r.fechaInicio.Value<=today && r.fechaFin.Value>= today && r.activo==true);
            
            //var options = new FindOptions<Promocion, BsonDocument> { Projection = null };
            //var promociones = await Collection.FindAsync(filter);

            return await  Collection.FindAsync(filter).Result.ToListAsync();
        }
        public async Task<List<Promocion>> getPromocionesVigentesxFecha(DateTime? fecha)
        {            
            var filter = Builders<Promocion>.Filter
                    .Where(r => r.fechaInicio.Value <= fecha && r.fechaFin.Value >= fecha && r.activo == true);            

            return await Collection.FindAsync(filter).Result.ToListAsync();
        }
        public async Task<List<Promocion>> getPromocionesVigentesxVenta(Promocion promocion)
        {
            var filter = Builders<Promocion>.Filter
                    .Where(r => r.mediosDePago.Contains(promocion.mediosDePago.FirstOrDefault()) 
                    && r.bancos.Contains(promocion.bancos.FirstOrDefault()) 
                    && r.categoriasProductos.Contains(promocion.categoriasProductos.FirstOrDefault()) 
                    && r.activo==true);
        

            return await Collection.FindAsync(filter).Result.ToListAsync();
        }

        public async Task insertPromocion(Promocion promocion)
        {
            promocion.activo = true;
            promocion.fechaCreacion = DateTime.Now;            
            await Collection.InsertOneAsync(promocion);
        }

        public Task ModificarPromocion(Promocion promocion)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePromocion(Promocion promocion)
        {
            promocion.fechaModificacion = DateTime.Now;
            var filter = Builders<Promocion>.Filter.Eq(r => r.id, promocion.id);
            await Collection.ReplaceOneAsync(filter,promocion);
            
        }

        public async Task EliminarPromocion(Guid id)
        {
            var filter = Builders<Promocion>.Filter.Eq(s => s.id, id);
            var update = Builders<Promocion>.Update.Set(s => s.activo, false).Set(c=>c.fechaModificacion,DateTime.Now);
            await Collection.UpdateOneAsync(filter, update);

        }
        public async Task UpdateVigencia(Promocion promocion, Guid id)
        {
            var filter = Builders<Promocion>.Filter.Eq(s => s.id, id);
            var update = Builders<Promocion>.Update.Set(s=>s.fechaInicio, promocion.fechaInicio)
                                                    .Set(rr=>rr.fechaFin, promocion.fechaFin)
                                                    .Set(c => c.fechaModificacion, DateTime.Now);

            await Collection.UpdateOneAsync(filter, update);
        }
       

        //public async Task DeletePromocion(Guid id)
        //{
        //    Promocion promocion = new Promocion();
        //    promocion.activo = false;
        //    await Collection.FindOneAndUpdateAsync(rr => rr.id == id, s=>s.activo=false,null);
        //    var filter = Builders<Promocion>.Filter.Eq(r => r.id, id);
        //    await Collection.ReplaceOneAsync(filter, promocion);
        //}

    }
}
