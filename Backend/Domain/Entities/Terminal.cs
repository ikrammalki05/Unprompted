namespace Domain.Entities;
public class SessionTerminal
{
    public Guid IdSession { get; set; }

    public string IdConteneurDocker { get; set; }
        = string.Empty;

    public string IdUtilisateur { get; set; }
        = string.Empty;

    public DateTime DateCreation { get; set; }
        = DateTime.UtcNow;

    public bool Active { get; set; } = true;
}