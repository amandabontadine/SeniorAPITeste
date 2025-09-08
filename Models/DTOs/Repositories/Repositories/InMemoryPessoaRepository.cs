using SeniorAPITeste.Models;

namespace SeniorAPITeste.Repositories;

public sealed class InMemoryPessoaRepository : IPessoaRepository
{
    private readonly List<Pessoa> _db = new()
    {
        new Pessoa { Codigo = 1, Nome = "João", Cpf = "11144477735", Uf = "SP", DataNascimento = new(1990,1,1) },
        new Pessoa { Codigo = 2, Nome = "Maria", Cpf = "22233344405", Uf = "RJ", DataNascimento = new(1992,5,10) }
    };
    private int _seq = 3;

    public IEnumerable<Pessoa> Listar() => _db;

    public IEnumerable<Pessoa> ListarPorUf(string uf) =>
        _db.Where(p => string.Equals(p.Uf, uf, StringComparison.OrdinalIgnoreCase));

    public Pessoa? Obter(int codigo) => _db.FirstOrDefault(p => p.Codigo == codigo);

    public Pessoa Adicionar(Pessoa pessoa)
    {
        pessoa.Codigo = _seq++;
        _db.Add(pessoa);
        return pessoa;
    }

    public Pessoa? Atualizar(int codigo, Pessoa pessoa)
    {
        var existente = Obter(codigo);
        if (existente is null) return null;

        existente.Nome = pessoa.Nome;
        existente.Cpf = pessoa.Cpf;
        existente.Uf = pessoa.Uf;
        existente.DataNascimento = pessoa.DataNascimento;
        return existente;
    }

    public bool Remover(int codigo)
    {
        var p = Obter(codigo);
        if (p is null) return false;
        _db.Remove(p);
        return true;
    }
}
