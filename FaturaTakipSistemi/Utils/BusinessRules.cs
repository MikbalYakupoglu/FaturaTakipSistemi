using FaturaTakip.Utils.Results;
using IResult = FaturaTakip.Utils.Results.IResult;

namespace FaturaTakip.Utils;

public class BusinessRules
{
    public static IResult Run(params IResult[] rules)
    {
        foreach (var rule in rules)
        {
            if (!rule.IsSuccess)
            {
                return rule;
            }
        }
        return new SuccessResult();
    }
}