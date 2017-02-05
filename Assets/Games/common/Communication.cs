using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using WWWDic = System.Collections.Generic.Dictionary<string, string>;

namespace PSApi
{
    /// <summary>
    /// This class handles the communication between the server
    /// and the mobile application
    /// </summary>
    public class Communication : Singleton<Communication>
    {
        protected Communication() { }
        /* lambda function to quotes string */
        public delegate string sdels(string s);
        #region STATIC JSON Tags
        private static string TAB = "\t";
        private static string CLOSE = "}";
        private static string OPEN = "{";
        private static string EQ = " : ";
        private static string RET = "\n";
        private static string NEXT = ",";
        private static string OPENA = "[";
        private static string CLOSEA = "]";
        #endregion

        /// <summary>
        /// This structure contains the routes of the PlayarShop API
        /// </summary>
        struct Config
        {
            public static string baseUrl = "http://api.playarshop.com/";

            public static string urlScore = baseUrl + "scores";
            public static string urlDiscount = baseUrl + "discounts";
            public static string urlLogin = baseUrl + "players/sign_in";
            public static string urlRegister = baseUrl + "players/sign_up";
            public static string urlUser = baseUrl + "users";
            public static string urlTarget = baseUrl + "games/";
            public static string passwdReset = baseUrl + "email";
        }

        #region exceptions

