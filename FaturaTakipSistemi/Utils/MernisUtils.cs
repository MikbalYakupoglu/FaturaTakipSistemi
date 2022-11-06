using GovermentIdVerification;

namespace FaturaTakip.Utils;

public static class MernisUtils    
{
    public static async Task<bool> VerifyGovermentId(string govermentId, string name, string lastName, int yearOfBirth) 
    {
        // TODO : Development için deaktif edilmiştir.
        //var client = new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);
        //var response = await client.TCKimlikNoDogrulaAsync(long.Parse(govermentId), name, lastName, yearOfBirth);
        //var result = response.Body.TCKimlikNoDogrulaResult;

        //return result;

        return true;
    }
}