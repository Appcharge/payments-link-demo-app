using System;
using System.Collections.Generic;

namespace Appcharge.Models
{
    [Serializable]
    public class Customer
    {
        public string id;
        public string email;
    }

    [Serializable]
    public class PriceDetails
    {
        public int price;
        public string currency;
    }

    [Serializable]
    public class Offer
    {
        public string name;
        public string sku;
        public string assetUrl;
        public string description;
    }

    [Serializable]
    public class Item
    {
        public string name;
        public string assetUrl;
        public string sku;
        public string quantityDisplay;
        public int quantity;
    }

    [Serializable]
    public class SessionRequest
    {
        public Customer customer;
        public PriceDetails priceDetails;
        public Offer offer;
        public List<Item> items;
        public string redirectUrl;
    }
}