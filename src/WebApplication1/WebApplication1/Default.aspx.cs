using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using LinqToTwitter;

namespace WebApplication1
{
    public partial class _Default : Page
        
    {
        //Count Tweet
        int countKebersihan, countTaman, countPendidikan, countPerhubungan, countSosial, countUnknown;
        //KMP
        public static int[] computeFail(String pattern)
        {
            if (pattern.Length > 0) { 
                int[] fail = new int[pattern.Length];
                fail[0] = 0;
                int m = pattern.Length;
                int j = 0;
                int i = 1;

                while (i < m)
                {
                    if (pattern[j] == pattern[i])
                    {
                        fail[i] = j + 1;
                        i++;
                        j++;
                    }
                    else if (j > 0)
                    {
                        j = fail[j - 1];
                    }
                    else
                    {
                        fail[i] = 0;
                        i++;
                    }
                }
                return fail;
            }
            else
            {
                return null;
            }
            
        }

        public static int kmpMatch(String text, String pattern)
        {

            int n = text.Length;
            int m = pattern.Length;
            if (m > 0)
            {
                int[] fail = computeFail(pattern);
                int i = 0;
                int j = 0;

                while (i < n)
                {
                    if (pattern[j] == text[i])
                    {
                        if (j == m - 1)
                        {
                            return (i - m + 1);
                        }
                        i++;
                        j++;
                    }
                    else if (j > 0)
                    {
                        j = fail[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return -1;
        }

        //BM
        public static int[] buildLast(String pattern)
        {
            int[] last = new int[256];
            int i;
            for (i = 0; i < 256; i++)
            {
                last[i] = -1;
            }

            for (i = 0; i < pattern.Length; i++)
            {
                last[(byte)pattern[i]] = i;
            }

            return last;
        }

        public static int bmMatch(String text, String pattern)
        {
            int[] last = buildLast(pattern);
            int n = text.Length;
            int m = pattern.Length;
            int i = m - 1;
            if (m > 0)
            {
                if (i > n - 1)
                {
                    return -1;
                }
                int j = m - 1;
                do
                {
                    if (pattern[j] == text[i])
                    {
                        if (j == 0)
                        {
                            return i;
                        }
                        else
                        {
                            i--;
                            j--;
                        }
                    }
                    else
                    {
                        int lo = last[(byte)text[i]];
                        i = i + m - Math.Min(j, 1 + lo);
                        j = m - 1;
                    }
                } while (i <= n - 1);
            }
            return -1;
        }

        //TWITTER API
        private SingleUserAuthorizer authorizer =
        new SingleUserAuthorizer
        {
            CredentialStore =
            new SingleUserInMemoryCredentialStore
            {
                ConsumerKey =
                  "YnkxXqhFPIFEc6U3OL9n4XEr9",
                ConsumerSecret =
                 "DX92M5YFsfreLb2t3GairRyKcJ9wWVLS1MbbG6GKUVaIx1JqOL",
                AccessToken =
                 "583003254-Ltb76zr5ebc2yOoupZC7xynVUFOpqDvOqCsIx9Wx",
                AccessTokenSecret =
                 "ExevhdwONuJChzAAYgjAj4qU9xmgFztNp5BoalLU05aJE"
            }
        };
        

        private List<Status> SearchTwitter(string searchTerm)
        {
            var twitterContext = new TwitterContext(authorizer);
            var srch = Enumerable.SingleOrDefault((from search in twitterContext.Search
                                                   where search.Type == SearchType.Search &&
                                                   search.Query == searchTerm && search.Count == 200
                                                   select search));
            if (srch != null && srch.Statuses.Count > 0)
            {
                
                return srch.Statuses;
            }

            return new List<Status>();
        }

        public void seekTwit(object sender, EventArgs e)
        {
            var results = SearchTwitter(keyTwit.Text);
            results = SearchTwitter(keyTwit.Text);

            //Memisahkan Keyword
            String[] kebersihan = (keyKebersihan.Text).Split(',',' ');
            String[] pertamanan = (keyPertamanan.Text).Split(',',' ');
            String[] pendidikan = (keyPendidikan.Text).Split(',',' ');
            String[] perhubungan = (keyPerhubungan.Text).Split(',',' ');
            String[] sosial = (keySosial.Text).Split(',');
            String[] listplace = new String[200];
            String[] arraytweet = new String[200];
            String[] nama = new String[200];
            ulong[] arrayid = new ulong[200];
            int[,] M = new int[200,5];
            int i = 0;

            foreach (var tweet in results)
            {
                arraytweet[i] = tweet.Text.ToString();
                nama[i] = tweet.User.ScreenNameResponse;
                arrayid[i] = tweet.StatusID;
                i++;
            }
            
            dinasKebersihan.Text = "";
            dinasPendidikan.Text = "";
            dinasPerhubungan.Text = "";
            dinasPertamanan.Text = "";
            dinasSosial.Text = "";
            dinasUnknown.Text = "";
            lokasi.Text = "";


            int k=0;
            for (i=0;i<arraytweet.Length;i++)
            {
                if (arraytweet[i] != null)
                {
                    String[] temp = arraytweet[i].Split(' ');
                    for (int a = 0; a < temp.Length; a++)
                    {
                        if (temp[a] == "di")
                        {
                            listplace[k] = temp[a + 1];
                            k++;
                        }
                    }
                }
            }
            
            if (algoritma.SelectedItem.Value == "kmp")
            {
                countKebersihan = 0;
                countTaman = 0;
                countPendidikan = 0;
                countPerhubungan = 0;
                countSosial = 0;
                countUnknown = 0;
                
                for (i = 0; i < arraytweet.Length; i++)
                {
                   
                  
                    //kebersihan
                    int j = 0; bool found = false;
                    if (arraytweet[i] != null)
                    {
                        
                        if (kebersihan.Length > 0)
                        {
                            while (j < kebersihan.Length && found == false)
                            {

                                if (kmpMatch(arraytweet[i].ToLower(), kebersihan[j].ToLower()) != -1)
                                {
                                    M[i, 0] = kmpMatch(arraytweet[i].ToLower(), kebersihan[j].ToLower());
                                    found = true;
                                    
                                }
                                else
                                {
                                    M[i, 0] = -1;
                                    j++;
                                    
                                }
                            }
                        }
                        //pertamanan
                        j = 0; found = false;
                        if (pertamanan.Length > 0)
                        {
                        
                            while (j < pertamanan.Length && found == false)
                            {
                                if (kmpMatch(arraytweet[i].ToLower(), pertamanan[j].ToLower()) != -1)
                                {
                                    M[i, 1] = kmpMatch(arraytweet[i].ToLower(), pertamanan[j].ToLower());
                                    found = true;
                                }
                                else
                                {
                                    M[i, 1] = -1;
                                    j++;
                                }
                            }
                        }
                        //pendidikan
                        j = 0; found = false;
                        if (pendidikan.Length > 0)
                        {
                            while (j < pendidikan.Length && found == false)
                            {
                                if (kmpMatch(arraytweet[i].ToLower(), pendidikan[j].ToLower()) != -1)
                                {
                                    M[i, 2] = kmpMatch(arraytweet[i].ToLower(), pendidikan[j].ToLower());
                                    found = true;
                                }
                                else
                                {
                                    M[i, 2] = -1;
                                    j++;
                                }
                            }
                        }
                        //perhubungan
                        j = 0; found = false;
                        if (perhubungan.Length > 0)
                        {
                            while (j < perhubungan.Length && found == false)
                            {
                                if (kmpMatch(arraytweet[i].ToLower(), perhubungan[j].ToLower()) != -1)
                                {
                                    M[i, 3] = kmpMatch(arraytweet[i].ToLower(), perhubungan[j].ToLower());
                                    found = true;
                                }
                                else
                                {
                                    M[i, 3] = -1;
                                    j++;
                                }
                            }
                        }
                        //sosial
                        j = 0; found = false;
                        if (sosial.Length > 0)
                        {
                            while (j < sosial.Length && found == false)
                            {
                                if (kmpMatch(arraytweet[i].ToLower(), sosial[j].ToLower()) != -1)
                                {
                                    M[i, 4] = kmpMatch(arraytweet[i].ToLower(), sosial[j].ToLower());
                                    found = true;
                                }
                                else
                                {
                                    M[i, 4] = -1;
                                    j++;
                                }
                            }
                        }
                    }
                }

                for (i=0;i<200;i++)
                {
                    if (arraytweet[i] != null)
                    {
                        bool found = false; int j = 0;
                        while (found == false && j < 5)
                        {
                            if (M[i, j] != -1)
                            {
                                found = true;
                            }
                            else
                            {
                                j++;
                            }
                        }

                        if (found != false) //ada selain -1
                        {
                            int min = 99999;
                            int jmin = -1;
                            for (j = 0; j < 5; j++)
                            {
                                if (M[i, j] < min && M[i, j] != -1)
                                {
                                    min = M[i, j];
                                    jmin = j;
                                }
                            }
                            if (jmin == 0)
                            {
                                dinasKebersihan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "< a href=https://twitter.com/statuses/" + arrayid[i] +  ">link</a> <br>" ;
                                countKebersihan++;
                            }
                            else if (jmin == 1)
                            {
                                dinasPertamanan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "< a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>";
                                countTaman++;
                            }
                            else if (jmin == 2)
                            {
                                dinasPendidikan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "< a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>";
                                countPendidikan++;
                            }
                            else if (jmin == 3)
                            {
                                dinasPerhubungan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "< a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>";
                                countPerhubungan++;
                            }
                            else if (jmin == 4)
                            {
                                dinasSosial.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "< a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>";
                                countSosial++;
                            }
                        } else
                        {
                            dinasUnknown.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + " <a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>";
                            countUnknown++;
                        }
                        
                    }
                       
                }

                dinasKebersihan.Text += "Jumlah Tweet : " + countKebersihan + "<br>";
                dinasPendidikan.Text += "Jumlah Tweet : " + countPendidikan + "<br>";
                dinasPertamanan.Text += "Jumlah Tweet : " + countTaman + "<br>";
                dinasSosial.Text += "Jumlah Tweet : " + countSosial + "<br>";
                dinasPerhubungan.Text += "Jumlah Tweet : " + countPerhubungan + "<br>";
                dinasUnknown.Text += "Jumlah Tweet : " + countUnknown + "<br>";

            } else if (algoritma.SelectedItem.Value == "boyermor")
            {
                countKebersihan = 0;
                countTaman = 0;
                countPendidikan = 0;
                countPerhubungan = 0;
                countSosial = 0;
                countUnknown = 0;
          
                for (i = 0; i < arraytweet.Length; i++)
                {
                   
                    
                    
                    //kebersihan
                    int j = 0; bool found = false;
                    if (arraytweet[i] != null)
                    {
                        
                        if (kebersihan.Length > 0)
                        {
                            while (j < kebersihan.Length && found == false)
                            {

                                if (bmMatch(arraytweet[i].ToLower(), kebersihan[j].ToLower()) != -1)
                                {
                                    M[i, 0] = bmMatch(arraytweet[i].ToLower(), kebersihan[j].ToLower());
                                    found = true;
                          
                                }
                                else
                                {
                                    M[i, 0] = -1;
                                    j++;
                                }
                            }
                        }
                        //pertamanan
                        j = 0; found = false;
                        if (pertamanan.Length > 0)
                        {

                            while (j < pertamanan.Length && found == false)
                            {
                                if (bmMatch(arraytweet[i].ToLower(), pertamanan[j].ToLower()) != -1)
                                {
                                    M[i, 1] = bmMatch(arraytweet[i].ToLower(), pertamanan[j].ToLower());
                                    found = true;
                                    
                                }
                                else
                                {
                                    M[i, 1] = -1;
                                    j++;
                                }
                            }
                        }
                        //pendidikan
                        j = 0; found = false;
                        if (pendidikan.Length > 0)
                        {
                            while (j < pendidikan.Length && found == false)
                            {
                                if (bmMatch(arraytweet[i].ToLower(), pendidikan[j].ToLower()) != -1)
                                {
                                    M[i, 2] = bmMatch(arraytweet[i].ToLower(), pendidikan[j].ToLower());
                                    found = true;
                        
                                }
                                else
                                {
                                    M[i, 2] = -1;
                                    j++;
                                }
                            }
                        }
                        //perhubungan
                        j = 0; found = false;
                        if (perhubungan.Length > 0)
                        {
                            while (j < perhubungan.Length && found == false)
                            {
                                if (bmMatch(arraytweet[i].ToLower(), perhubungan[j].ToLower()) != -1)
                                {
                                    M[i, 3] = bmMatch(arraytweet[i].ToLower(), perhubungan[j].ToLower());
                                    found = true;
                               
                                }
                                else
                                {
                                    M[i, 3] = -1;
                                    j++;
                                }
                            }
                        }
                        //sosial
                        j = 0; found = false;
                        if (sosial.Length > 0)
                        {
                            while (j < sosial.Length && found == false)
                            {
                                if (bmMatch(arraytweet[i].ToLower(), sosial[j].ToLower()) != -1)
                                {
                                    M[i, 4] = bmMatch(arraytweet[i].ToLower(), sosial[j].ToLower());
                                    found = true;
                             
                                }
                                else
                                {
                                    M[i, 4] = -1;
                                    j++;
                                }
                            }
                        }
                    }
                }

                

                for (i = 0; i < 200; i++)
                {
                    if (arraytweet[i] != null)
                    {
                        bool found = false; int j = 0;
                        while (found == false && j < 5)
                        {
                            if (M[i, j] != -1)
                            {
                                found = true;
                            }
                            else
                            {
                                j++;
                            }
                        }

                        if (found != false) //ada selain -1
                        {
                            int min = 99999;
                            int jmin = -1;
                            for (j = 0; j < 5; j++)
                            {
                                if (M[i, j] < min && M[i, j] != -1)
                                {
                                    min = M[i, j];
                                    jmin = j;
                                }
                            }
                            if (jmin == 0)
                            {
                                dinasKebersihan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "<a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>"; ;
                                countKebersihan++;
                            }
                            else if (jmin == 1)
                            {
                                dinasPertamanan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "<a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>"; ;
                                countTaman++;
                            }
                            else if (jmin == 2)
                            {
                                dinasPendidikan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "<a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>"; ;
                                countPendidikan++;
                            }
                            else if (jmin == 3)
                            {
                                dinasPerhubungan.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "<a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>"; ;
                                countPerhubungan++;
                            }
                            else if (jmin == 4)
                            {
                                dinasSosial.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "<a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>"; ;
                                countSosial++;
                            }
                        }
                        else
                        {
                            dinasUnknown.Text += "<p>" + nama[i] + " : " + TwitterExtensions.TextAsHtml(arraytweet[i]) + "<a href=https://twitter.com/statuses/" + arrayid[i] + ">link</a> <br>"; ;
                            countUnknown++;
                        }
                        
                    }

                }
                dinasKebersihan.Text += "Jumlah Tweet : " + countKebersihan + "<br>";
                dinasPendidikan.Text += "Jumlah Tweet : " + countPendidikan + "<br>";
                dinasPertamanan.Text += "Jumlah Tweet : " + countTaman + "<br>";
                dinasSosial.Text += "Jumlah Tweet : " + countSosial + "<br>";
                dinasPerhubungan.Text += "Jumlah Tweet : " + countPerhubungan + "<br>";
                dinasUnknown.Text += "Jumlah Tweet : " + countUnknown + "<br>";
            }
            int b = 0;
            String urlGoogleMap = "<a href = https://www.google.com/maps?q=";
            while (listplace[b] != null)
            {
                lokasi.Text += urlGoogleMap + listplace[b] + ">" + listplace[b] + "</a><br>";

                b++;
            }
        }
    }

    public static class TwitterExtensions
    {
        private static readonly Regex _parseUrls = new Regex("\\b(([\\w-]+://?|www[.])[^\\s()<>]+(?:\\([\\w\\d]+\\)|([^\\p{P}\\s]|/)))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _parseMentions = new Regex("(^|\\W)@([A-Za-z0-9_]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _parseHashtags = new Regex("[#]+[A-Za-z0-9-_]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Parse Status Text to HTML equivalent
        /// </summary>
        /// <param name="status">The LinqToTwitter <see cref="Status"/></param>
        /// <returns>Formatted HTML string</returns>
        public static string TextAsHtml(this String temp)
        {
            string tweetText = temp;

            if (!String.IsNullOrEmpty(tweetText))
            {
                // Replace URLs
                foreach (var urlMatch in _parseUrls.Matches(tweetText))
                {
                    Match match = (Match)urlMatch;
                    tweetText = tweetText.Replace(match.Value, String.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", match.Value));
                }

                // Replace Mentions
                foreach (var mentionMatch in _parseMentions.Matches(tweetText))
                {
                    Match match = (Match)mentionMatch;
                    if (match.Groups.Count == 3)
                    {
                        string value = match.Groups[2].Value;
                        string text = "@" + value;
                        tweetText = tweetText.Replace(text, String.Format("<a href=\"http://twitter.com/{0}\" target=\"_blank\">{1}</a>", value, text));
                    }
                }

                // Replace Hash Tags
                foreach (var hashMatch in _parseHashtags.Matches(tweetText))
                {
                    Match match = (Match)hashMatch;
                    string query = Uri.EscapeDataString(match.Value);
                    tweetText = tweetText.Replace(match.Value, String.Format("<a href=\"http://search.twitter.com/search?q={0}\" target=\"_blank\">{1}</a>", query, match.Value));
                }
            }

            return tweetText;
        }
    }

    /* //BAWAAN DARI ASP
     protected void Page_Load(object sender, EventArgs e)
     {

     }
     */
}