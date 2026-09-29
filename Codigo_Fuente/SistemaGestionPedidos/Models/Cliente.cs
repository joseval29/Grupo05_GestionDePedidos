using System.ComponentModel.DataAnnotations;

namespace SistemaGestionPedidos.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Display(Name = "Fecha de registro")]
    [DataType(DataType.Date)]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Relación 1 a muchos: un cliente puede tener muchos pedidos
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
