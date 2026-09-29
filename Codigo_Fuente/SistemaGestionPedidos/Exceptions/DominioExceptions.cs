namespace SistemaGestionPedidos.Exceptions;

/// <summary>
/// Se lanza cuando se intenta vender más unidades de un producto de las que hay en stock.
/// </summary>
public class StockInsuficienteException : Exception
{
    public StockInsuficienteException(string nombreProducto, int stockDisponible, int cantidadSolicitada)
        : base($"Stock insuficiente para '{nombreProducto}'. Disponible: {stockDisponible}, solicitado: {cantidadSolicitada}.")
    {
    }
}

/// <summary>
/// Se lanza cuando se busca una entidad (Cliente, Producto, Pedido) que no existe en la base de datos.
/// </summary>
public class EntidadNoEncontradaException : Exception
{
    public EntidadNoEncontradaException(string entidad, int id)
        : base($"{entidad} con Id {id} no fue encontrado.")
    {
    }
}

/// <summary>
/// Se lanza cuando un pedido no cumple las reglas mínimas de negocio (ej. sin líneas de producto).
/// </summary>
public class PedidoInvalidoException : Exception
{
    public PedidoInvalidoException(string mensaje) : base(mensaje)
    {
    }
}
