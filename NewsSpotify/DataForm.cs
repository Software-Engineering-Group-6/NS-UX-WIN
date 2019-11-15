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
    public partial class DataForm : Form
    {
        public DataForm()
        {
            InitializeComponent();
            // make sure the data grid view expands as the container window
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right |
                AnchorStyles.Top |
                AnchorStyles.Left;
        }

        public bool PopulateTracksWithNews(List<Track> tracks, List<News> news)
        {
            // auto size the columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // auto size the rows
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // set the column count, names and other properties
            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].Name = "Source";
            dataGridView1.Columns[1].Name = "Headline";
            dataGridView1.Columns[2].Name = "Name";
            dataGridView1.Columns[3].Name = "Artists";
            dataGridView1.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[4].Name = "Duration";
            dataGridView1.Columns[5].Name = "Popularity";
            dataGridView1.Columns[6].Name = "External URL";
            dataGridView1.Columns[7].Name = "Preview URL";

            // set style of 7th and 8th columns to hyperlink style
            dataGridView1.Columns[6].DefaultCellStyle = get_hyperlink_style();
            dataGridView1.Columns[7].DefaultCellStyle = get_hyperlink_style();

            // now iterate through the data and add it
            for (int i = 0; i < tracks.Count; ++i)
            {
                String[] row = {news[i].source, news[i].headline, tracks[i].name,
                                list_to_multiline(tracks[i].artists),
                                tracks[i].duration,
                                Int32.Parse(tracks[i].popularity).ToString("00") + "/100",
                                tracks[i].external_url, tracks[i].preview_url};
                dataGridView1.Rows.Add(row);
            }
            if (dataGridView1.Rows.Count == tracks.Count) return true;
            else return false;
        }

        public bool PopulateTracks(List<Track> tracks)
        {
            // auto size the columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // auto size the rows
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // set the column count, names and other properties
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Artists";
            dataGridView1.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[2].Name = "Duration";
            dataGridView1.Columns[3].Name = "Popularity";
            dataGridView1.Columns[4].Name = "External URL";
            dataGridView1.Columns[5].Name = "Preview URL";

            // set style of 7th and 8th columns to hyperlink style
            dataGridView1.Columns[4].DefaultCellStyle = get_hyperlink_style();
            dataGridView1.Columns[5].DefaultCellStyle = get_hyperlink_style();

            // now iterate through the data and add it
            for (int i = 0; i < tracks.Count; ++i)
            {
                String[] row = {tracks[i].name,
                                list_to_multiline(tracks[i].artists),
                                tracks[i].duration,
                                Int32.Parse(tracks[i].popularity).ToString("00") + "/100",
                                tracks[i].external_url, tracks[i].preview_url};
                dataGridView1.Rows.Add(row);
            }
            if (dataGridView1.Rows.Count == tracks.Count) return true;
            else return false;
        }

        public String list_to_multiline(List<String> list)
        {
            return String.Join("\n", list.ToArray());
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText.Contains("URL"))
            {
                System.Diagnostics.Process.Start(dataGridView1.CurrentCell.EditedFormattedValue as String);
            }
        }

        private DataGridViewCellStyle get_hyperlink_style()
        {
            DataGridViewCellStyle dgvc = new DataGridViewCellStyle();
            System.Drawing.Font font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Underline);
            dgvc.Font = font;
            dgvc.ForeColor = Color.Blue;
            return dgvc;
        }
    }
}
