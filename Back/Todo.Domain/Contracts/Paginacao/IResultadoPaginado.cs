namespace Todo.Domain.Contracts.Paginacao;

public interface IResultadoPaginado<T>
{
    public IList<T> Itens { get; set; }
    public IPaginacao Paginacao { get; set; }
}