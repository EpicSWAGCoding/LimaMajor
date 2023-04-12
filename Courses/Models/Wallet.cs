namespace Courses.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string WalletNum { get; set; }
        public double Money { get; set; }
        public int UserId { get; set; }
        public User user{ get; set; }
    }
}