        /// <summary>
        /// Exception raised if an error occured during the login
        /// </summary>
        public class InvalidCredentials : System.Exception
        {
            public InvalidCredentials()
            {
            }
            public InvalidCredentials(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// Exception raised on a 500 status
        /// </summary>
        public class ServerError : System.Exception
        {
            public ServerError()
            {

            }
            public ServerError(string message) : base(message)
            {

            }
        }

        #endregion

        #region Models

        public class CustomisationModel
        {
            #region fields
            public struct Data
            {
                public string color1;
                public string color2;
                public string custom;
                public string logo;
            }
            public Data data;
            #endregion

            #region JSON serialisation

            static public sdels Q = x => "\"" + x + "\"";
            /// <summary>
            /// The function serialize the class into JSON.
            /// </summary>
            /// <returns>a string which contains the JSON (pretty)</returns>
            public string toJSON()
            {
                string r = Q("customization") + EQ + OPEN + RET;

                r += TAB + Q("color1") + EQ + Q(this.data.color1) + NEXT + RET;
                r += TAB + Q("color2") + EQ + Q(this.data.color2) + NEXT + RET;
                r += TAB + Q("logo") + EQ + Q(this.data.logo) + NEXT + RET;
                r += TAB + Q("custom") + EQ + Q(this.data.custom) + RET;

                r += CLOSE;

                return r;
            }
            
            #endregion
        }

        public class GenericResponseHandling
        {
            #region fields
            public struct Data
            {
                public string message;
                public string status;
            }
            public Data data;
            #endregion

            public int responseHandle(string t)
            {
                data.message = t;
                return 0;
            }
        }

        /// <summary>
        /// Represents the User Model in the postgresql bdd
        /// </summary>
        public class UserModel
        {
            #region fields
            public struct Profile
            {
                public string email;
                public string location;
                public string username;
            }
            public Profile profile;

            public string auth_token = "";
            
            #endregion fields

            #region JSON serialization

            static public sdels Q = x => "\"" + x + "\"";
            /// <summary>
            /// The function serialize the class into JSON.
            /// </summary>
            /// <returns>a string which contains the JSON (pretty)</returns>
            public string toJSON()
            {
                string r = Q("user") + EQ + OPEN + RET;

                r += TAB + Q("profile") + " : " + OPEN + RET;

                r += TAB + TAB + Q("email") + EQ + Q(this.profile.email) + RET;
                r += TAB + TAB + Q("adress") + EQ + Q(this.profile.location) + RET;
                r += TAB + TAB + Q("user_name") + EQ + Q(this.profile.username) + RET;
                r += CLOSE + NEXT + RET;

                r += TAB + Q("auth_token") + EQ + Q(this.auth_token) + RET;

                r += CLOSE;

                return r;
            }

            /* This function return the correct json to send to the API */
            static public string toJSONUpdate(string email, string password, string username, string location)
            {
                string r = OPEN + RET;

                r += TAB + Q("email") + EQ + Q(email) + NEXT + RET;
                if (password.Length > 0)
                {
                    r += TAB + Q("password") + EQ + Q(password) + NEXT + RET;
                }
                r += TAB + Q("user_name") + EQ + Q(username) + NEXT + RET;
                r += TAB + Q("adress") + EQ + Q(location) + RET;

                return r + CLOSE;
            }
            static public string toJSONChangeInfos(string e, string p, string f, string l, string a)
            {
                string r = "";

                r += OPEN + RET;

                r += TAB + Q("email") + EQ + Q(e) + NEXT + RET;
                r += TAB + Q("password") + EQ + Q(p) + NEXT + RET;
                r += CLOSE;

                return r;
            }


            /* This function return the correct json to send to the API */
            static public string toJSONAuth(string e, string p)
            {
                string r = "";

                r += OPEN + RET;
                
                r += TAB + Q("email") + EQ + Q(e) + NEXT + RET;
                r += TAB + Q("password") + EQ + Q(p) + RET;

                r += CLOSE;

                return r;
            }

            static public string toJSONPasswdReset(string e)
            {
                string r = "";

                r += OPEN + RET;
                
                r += TAB + Q("email") + EQ + Q(e) + RET;
                
                r += CLOSE;

                return r;
            }

            /* This function return the correct json to send to the API */
            static public string toJSONRegister(string e, string p)
            {
                string r = "";

                r += OPEN + RET;
                
                r += TAB + Q("email") + EQ + Q(e) + NEXT + RET;
                r += TAB + Q("password") + EQ + Q(p) + RET;
                
                r += CLOSE;
                
                return r;
            }
            #endregion

            #region JSON deserialization

            /// <summary>
            /// This function fetch the user informations returned by the API
            /// and completes the class with the data
            /// </summary>
            /// <param name="raw">Raw JSON returned by the API</param>
            /// <returns></returns>
            public int fetchUser(string raw)
            {
                JSONNode N = JSON.Parse(raw);
               
                try
                {              
                    // Fetch authentification data
                    this.profile.email = N["email"];
                   
                    // Fetch profile data
                    this.profile.username = (string)N["user_name"] == null ? "" : (string)N["user_name"];
                    this.profile.location = (string)N["adress"] == null ? "" : (string)N["adress"];
                }
                catch (System.Exception)
                {
                    throw new ServerError();
                }
                
                return 0;
            }

            public int fetchToken(string raw)
            {
                Debug.Log("FetchToken");
                try
                {
                    JSONNode N = JSON.Parse(raw);
                    this.auth_token = "Auth-Basic " + N["auth_token"];
                    Debug.Log("Just set the token to :" + this.auth_token);
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }

                return 0;
            }

            #endregion
        }

        public class DiscountModel
        {
            #region fields
            public struct Data
            {
                public string name;
                public string success;
                public string created_a;
            }
            public Data[] data;
            #endregion

            #region JSON serialization
            static public sdels Q = x => "\"" + x + "\"";
            /// <summary>
            /// This function returns our model as JSON
            /// </summary>
            /// <returns></returns>
            public string toJSON()
            {
                string r = Q("scores") + EQ + OPEN + RET;

                r += OPENA + RET;

                foreach (Data e in this.data)
                {
                    r += TAB + OPEN + RET;
                    r += TAB + Q("name") + EQ + Q(e.name) + NEXT + RET;
                    r += TAB + Q("created_at") + EQ + Q(e.created_a) + RET;
                    r += TAB + Q("score") + EQ + Q(e.success) + RET;
                    r += CLOSE + NEXT + RET;
                }

                r += CLOSE;

                return r;
            }
            #endregion           

            public int fetchDiscount(string raw)
            {
                Debug.Log("FetchDiscount");
                Debug.Log(raw);
                try
                {
                    JSONNode N = JSON.Parse(raw);
                    JSONArray S = N["discount"].AsArray;

                    this.data = new Data[S.Count];

                    int i = 0;
                    foreach (JSONNode E in S)
                    {
                        // Fetch score response
                        this.data[i].name = E["name"];
                        this.data[i].success = E["success"];
                        this.data[i].created_a = E["created_at"];
                        i++;
                    }
                }
                catch (System.Exception e)
                {
                    throw new ServerError();
                }
                return 0;
            }

        }

        public class ScoreModel
        {
            #region fields
            public struct Data
            {
                public string name;
                public string score;
                public string created_a;
            }
            public Data[] data;

            public string result;
            #endregion
            
            #region JSON serialization
            static public sdels Q = x => "\"" + x + "\"";
            /// <summary>
            /// This function returns our model as JSON
            /// </summary>
            /// <returns></returns>
            public string toJSON()
            {
                string r = Q("scores") + EQ + OPEN + RET;

                r += OPENA + RET;
                
                foreach (Data e in this.data)
                {
                    r += TAB + OPEN + RET;
                    r += TAB + Q("name") + EQ + Q(e.name) + NEXT + RET;
                    r += TAB + Q("created_at") + EQ + Q(e.created_a) + NEXT + RET;
                    r += TAB + Q("score") + EQ + Q(e.score) + RET;
                    r += CLOSE + NEXT + RET;                    
                }
                
                r += CLOSE;

                return r;
            }
            #endregion


            static public string toJSON(string g, string t, string s)
            {
                string r = "";

                r += OPEN + RET;
                
                r += TAB + Q("game_id") + EQ + Q(g) + NEXT + RET;
                r += TAB + Q("target_id") + EQ + Q(t) + NEXT + RET;
                r += TAB + Q("score")  + EQ + Q(s)+ RET;

                r += CLOSE;

                return r;
            }

            public int fetchResult(string raw)
            {
                try
                {
                    JSONNode N = JSON.Parse(raw);
                    this.result = N["reduction"];
                }
                catch (System.Exception e)
                {
                    throw new ServerError();
                }
                return 0;

            }

            public int fetchScore(string raw)
            {
                Debug.Log("RAW data");
                Debug.Log(raw);
                try
                {
                    JSONNode N = JSON.Parse(raw);
                    JSONArray S = N["scores"].AsArray;

                    Debug.Log("SCORES!");
                    Debug.Log(S.ToString());

                    this.data = new Data[ S.Count ];
                    
                    int i = 0;
                    foreach (JSONNode E in S)
                    {
                        // Fetch score response
                        this.data[i].name = E["name"];
                        this.data[i].score = E["score"];
                        this.data[i].created_a = E["created_at"];
                        i++;
                    }

                    Debug.Log("TOJSON?");
                    Debug.Log(this.toJSON());
                }
                catch (System.Exception e)
                {
                    throw new ServerError();
                }
                return 0;
            }
        }

        public struct TargetData
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string Id  { get; set; }
        }

