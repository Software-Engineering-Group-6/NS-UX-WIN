using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsSpotify;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NS_UX_WIN_Unit_Tests
{
    [TestClass]
    public class DataFormUnitTest
    {
        [TestMethod]
        public void test_populate_tracks_with_news()
        {
            String mock_json_input = Properties.Resources.mock_json_input_with_news;
            // our list of Tracks
            List<Track> tracks = new List<Track>();
            // our list of News
            List<News> news = new List<News>();
            // holds the deserialized JSON
            List<Dictionary<String, JObject>> news_track;

            // parse JSON input
            news_track = JsonConvert.DeserializeObject<List<Dictionary<String, JObject>>>(mock_json_input);

            // go through each entry and build the corresponding objects
            foreach (var entry in news_track)
            {
                tracks.Add(entry["track"].ToObject<Track>());
                news.Add(entry["news"].ToObject<News>());
            }
            // make sure the size of tracks and news is correct
            Assert.AreEqual(tracks.Count, 10);
            Assert.AreEqual(news.Count, 10);
        }

        [TestMethod]
        public void test_populate_tracks()
        {
            String mock_json_input = Properties.Resources.mock_json_input_without_news;
            // our list of Tracks
            List<Track> tracks = new List<Track>();
            // holds the deserialized JSON
            List<Dictionary<String, JObject>> news_track;

            // parse JSON input
            news_track = JsonConvert.DeserializeObject<List<Dictionary<String, JObject>>>(mock_json_input);

            // go through each entry and build the corresponding objects
            foreach (var entry in news_track)
            {
                tracks.Add(entry["track"].ToObject<Track>());
            }
            // make sure the size of tracks and news is correct
            Assert.AreEqual(tracks.Count, 5);
        }

        [TestMethod]
        public void test_list_to_multiline()
        {
            String[] test_list = { "hello", "unit", "tests" };
            DataForm df = new DataForm();
            String result = df.list_to_multiline(new List<String>(test_list));
            // check the result is equal to what we expect
            Assert.AreEqual(result, "hello\nunit\ntests");
        }
    }
}
