#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ushtrimi_pergjithshem.Models;
public class Magazine
{    
    [Key]
    public int MagazineId { get; set; }    
    public string Title { get; set; }  
    // CreatedAt and UpdatedAt removed for brevity
    // Our navigation property to our Subscription class
    // Notice there is NO reference to the Person class   
    public List<Subscription> Readers { get; set; } = new List<Subscription>();
}
