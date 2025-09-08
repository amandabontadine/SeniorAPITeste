namespace SeniorAPITeste.DTOs;

public sealed class PessoaCreateDto
{
    public string Nome { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Uf { get; set; } = default!;
    public DateTime DataNascimento { get; set; }
}

public sealed class PessoaUpdateDto
{
    public string Nome { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Uf { get; set; } = default!;
    public DateTime DataNascimento { get; set; }
}
