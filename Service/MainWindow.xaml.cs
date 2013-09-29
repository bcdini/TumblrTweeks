using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Documents;

namespace TumblrTweaksWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyValuePair<string, string> LoginDetails { get; set; }
        public delegate void PlaceFetchHandler(Collection<Post> poiItems = null);

        public PlaceFetchHandler PlaceFetched;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SearchAndDestroyTumbls_Click(object sender, RoutedEventArgs e)
        {
            LoadPosts();
       }

        private void LoadPosts()
        {
            System.Threading.Thread.Sleep(5000);

            OAuth.ConsumerKey = "lQS712YUjSnDppww0NV8EcFn1Y4Ch7p6cSnUB4g11MKt314hJs";
            OAuth.ConsumerSecret = "fXC9IhAMyr91jzS437ycXrw4GdI5fHyI3ZesWCpacZ7zS1EILL";

            LoginDetails = OAuth.XAuthAccess("pooran@live.com", "p00ranpra5ad");

            var postUrl = string.Format("https://api.tumblr.com/v2/blog/pooran.tumblr.com/posts/text?api_key=lQS712YUjSnDppww0NV8EcFn1Y4Ch7p6cSnUB4g11MKt314hJs");
            //string str = OAuth.OAuthData(postUrl, "POST", LoginDetails.Key, LoginDetails.Value, prms);


            WebRequest request = WebRequest.Create(postUrl.ToString());
            request.BeginGetResponse(GetPOIsCallback, request);
        }
        void GetPOIsCallback(IAsyncResult result)
        {
            try
            {
                HttpWebRequest request = result.AsyncState as HttpWebRequest;
                HttpWebResponse response = null;
                Collection<Post> list = new Collection<Post>();
                if (request != null)
                {
                    var responseObject = new RootObject();
                    try
                    {
                        response = (HttpWebResponse)request.EndGetResponse(result);
                        var jsonSerializer = new DataContractJsonSerializer(typeof(RootObject));
                        responseObject = (RootObject)jsonSerializer.ReadObject(response.GetResponseStream());

                        string rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        if (PlaceFetched != null)
                            PlaceFetched(null);
                        return;
                    }
                    List<object> post2delete = new List<object>();
                    List<Reblog> post2repost = new List<Reblog>();
                    foreach (Post item in responseObject.response.posts)
                    {
                        if (item.tags.Count > 0)
                        {
                            foreach (string tag in item.tags)
                            {
                                if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_[0-9]?[0-9]?[0-9]?[0-9]?[0-9]s"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalSeconds > (double.Parse(tag.Substring(1, tag.IndexOf('s') - 1))))
                                    {
                                        post2delete.Add(item.id);
                                    }

                                }
                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_[0-9]?[0-9]?[0-9]?[0-9]?[0-9]m"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalMinutes > (double.Parse(tag.Substring(1, tag.IndexOf('m') - 1))))
                                    {
                                        post2delete.Add(item.id);
                                    }

                                }
                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_[0-9]?[0-9]?[0-9]?[0-9]?[0-9]h"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalHours > (double.Parse(tag.Substring(1, tag.IndexOf('h') - 1))))
                                    {
                                        post2delete.Add(item.id);
                                    }
                                }

                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_[0-9]?[0-9]?[0-9]?[0-9]?[0-9]d"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalDays > (double.Parse(tag.Substring(1, tag.IndexOf('d') - 1))))
                                    {
                                        post2delete.Add(item.id);
                                    }
                                }

                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_r[0-9]?[0-9]?[0-9]?[0-9]?[0-9]s"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalSeconds > (double.Parse(tag.Substring(3, tag.IndexOf('s') - 2))))
                                    {
                                        post2repost.Add(new Reblog() { id = item.id, reblog_key = item.reblog_key });
                                    }
                                }
                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_r[0-9]?[0-9]?[0-9]?[0-9]?[0-9]m"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalMinutes > (double.Parse(tag.Substring(2, tag.IndexOf('m') - 2))))
                                    {
                                        post2repost.Add(new Reblog() { id = item.id, reblog_key = item.reblog_key });
                                    }
                                }
                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_r[0-9]?[0-9]?[0-9]?[0-9]?[0-9]h"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalHours > (double.Parse(tag.Substring(2, tag.IndexOf('h') - 2))))
                                    {
                                        post2repost.Add(new Reblog() { id = item.id, reblog_key = item.reblog_key });
                                    }
                                }
                                else if (System.Text.RegularExpressions.Regex.IsMatch(tag, "_r[0-9]?[0-9]?[0-9]?[0-9]?[0-9]d"))
                                {
                                    DateTime dt = DateTime.Parse(item.date);
                                    System.TimeSpan diffResult = DateTime.Now.Subtract(dt);
                                    if (diffResult.TotalDays > (double.Parse(tag.Substring(2, tag.IndexOf('d') - 2))))
                                    {
                                        post2repost.Add(new Reblog() { id = item.id, reblog_key = item.reblog_key });
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in post2delete)
                    {
                          var prms = new Dictionary<string, object>();
                          prms.Add("id", item.ToString());
            
                          var postUrl = "https://api.tumblr.com/v2/blog/pooran.tumblr.com/post/delete";
                          string str = OAuth.OAuthData(postUrl, "POST", LoginDetails.Key, LoginDetails.Value, prms);
           
                        //KeyValuePair<string,string> keyvalue= OAuth.XAuthAccess("pooran@live.com", "p00ranpra5ad");
                        //var postUrl = string.Format("https://api.tumblr.com/v2/blog/pooran.tumblr.com/post/delete?consumer_key=lQS712YUjSnDppww0NV8EcFn1Y4Ch7p6cSnUB4g11MKt314hJs&consumer_secret=fXC9IhAMyr91jzS437ycXrw4GdI5fHyI3ZesWCpacZ7zS1EILL&token="+keyvalue.Key
                        //    +"&token_secret="+ keyvalue.Value + "&id=" + item.ToString());
                        //WebRequest request2 = WebRequest.Create(postUrl.ToString());
                        //request2.Method = "POST";
                        //request2.GetResponse();

                    }
                    foreach (var item in post2repost)
                    {
                        var postUrl = string.Format("https://api.tumblr.com/v2/blog/pooran.tumblr.com/post/reblog?consumer_key=lQS712YUjSnDppww0NV8EcFn1Y4Ch7p6cSnUB4g11MKt314hJs&consumer_secret=fXC9IhAMyr91jzS437ycXrw4GdI5fHyI3ZesWCpacZ7zS1EILL&token=TPKDdyc8YyF96KMUeSW4hsyBi60DgcD5iifrcmvrn3QmxWigDx&token_secret=YHpiKabYcjCRfEVfu3nadWOfCsIo2GfJDpfTMLpqCMwZ77tI4t&id=" + item.ToString() + "&reblog_key" + item.reblog_key);
                        WebRequest request2 = WebRequest.Create(postUrl.ToString());
                        request2.Method = "POST";
                        request2.GetResponse();
                    }
                }
            }
            catch (Exception)
            {
                    
            }
            finally
            {
                LoadPosts();
            }
        }
    }
    public class Meta
    {
        public int status { get; set; }
        public string msg { get; set; }
    }

    public class Reblog
    {
        public object id { get; set; }
        public string reblog_key { get; set; }
    }
    public class Blog
    {
        public string title { get; set; }
        public string name { get; set; }
        public int posts { get; set; }
        public string url { get; set; }
        public int updated { get; set; }
        public string description { get; set; }
        public bool ask { get; set; }
        public bool ask_anon { get; set; }
        public bool is_nsfw { get; set; }
        public bool share_likes { get; set; }
    }

    public class Post
    {
        public string blog_name { get; set; }
        public object id { get; set; }
        public string post_url { get; set; }
        public string slug { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public int timestamp { get; set; }
        public string state { get; set; }
        public string format { get; set; }
        public string reblog_key { get; set; }
        public List<string> tags { get; set; }
        public string short_url { get; set; }
        public List<object> highlighted { get; set; }
        public int note_count { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class Response
    {
        public Blog blog { get; set; }
        public List<Post> posts { get; set; }
    }

    public class RootObject
    {
        public Meta meta { get; set; }
        public Response response { get; set; }
    }


}
