namespace PSHeavyMetal.Core.Services
{
    //public class LanguagePair
    //{
    //    public Func<string> name;
    //    public string value;
    //}

    //public class LanguageService : ILanguageService
    //{
    //    public LanguageService()
    //    {
    //        CurrentLanguage = new LocalizedString(() => GetCurrentLanguageName());

    //        languageMapping = new List<LanguagePair>();
    //        languageMapping.Add(new LanguagePair { name = () => AppResource.English, value = "en" });
    //        languageMapping.Add(new LanguagePair { name = () => AppResource.Spanish, value = "es" });
    //    }

    //    public LocalizedString CurrentLanguage { get; }

    //    private List<LanguagePair> languageMapping { get; }

    //    public void ChangeLanguage(string language)
    //    {
    //        string selectedName = await Application.Current.MainPage.DisplayActionSheet(
    //    AppResource.ChangeLanguage,
    //    null, null,
    //    languageMapping.Select(m => m.name()).ToArray());
    //        if (selectedName == null)
    //        {
    //            return;
    //        }

    //        string selectedValue = languageMapping.Single(m => m.name() == selectedName).value;
    //        LocalizationResourceManager.Current.CurrentCulture = selectedValue == null ? CultureInfo.CurrentCulture : new CultureInfo(selectedValue);
    //    }

    //    private string GetCurrentLanguageName()
    //    {
    //        string name = languageMapping.SingleOrDefault(m => m.value == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName).name.ToString();

    //        return name != null ? name : LocalizationResourceManager.Current.CurrentCulture.DisplayName;
    //    }
    //}
}