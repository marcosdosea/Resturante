using Core.DTO;
using Core;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ItemcardapioService : IItemcardapioService
    {
        private readonly RestauranteContext context;
        public ItemcardapioService(RestauranteContext context)
        {
            this.context = context;
        }

        public uint Create(Itemcardapio itemcardapio)
        {
            context.Add(itemcardapio);
            context.SaveChanges();
            return itemcardapio.Id;
        }

        public void Delete(uint id)
        {
            var itemcardapio = context.Itemcardapios.Find(id);

            if (itemcardapio != null)
            {
                context.Remove(itemcardapio);
                context.SaveChanges();
            }

        }

        public void Edit(Itemcardapio itemcardapio)
        {

            context.Update(itemcardapio);
            context.SaveChanges();

        }

        

        public Itemcardapio? Get(uint id)
        {
            return context.Itemcardapios.Find(id);
        }

        public IEnumerable<Itemcardapio> GetAll()
        {
            return context.Itemcardapios.AsNoTracking();
        }

        public IEnumerable<ItemcardapioDto> GetByNome(string nome)
        {
            var query = from Itemcardapio in context.Itemcardapios
                        where Itemcardapio.Nome.StartsWith(nome)
                        orderby Itemcardapio.Nome
                        select new ItemcardapioDto
                        {
                            Id = Itemcardapio.Id,
                            Nome = Itemcardapio.Nome,      
                        };
            return query;
        }

        public async Task<List<ItemcardapioDto>> Buscaritemporid(uint id)
        {
            var itemcardapio = await context.Itemcardapios
                .Where(g => g.Id == id)
                .Select(g => new ItemcardapioDto
                {
                    Id = g.Id,
                    Nome = g.Nome,
                    Preco = g.Preco,
                    Disponivel = g.Disponivel,
                    IdRestaurante = g.IdRestaurante

                })
                .ToListAsync();

            return itemcardapio;
        }

        public async Task<List<ItemcardapioDto>> BuscarItensPorNome(string nome)
        {
            return await context.Itemcardapios
                .Where(i => i.Nome.Contains(nome))
                .Select(i => new ItemcardapioDto
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Preco = i.Preco,
                    Disponivel = i.Disponivel,
                    IdRestaurante = i.IdRestaurante
                })
                .ToListAsync();
        }
    }
}
