namespace CadastroDeProdutos
{
    public class CadProduto
    {
        public int Id { get; set; }
        public string? NomeProduto { get; set; }
        [Key] public int CodigoProduto { get; set; }
        public double Preco { get; set; }
        public string? Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public double Avaliacao { get; set; }
        public string? Categoria { get; set; }
    }
}
