using Postgrest.Models;
using Postgrest.Attributes;

public class score : BaseModel
{
    [Column("id"), PrimaryKey]
    public int id { get; set; }


    [Column("score")]
    public int puntaje { get; set; }
    [Column("usuario_id")]
    public int usuario_id { get; set; }
    
}