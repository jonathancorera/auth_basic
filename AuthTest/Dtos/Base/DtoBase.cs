namespace AuthTest.Dtos.Base
{
    public class DtoBase
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CreatedById { get; set; }
    }
}