        /// <summary>
        /// Represents the Target Model in the postgresql
        /// </summary>
        public class TargetModel
        {
            public TargetModel()
            {
                this.customization = new CustomisationModel();
                this.targets = new List<TargetData>();
            }

            #region fields

            private CustomisationModel customization;
            public  CustomisationModel Customization
            {
                get
                {
                    return customization;
                }
            }

            private int games;
            public int Games
            {
                get
                {
                    return this.games;
                }
            }

            private List<TargetData> targets;
            public List<TargetData> Targets
            {
                get
                {
                    return this.targets;
                }
            }

            public string game_id;
            private string name;

            private string id;
            public string Id
            {
                get
                {
                    return this.id;
                }
            }

            #endregion

            #region JSON serialization
            static public sdels Q = x => "\"" + x + "\"";
            /// <summary>
            /// This function returns our model as JSON
            /// </summary>
            /// <returns></returns>
            public string toJSON()
            {
                string r = Q("target") + EQ + OPEN + RET;

                r += this.Customization.toJSON() + NEXT + RET;

                r += TAB + Q("games") + EQ + Q(this.games.ToString()) + NEXT + RET;
                r += TAB + Q("name") + EQ + Q(this.name) + RET;

                r += CLOSE;

                return r;
            }
            #endregion

