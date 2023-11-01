using System.ComponentModel.DataAnnotations;
namespace FluentValidation.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMail { get; set; }
        public int CustomerAge { get; set; }
        public DateTime? BirthDay { get; set; }
        public IList<Address> Addresses { get; set; }
        public Gender Gender { get; set; }
    }
}