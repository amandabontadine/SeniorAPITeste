namespace SeniorAPITeste.Models;

public sealed class Pessoa
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Uf { get; set; } = default!; // Ex.: "SP"
    public DateTime DataNascimento { get; set; }
}