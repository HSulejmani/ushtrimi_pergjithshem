#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ushtrimi_pergjithshem.Models;
public class Person
{    
    [Key]
    public int PersonId { get; set; }    
    public string Name { get; set; }  
    // CreatedAt and UpdatedAt removed for brevity
    // Our Person class also needs a reference to Subscriptions
    // and contains NO reference to Magazines  
    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
