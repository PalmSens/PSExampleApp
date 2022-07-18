namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface IMessageService
    {
        public void LongAlert(string message);

        public void ShortAlert(string message);
    }
}