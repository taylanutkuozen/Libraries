namespace LibrariesFluentValidation.DTOs
{
    public class CustomerDto
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMail { get; set; }
        public int CustomerAge { get; set; }
        public string FullName { get; set; }
        public string CCNumber { get; set; }
        public DateTime CCValidDate { get; set; }
        /*public string CreditCardNumber { get; set; } //Flattening otomatik yapabilmek için ilgili class ve ilgili property isimleri birleştirilir ve type olarak ilgili property type verilir.
        public DateTime CreditCardValidDate { get; set; }*/
    }
}
