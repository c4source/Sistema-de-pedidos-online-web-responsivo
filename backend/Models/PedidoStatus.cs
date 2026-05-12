namespace Pim.Models
{
    public static class PedidoStatus
    {
        public const string AguardandoAprovacao = "Aguardando Aprovação";
        public const string Aprovado = "Aprovado";
        public const string EmPreparacao = "Em Preparação";
        public const string ProntoParaRetirada = "Pronto para Retirada";
        public const string SaiuParaEntrega = "Saiu para Entrega";
        public const string Entregue = "Entregue";
        public const string Retirado = "Retirado";
        public const string Cancelado = "Cancelado";

        public static readonly string[] Todos =
        {
            AguardandoAprovacao,
            Aprovado,
            EmPreparacao,
            ProntoParaRetirada,
            SaiuParaEntrega,
            Entregue,
            Retirado,
            Cancelado
        };

        public static readonly string[] StatusOperacionais =
        {
            Aprovado,
            EmPreparacao,
            ProntoParaRetirada,
            SaiuParaEntrega,
            Entregue,
            Retirado
        };
    }
}
