namespace ReadyTechCoffee.Business.Services
{
    public interface IDate
    {
        public DateTime GetCurrentDate();
    }

    public class Date: IDate
    {
        public DateTime GetCurrentDate()
        {
            return System.DateTime.Now;
        }
    }
}