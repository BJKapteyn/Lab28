using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardBuilderLab.Models;
using Newtonsoft.Json.Linq;

namespace CardBuilderLab.Controllers
{
    public class HomeController : Controller
    {
        public bool ShuffledDeck = false;

        public JToken GetDeck()
        {
            int numberOfDecks = 1;
            string URL = $"https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count={numberOfDecks}";
            //send website ping
            HttpWebRequest request = WebRequest.CreateHttp(URL);
            //get response in form of text
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //convert to a streamreader 
            StreamReader rd = new StreamReader(response.GetResponseStream());
            //write to a string in json notation
            string jsonText = rd.ReadToEnd();
            //convert to json token
            JToken j = JToken.Parse(jsonText);

            return j;
        }

        public string GetCards(JToken deck, int numCards)
        {
            string id = deck["deck_id"].ToString();
            string URL = $"https://deckofcardsapi.com/api/deck/{id}/draw/?count={numCards}";

            HttpWebRequest request = WebRequest.CreateHttp(URL);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());

            string APIText = rd.ReadToEnd();

            return APIText;
        }


        public List<Card> CreateCardList(string APIText)
        {
            JToken json = JToken.Parse(APIText);

            List<JToken> jCards = json["cards"].ToList();

            List<Card> Cards = new List<Card>();

            foreach(JToken jcard in jCards)
            {
                Card c = new Card();
                c.Name = (string)jcard["value"];
                c.Image = (string)jcard["image"];
                c.Suit = (string)jcard["suit"];
                c.Code = (string)jcard["code"];
                Cards.Add(c);
            }

            return Cards;
        }

        public ActionResult DrawCards()
        {
            if(!ShuffledDeck)
            {
                Session["Deck"] = GetDeck();
                ShuffledDeck = true;
            }
            //get the current deck
            JToken deck = (JToken)Session["Deck"];


            string jsonCards = GetCards(deck, 5);

            List<Card> cards = CreateCardList(jsonCards);

            return View(cards); 
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}