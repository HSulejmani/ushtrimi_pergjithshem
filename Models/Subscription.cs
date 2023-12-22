#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ushtrimi_pergjithshem.Models;
public class Subscription
{
    [Key]    
    public int SubscriptionId { get; set; } 
    // The IDs linking to the adjoining tables   
    public int PersonId { get; set; }    
    public int MagazineId { get; set; }
    // Our navigation properties - don't forget the ?    
    public Person? Person { get; set; }    
    public Magazine? Magazine { get; set; }
}
