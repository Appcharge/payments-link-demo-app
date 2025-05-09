using System.Collections;
using System.Collections.Generic;
using Appcharge.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Appcharge.Networking
{
    public class HttpRequest
    {
        protected static readonly string PUBLISHER_TOKEN = "[YOUR_PUBLISHER_TOKEN]";
        protected static readonly string CHECKOUT_PUBLIC_KEY = "[YOUR_CHECKOUT_PUBLIC_KEY]";
        protected static readonly string CREATE_SESSION_URL = "[YOUR_CREATE_SESSION_URL]";

        public static IEnumerator CreateSession(bool debugMode, System.Action<string> onSuccess, System.Action<string> onError)
        {
            var itemData = new Item
            {
                name = "Coin Chest",
                assetUrl = "https://media.appcharge.com/media/demos/royalblast_coin_chest.png",
                sku = "coins_xo",
                quantityDisplay = "10000 Coins",
                quantity = 10000
            };

            var sessionRequest = new SessionRequest
            {
                customer = new Customer { id = "John Doe", email = "john@doe.com" },
                priceDetails = new PriceDetails { price = 2490, currency = "USD" },
                offer = new Offer
                {
                    name = "Coin Chest",
                    sku = "best_deal_package",
                    assetUrl = itemData.assetUrl,
                    description = "Coin Chest"
                },
                items = new List<Item> { itemData },
                redirectUrl = "https://your-redirect-url.domain.com" + (debugMode ? "?debug=true" : "")
            };

            string jsonData = JsonUtility.ToJson(sessionRequest, prettyPrint: true);

            using (UnityWebRequest webRequest = new UnityWebRequest(CREATE_SESSION_URL, "POST"))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.method = UnityWebRequest.kHttpVerbPOST;
                webRequest.SetRequestHeader("x-publisher-token", PUBLISHER_TOKEN);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError?.Invoke(webRequest.error);
                }
                else
                {
                    var response = JsonUtility.FromJson<SessionResponse>(webRequest.downloadHandler.text);
                    string returnUrl = string.Format("{0}/{1}?checkout-token={2}", response.url, response.checkoutSessionToken, CHECKOUT_PUBLIC_KEY);
                    onSuccess?.Invoke(returnUrl);
                }
            }
        }
    }
}