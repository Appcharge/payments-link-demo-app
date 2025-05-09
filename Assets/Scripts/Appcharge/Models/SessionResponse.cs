using System;

namespace Appcharge.Models
{
[Serializable]
public class SessionResponse
{
    public string checkoutSessionToken;
    public string purchaseId;
    public string url;
}
}