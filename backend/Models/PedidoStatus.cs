namespace Pim.Models
{
    public static class PedidoStatus
    {
        public const string Recebido = "recebido";
        public const string EmPreparo = "em_preparo";
        public const string Pronto = "pronto";
        public const string Finalizado = "finalizado";
        public const string Cancelado = "cancelado";

        public static readonly string[] Todos =
        {
            Recebido,
            EmPreparo,
            Pronto,
            Finalizado,
            Cancelado
        };

        public static readonly string[] StatusOperacionais =
        {
            Recebido,
            EmPreparo,
            Pronto,
            Finalizado,
            Cancelado
        };
    }
}