            #region JSON deserialization
            /// <summary>
            /// This functions parses raw json and put it into our model
            /// </summary>
            /// <param name="raw">Raw JSON</param>
            /// <returns></returns>
            public int fetchTarget(string raw)
            {
                Debug.Log(raw);
                JSONNode N = JSON.Parse(raw);
                JSONNode G = N["game"][0];
                JSONArray T = N["targets"].AsArray;

                foreach (JSONNode i in T)
                {
                    TargetData newData = new TargetData();
                    newData.Name = i["vuforia_name"];
                    newData.Url = Config.baseUrl + i["image"]["url"];
                    newData.Id = i["id"];
                    this.Targets.Add(newData);
                }
                // check response
                try
                {                    
                    this.games = G["ref"].AsInt;
                    Debug.Log("teetetetete");
                    Debug.Log(G);
                    this.game_id = G["id"];
                    this.id = G["id"];
                    this.customization.data.color1 = G["color1"];
                    this.customization.data.color2 = G["color2"];

                    /*temp*/
                    /*if (this.customization.data.color1 == "rouge")
                    {
                        this.customization.data.color1 = "#FF4200";
                    }
                    if (this.customization.data.color2 == "bleu")
                    {
                        this.customization.data.color2 = "#4200FF";
                    }*/
                    this.customization.data.custom = G["custom"];
                    this.customization.data.logo = Config.baseUrl + G["logo"];
                }
                catch (System.Exception e)
                {
                    throw new ServerError();
                }
                Debug.Log(this.toJSON());

                return 0;
            }
             #endregion
    }
        #endregion

        #region Model instances
        /// <summary>
        /// User instance
        /// </summary>
        private UserModel user;
        public UserModel User
        {
            get
            {
                return user;
            }
        }

        private GenericResponseHandling genericHandler;
        public GenericResponseHandling GenericHandler
        {
            get
            {
                return genericHandler;
            }
        }

        private ScoreModel score;
        public ScoreModel Score
        {
            get
            {
                return score;
            }
        }

        private DiscountModel discount;
        public DiscountModel Discount
        {
            get
            {
                return discount;
            }
        }

        /// <summary>
        /// Target instance
        /// </summary>
        private TargetModel target;
        public TargetModel Target
        {
            get
            {
                return target;
            }
        }

        #endregion
 
        #region basic methods

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// This function is automatically called by Unity at the initialization of the module
        /// </summary>
        void Start()
        {
            user = new UserModel();
            target = new TargetModel();
            genericHandler = new GenericResponseHandling();
            score = new ScoreModel();
            discount = new DiscountModel();
        }

        /// <summary>
        /// This function is called once per frame
        /// </summary>
        void Update() { }

        #endregion

        #region api calls

        /// <summary>
        /// This function tries to login the user.
        /// </summary>
        /// <param name="usr">User's username</param>
        /// <param name="pwd">Password in clear</param>
        /// <param name="onComplete">A callback function</param>
        /// <returns>0 Always - In case of failure it'll raise an Exception.</returns>
        public void postLogin(string usr, string pwd, System.Func<int, string, int> onComplete )
        {
            POST(Config.urlLogin, UserModel.toJSONAuth(usr, pwd), user.fetchToken, onComplete);
        }
        
        public void postRegister(string usr, string pwd, System.Func<int, string, int> onComplete)
        {
            POST(Config.urlRegister, UserModel.toJSONRegister(usr, pwd), user.fetchToken, onComplete);
        }

        /// <summary>
        /// Get Target informations such as games
        /// </summary>
        /// <param name="data">Target name</param>
        /// <param name="onComplete">CallBack function</param>
        public void getTarget(string data, System.Func<int, string, int> onComplete)
        {
            Debug.Log("@Call : " + Config.urlTarget + "/" + data);
            GET(Config.urlTarget + "/" + data, target.fetchTarget, onComplete);
        }

        public void getScores(System.Func<int, string, int> onComplete)
        {
            Debug.Log("@Call :" + Config.urlScore);
            GET(Config.urlScore, score.fetchScore, onComplete);
        }

        public void getDiscounts(System.Func<int, string, int> onComplete)
        {
            Debug.Log("@Call :" + Config.urlDiscount);
            GET(Config.urlDiscount, discount.fetchDiscount, onComplete);
        }

        public void fetchMyProfile(System.Func<int, string, int> onComplete)
        {
            GET(Config.urlUser, user.fetchUser, onComplete);
        }
        public void updateMyProfile(string e, string p, string u, string l, System.Func<int, string, int> onComplete)
        {
            POST(Config.urlUser, UserModel.toJSONUpdate(e, p, u, l), null, onComplete);
        }

        public void getUser(System.Func<int, string, int> onComplete)
        {
            Debug.Log("@Call :" + Config.urlUser);
            GET(Config.urlUser, user.fetchUser, onComplete);
        }

