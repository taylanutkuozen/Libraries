namespace LibrariesFluentValidation.Models
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
        public CreditCard CreditCard { get; set; }
        //public string GetFullName()//Get koyarak başına otomatik map işlemi oldu
        //{
        //    return $"{CustomerName}---{CustomerMail}---{CustomerAge}"; 
        //}
        public string FullName2()
        {
            return $"{CustomerName}-{CustomerMail}-{CustomerAge}";
        }
    }
}