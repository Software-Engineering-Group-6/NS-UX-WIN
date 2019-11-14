using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewsSpotify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // default selection on combobox is the first item, which is 1
            comboBox1.SelectedIndex = 0;
            // the search terms text box is enabled by default
            textBox1.Enabled = true;
            // the use news checbox is unticked by default
            checkBox1.Checked = false;
            // the excluded sources text box is disabled by default
            textBox2.Enabled = false;
            // do not allow users to resize the form
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            // the ready / building... label setup
            label5.Text = "READY";
            label5.ForeColor = Color.Green;
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // disable search terms text box
                textBox1.Enabled = false;
                // enable excluded terms text box
                textBox2.Enabled = true;
            } else
            {
                // enable search terms text box
                textBox1.Enabled = true;
                // disable excluded sources text box
                textBox2.Enabled = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // we are about to generate tracks
            // get the maximum number of tracks to output
            // note: due to the nature of the combobox, we know
            // for sure that the value of it is an integer, since we
            // have defined the values that go in there ourselves
            String max_track_out = comboBox1.Text;
            // this is to execute the process
            ProcessStartInfo start_info = new ProcessStartInfo();
            start_info.FileName = "ns_main\\ns_main.exe";
            start_info.Arguments = "";
            start_info.UseShellExecute = false;
            start_info.RedirectStandardOutput = true;
            start_info.CreateNoWindow = true;
            start_info.WorkingDirectory = "ns_main";
            Process python_proc = new Process();
            // json input stored here
            String json_input = "";

            // add the max tracks option to the arguments
            start_info.Arguments += "--max=" + max_track_out + " ";

            // ALWAYS add the --json flag because that's how the UX
            // gets the output from the NewsSpotify application
            start_info.Arguments += "--json ";

            // since the user is about to have to wait for ~10 seconds
            // let them know the application is doing something
            label5.Text = "BUILDING...";
            label5.ForeColor = Color.Red;
            // the application will not redraw until we
            // exit the function, so we force it to redraw
            // so the change takes effect before we go into
            // the waiting portion
            this.Refresh();

            // check which type of query we are doing
            if (checkBox1.Checked)
            {
                // we are using news headlines
                // collect excluded sources

                // add the news options to the argument line
                start_info.Arguments += "--news ";

                var excl_sources = new List<String>();
                // add those defined sources which are not whitespace or empty
                foreach (String line in textBox2.Lines)
                {
                    if (!String.IsNullOrWhiteSpace(line)) excl_sources.Add(line);
                }
                // now we build the command and run it
                // write excluded sources to a file in ns_main directory
                System.IO.File.WriteAllLines(@"ns_main\.exclusion", excl_sources.ToArray());
                // add this to the arguments
                start_info.Arguments += " --exclude=.exclusion";
                // set the process information before running it
                python_proc.StartInfo = start_info;
                // run the process
                python_proc.Start();
                while (!python_proc.StandardOutput.EndOfStream)
                {
                    string line = python_proc.StandardOutput.ReadLine();
                    json_input += line;
                }
                python_proc.WaitForExit();
                // we now have the json input
                // call the show_data method
                // with correct parameters
                show_data(json_input, true);
            } else
            {
                // we are using terms input by the user
                // collect the terms
                // set the delimiter chars
                char[] delimiters = {' ', '-', ',', '.', ':', '!', '?', ';'};
                String[] search_terms = textBox1.Text.Split(delimiters);
                var clean_search_terms = new List<String>();
                foreach (String term in search_terms)
                {
                    if (!String.IsNullOrWhiteSpace(term)) clean_search_terms.Add(term);
                }
                // check there are no empty search terms
                if (clean_search_terms.Count < 1)
                {
                    // we cannot continue without search terms,
                    // tell the user
                    MessageBox.Show("You have not provided a valid amount of search terms. Cannot output tracks.",
                        "Fatal Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    // exit
                    label5.Text = "READY";
                    label5.ForeColor = Color.Green;
                    return;
                } else
                {
                    // otherwise, add the search terms to the arguments
                    foreach (string term in clean_search_terms)
                    {
                        start_info.Arguments += " " + term;
                    }
                }
                python_proc.StartInfo = start_info;
                python_proc.Start();
                while (!python_proc.StandardOutput.EndOfStream)
                {
                    string line = python_proc.StandardOutput.ReadLine();
                    json_input += line;
                }
                python_proc.WaitForExit();
                // now we have the json input
                // call the show data method with
                // correct parameters
                show_data(json_input, false);
            }
            // at the end of the process,
            // set the status label back to normal
            label5.Text = "READY";
            label5.ForeColor = Color.Green;
        }

        private void show_data(String j_input, bool has_news)
        {
            // our list of Tracks
            List<Track> tracks = new List<Track>();
            // our list of News
            List<News> news = new List<News>();
            // holds the deserialized JSON
            List<Dictionary<String, JObject>> news_track;

            // parse JSON input
            try
            {
                news_track = JsonConvert.DeserializeObject<List<Dictionary<String, JObject>>>(j_input);
            }
            catch
            {
                // something went wrong in the deserialization
                // which very likely means NewsSpotify threw an error
                // notify the user
                MessageBox.Show("NewsSpotify.exe has returned the following error: " + j_input,
                        "Fata Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                return;
            }
            
            // go through each entry and build the corresponding objects
            foreach (var entry in news_track)
            {
                tracks.Add(entry["track"].ToObject<Track>());
                if (has_news)
                {
                    news.Add(entry["news"].ToObject<News>());
                }
            }
            // we now have the objects we need with their data
            // create the new form and give it our data
            DataForm data_form = new DataForm();
            if (has_news)
            {
                data_form.PopulateTracksWithNews(tracks, news);
            }
            else
            {
                data_form.PopulateTracks(tracks);
            }
            data_form.Show();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
