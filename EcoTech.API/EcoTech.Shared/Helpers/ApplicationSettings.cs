namespace EcoTech.Shared.Helpers;
public class ApplicationSettings  //==  AS
{
    public AS_Secrets Secrets { get; set; }
    public AS_Configuration Configuration { get; set; }
    public AS_ConnectionStrings ConnectionStrings { get; set; }
    public AS_Azure Azure { get; set; }
    public AS_API Api { get; set; }
    public AS_Communication Communication { get; set; }

}
public class AS_Secrets
{
    public string JwtSiginingKey { get; set; }
    public string AESKey { get; set; }
    public string AESIV { get; set; }
}
public class AS_Configuration
{
    public string UseDynamicSecrets { get; set; }
    public string UseAppSettingsConnectionStrings { get; set; }
    public string UseKeyVaultConnectionStrings { get; set; }
    public string UseEnvironmentalConnectionStrings { get; set; }


}
public class AS_ConnectionStrings
{

    public string EcoTech { get; set; }
 
}
public class AS_Azure
{
    public string B2B_RCS_AZ_ConnectionString { get; set; }
    public bool UseAzure { get; set; }
    public string Db_Connection_Name { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string VaultUrl { get; set; }
    public string AzureAd { get; set; }
    public string FileUploadURL { get; set; }
    public string FileDownloadURL { get; set; }
    public string SubscriptionKeyName { get; set; }
    public string SubscriptionKey { get; set; }
    public string FileUploadTokenKeyName { get; set; }
    public string FileUploadTokenKey { get; set; }
    public string FileDownloadTokenKeyName { get; set; }
    public string FileDownloadTokenKey { get; set; }

    #region NOT IN USE
    /*     public string B2B_RCS_AZ_ConnectionString { get; set; }
    public string Mobile_ConnectionString { get; set; }*/
    #endregion
}
public class AS_API
{

    public string TokenValidationUrl { get; set; }
    public string TokenSubscriptionKeyName { get; set; }
    public string TokenSubscriptionKey { get; set; }
    public string Agency_username_Decrpt_Key { get; set; }

    #region NOT IN USE  
    /*  public string AadharMaskingUrl { get; set; }
      public string AadharMaskingKeyName { get; set; }
      public string AadharMaskingKeyValue { get; set; }
      public string Signatory_Mask_URL { get; set; }
      public string Signatory_Mask_Key { get; set; }
      public string AgencyBPES_DMS_AddFolder_URL { get; set; }
      public string AgencyBPES_DMS_AddFolder_Key { get; set; }
      public string AgencyBPES_DMS_AddDocument_URL { get; set; }
      public string AgencyBPES_DMS_AddDocument_Key { get; set; }
      public string PANVerificationURL { get; set; }
      public string AGENCYBPES_PAN_SERVICE_Key { get; set; }
      public string AGENCYBPES_PAN_SERVICE_PRODUCT { get; set; }
      public string EStamp_UniqueRefNo_Url { get; set; }
      public string EStamp_UniqueRefNo_Auth_Key { get; set; }
      public string EStamp_UniqueRefNo_Access_Key { get; set; }
      public string AGENCYBPES_IMPS_URL { get; set; }
      public string AGENCYBPES_IMPS_Key { get; set; }
      public string EStampStateListUrl { get; set; }
      public string EStampSubscriptionKey { get; set; }
      public string EStampAccessKey { get; set; }
      public string URL_CIBIL { get; set; }
      public string CibilSubscriptionKey { get; set; }
      public string URL_OuathToken { get; set; }
      public string CibilClientId { get; set; }
      public string CibilClientSecret { get; set; }*/
    #endregion
}
public class AS_Communication
{
    public string PostManAuthorizationToken { get; set; }
    public string PostManBaseAddress { get; set; }




}
