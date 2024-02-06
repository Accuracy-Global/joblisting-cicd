using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace JobPortal.Payments
{
    public class Card
    {
        //http://www.regular-expressions.info/creditcard.html
        //# Visa: ^4[0-9]{12}(?:[0-9]{3})?$ All Visa card numbers start with a 4. New cards have 16 digits. Old cards have 13.
        //# MasterCard:  All MasterCard numbers start with the numbers 51 through 55. All have 16 digits.
        //# American Express: ^3[47][0-9]{13}$ American Express card numbers start with 34 or 37 and have 15 digits.
        //# Diners Club: ^3(?:0[0-5]|[68][0-9])[0-9]{11}$ Diners Club card numbers begin with 300 through 305, 36 or 38. All have 14 digits. There are Diners Club cards that begin with 5 and have 16 digits. These are a joint venture between Diners Club and MasterCard, and should be processed like a MasterCard.
        //# Discover: ^6(?:011|5[0-9]{2})[0-9]{12}$ Discover card numbers begin with 6011 or 65. All have 16 digits.
        //# JCB: ^(?:2131|1800|35\d{3})\d{11}$ JCB 

        public const String AMEXPattern = @"^3[47][0-9]{13}$";
        public const String MasterCardPattern = @"^5[1-5][0-9]{14}$";
        public const String VisaCardPattern = @"^4[0-9]{12}(?:[0-9]{3})?$";
        public const String DinersClubCardPattern = @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$";
        public const String enRouteCardPattern = @"^(2014|2149)";
        public const String DiscoverCardPattern = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
        public const String JCBCardPattern = @"^(?:2131|1800|35\d{3})\d{11}$";

        protected NameValueCollection _patterns;
        public NameValueCollection CardPatterns
        {
            get
            {
                if (this._patterns == null)
                {
                    this._patterns = new NameValueCollection();
                    this._patterns.Add("AMEX", AMEXPattern);
                    this._patterns.Add("MasterCard", MasterCardPattern);
                    this._patterns.Add("Visa", VisaCardPattern);
                    this._patterns.Add("DinersClub", DinersClubCardPattern);
                    this._patterns.Add("enRoute", enRouteCardPattern);
                    this._patterns.Add("Discover", DiscoverCardPattern);
                    this._patterns.Add("JCB", JCBCardPattern);

                }
                return this._patterns;
            }
            set
            {
                this._patterns = value;
            }
        }

        public Card()
        {
        }

        public String GetCardType(String cardNumber)
        {
            String cardType = "Unknown";
            try
            {
                String cardNum = cardNumber.Replace(" ", "").Replace("-", "");
                Regex regex;
                foreach (String cardTypeName in this.CardPatterns.Keys)
                {
                    regex = new Regex(this.CardPatterns[cardTypeName]);
                    if (regex.IsMatch(cardNum))
                    {
                        cardType = cardTypeName;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return cardType;
        }
    }
}
