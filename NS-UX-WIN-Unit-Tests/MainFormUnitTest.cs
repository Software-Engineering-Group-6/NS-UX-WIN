using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsSpotify;

namespace NS_UX_WIN_Unit_Tests
{
    [TestClass]
    public class MainFormUnitTest
    {
        [TestMethod]
        public void test_show_data()
        {
            // initialize the object
            Form1 form = new Form1();
            form.Show();
            // test data
            String mock_json_input_good = Properties.Resources.mock_json_input_with_news;
            String mock_json_input_bad = @"{{";
            bool result_good = form.show_data(mock_json_input_good, true);
            bool result_bad = form.show_data(mock_json_input_bad, true);
            // assert it's good
            Assert.AreEqual(result_good, true);
            Assert.AreEqual(result_bad, false);
        }
    }
}
