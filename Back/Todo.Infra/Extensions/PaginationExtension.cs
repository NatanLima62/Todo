using Microsoft.EntityFrameworkCore;
using Todo.Domain.Paginacao;

namespace Todo.Infra.Extensions;

public static class PaginationExtension
{
    public static async Task<ResultadoPaginado<T>> BuscarPaginadoAsync<T>(this IQueryable<T> query, int pagina, int porPagina) where T : class
    {
        var resultado = new ResultadoPaginado<T>(pagina, porPagina, await query.CountAsync());

        var quantidadePaginas = (double)resultado.Paginacao.Total / porPagina;
        resultado.Paginacao.TotalDePaginas = (int)Math.Ceiling(quantidadePaginas);

        var pular = (pagina - 1) * porPagina;
        resultado.Itens = await query.Skip(pular).Take(porPagina).ToListAsync();
        resultado.Paginacao.TotalNaPagina = resultado.Itens.Count;
        return resultado;
    }
}