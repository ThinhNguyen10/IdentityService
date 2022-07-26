namespace Identity.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PassWord { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
