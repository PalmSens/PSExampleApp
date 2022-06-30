namespace PSHeavyMetal.Core.Services
{
    public interface ILanguageService
    {
        /// <summary>
        /// Change the current language
        /// </summary>
        /// <param name="language"></param>
        public void ChangeLanguage(string language);
    }
}