        public int postScore(string gid, string sc, System.Func<int, string, int> onComplete)
        {
            Debug.Log("post scores on /scores");
            POST(Config.urlScore, ScoreModel.toJSON(target.Id, target.Targets.ToArray()[0].Id, sc), score.fetchResult, onComplete);
            return 0;
        }

        /*public int postUsersInfos(string e, string p, string f, string l, string a, System.Func<int, string, int> onComplete)
        {
            POST(Config.urlChangeInfosUsers, UserModel.toJSONChangeInfos(e, p, f, l, a), null, onComplete);
            return 0;
        }/*

        /*
         * Get user information (userid = data)
         * GET /api/user/{id user}
         */
        public void getUserInformation(string data, System.Func<int, string, int> onComplete)
        {
            GET(Config.urlUser + data, null, onComplete);
        }
        
        public void resetMyPassword(string e, System.Func<int, string, int> onComplete)
        {
            POST(Config.passwdReset, UserModel.toJSONPasswdReset(e), null, onComplete);
        }
        
        #endregion

        #region tools

        /// <summary>
        /// This function waits for the request to end
        /// If the request don't have any errors then innerAction is called
        /// Otherwise the callback function provide by the user is called and notified
        /// that the request has failed. Server side error.
        /// </summary>
        /// <param name="www">The request</param>
        /// <param name="innerAction">Action to complete with the results of the request</param>
        /// <param name="onComplete">User Callback</param>
        /// <returns></returns>
        private IEnumerator WaitForRequest(WWW www, System.Func<string, int> innerAction, System.Func<int, string, int> onComplete)
        {
            /// wait for request to end
            yield return www;
            /// check errors
            if (www.error == null)
            {
                try
                {
                    if (innerAction != null)
                        innerAction(www.text);
                    if (onComplete != null)
                        onComplete(0, "");
                }
                catch (System.Exception)
                {
                    if (onComplete != null)
                        onComplete(1, "");
                }
            }
            else
            {
                onComplete(1, www.text);
                Debug.Log(www.error);
                Debug.Log(www.text);
            }
        }

        /// <summary>
        /// This function performs a GET request with the correct headers and Authorization token if it exists
        /// </summary>
        /// <param name="url">URL where to perform the GET</param>
        /// <param name="innerAction">Action to complete with the results of the request</param>
        /// <param name="onComplete">User callback</param>
        /// <returns></returns>
        public WWW GET(string url, System.Func<string, int> innerAction, System.Func<int, string, int> onComplete)
        {
            WWWForm form = new WWWForm();

            WWWDic headers = form.headers;
            byte[] rawData = form.data;


            headers["Accept"] = "application/json";
            headers["Content-Type"] = "application/json";
            headers["Authorization"] = user.auth_token;

            Debug.Log("Just doing a get");
            Debug.Log(url);
            Debug.Log(user.auth_token);

            //if (user.auth_token == "")
              //  headers["Authorization"] = "Auth-Basic eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjo1LCJleHAiOjE0NjkyMDI0OTZ9.kJI-rlMGDXbAAs9NgHYRo2gUeieWcyq-BE-_aD-dUGU";

            WWW www = new WWW(url, null, headers);
            StartCoroutine(WaitForRequest(www, innerAction, onComplete));
            return www;
        }

        /// <summary>
        /// This function performs a POST request with the correct headers and Authorization token if it exists
        /// </summary>
        /// <param name="url">URL where to perform the POST</param>
        /// <param name="data"></param>
        /// <param name="innerAction">Action to complete with the results of the request</param>
        /// <param name="onComplete">User callback</param>
        /// <returns></returns>
        public WWW POST(string url, string data, System.Func<string, int> innerAction, System.Func<int, string, int> onComplete)
        {
            Debug.Log(data);
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes(data);
            WWWDic headers = new WWWDic();

            headers["Accept"] = "application/json";
            headers["Content-Type"] = "application/json";

            /// Set the authorization token
            headers["Authorization"] = user.auth_token;
            //if (user.auth_token == "")
              //  headers["Authorization"] = "Auth-Basic eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjo1LCJleHAiOjE0NjkyMDI0OTZ9.kJI-rlMGDXbAAs9NgHYRo2gUeieWcyq-BE-_aD-dUGU"; 
            
            WWW www = new WWW(url, rawData, headers);

            StartCoroutine(WaitForRequest(www, innerAction, onComplete));
            return www;
        }

        #endregion
    }
}