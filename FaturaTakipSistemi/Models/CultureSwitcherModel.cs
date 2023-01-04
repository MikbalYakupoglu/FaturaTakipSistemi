using System.Globalization;

namespace FaturaTakip.Models;

public class CultureSwitcherModel
{
    public CultureInfo CurrentUICulture { get; set; }
    public List<CultureInfo> SupportedCultures { get; set; }
}