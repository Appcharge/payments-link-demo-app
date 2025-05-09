namespace Appcharge.Utils {
    public enum PurchaseStatus
    {
        Success,
        Fail,
        Cancel,
        Close,
        Unknown
    }

    public class PurchaseStatusUtils
    {
        public static PurchaseStatus ParseStatusFromDeepLink(string deepLinkUrl)
        {
            var uri = new System.Uri(deepLinkUrl);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            string statusValue = query.Get("status");

            if (System.Enum.TryParse(statusValue, true, out PurchaseStatus status))
            {
                return status;
            }

            return PurchaseStatus.Unknown;
        }
    }
}