using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DBSniffer
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = null;
        MySqlDataAdapter Adapter = new MySqlDataAdapter();
        MySqlCommand cmd;

        private void DB_InitConnection()
        {
            string dbConnectionString = @" 
                server=127.0.0.1;
                Port=3310;
                userid=cuser;
                password=msasia;
                database=log_instalacja"; //connection string. Provide all data needed to establish connection to DB.

            try
            {
                conn = new MySqlConnection(dbConnectionString); //This object is used to open a connection to a database.
                conn.Open(); //Opens DB connection.
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close(); //Closes DB connection.
                }
            }

        }


        
        private void LoadToColumn()
        {
            try
            {
                conn.Open();

                cmd = new MySqlCommand("SELECT * FROM so_log", conn); // Create the SelectCommand.
                Adapter.SelectCommand = cmd;
                DataSet DS = new DataSet();
                Adapter.Fill(DS);
                    


                //Adapter.Fill(DS);
                dataGridView1.DataSource = DS.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
     
        private void LoadData(string so)
        {
            DataSet ds = null;
            MySqlDataAdapter da = null;
            string stm = "SELECT CONCAT(" +
            "TRIM(Replace(SUBSTRING_INDEX(cpu,'@',1),'CPU','')),'/ '," +
            "SUBSTRING_INDEX(ram,'(',1),'/'," +
            "REPLACE(hdd1_size,'n/a','noHDD'),'/',REPLACE(hdd2_size,'No HDD2',''),'/'," +
            "optical,'/'," +
            "os_label)" +
            " AS spec," +
            "id,so,rp,model,serial,TRIM(Replace(SUBSTRING_INDEX(cpu,'@',1),'CPU','')) AS cpu," +
            "ram,hdd1_size,hdd1_model,hdd1_serial,hdd2_size,hdd2_model,hdd2_serial,optical,gpu1,gpu2," +
            "resname,diagonal,os_name,os_build,os_language,os_key,os_label,comments,install_date,new_licence" +
            " FROM so_log";
            if (!string.IsNullOrWhiteSpace(so))
            {
                stm = stm + " where so='" + so + "'";
            }

            try
            {
                conn.Open();
                ds = new DataSet();
                da = new MySqlDataAdapter(stm, conn);
                da.Fill(ds, "spec");

                dataGridView2.DataSource = ds.Tables["spec"];
                dataGridView1.DataSource= ds.Tables["spec"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

        }
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            LoadData(TBsearch.Text);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DB_InitConnection();
            LoadData(TBsearch.Text);
        }


        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            //return string from selected cell
            string CellStringValue(int cell_id, int i)
            {
                return dataGridView2.SelectedRows[i].Cells[cell_id].Value.ToString();
            }

            //counts selected rows and display number in tool strip
            if (dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected) > 0)
            {
                ToolStrip_selected.Text = "Selected: "+dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected).ToString();
            }



            #region id sheet
            /*
             *  #### Cell IDs:
             *  0 - specification
             *  1 - MySQL id
             *  2 - SO
             *  3 - RP
             *  4 - model
             *  5 - serial
             *  6 - CPU
             *  7 - RAM
             *  8 - HDD 1 Size
             *  9 - HDD 1 Model
             *  10- HDD 1 Serial
             *  11- HDD 2 Size
             *  12- HDD 2 Model
             *  13- HDD 1 Serial
             *  14- Optical
             *  15- GPU1
             *  16- GPU2
             *  17- ResName
             *  18- Diagonal
             *  19- Installed OS
             *  20- OS build
             *  21- OS languages
             *  22- OS key
             *  23- OS label
             *  24- comments
             *  25- Install date
            */
            #endregion
            if (dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected) > 0)
            {
                #region clear list boxes
                LBox_model.Items.Clear();
                LBox_cpu.Items.Clear();
                LBox_ram.Items.Clear();
                LBox_disk1Size.Items.Clear();
                LBox_disk2Size.Items.Clear();
                LBox_diagonal.Items.Clear();
                LBox_res.Items.Clear();
                LBox_optical.Items.Clear();
                LBox_label.Items.Clear();

                LBox_serial.Items.Clear();
                LBox_disk1Serial.Items.Clear();
                LBox_disk2Serial.Items.Clear();
                LBox_osLicence.Items.Clear();
                LBox_diskModel1.Items.Clear();
                LBox_diskModel2.Items.Clear();
                LBox_osLanguage.Items.Clear();
                LBox_osBuild.Items.Clear();
                LBox_installedOS.Items.Clear();
                LBox_gpu2.Items.Clear();
                LBox_gpu1.Items.Clear();
                #endregion

                for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {
                    if (!LBox_serial.Items.Contains(CellStringValue(5, i)))
                        LBox_serial.Items.Add(CellStringValue(5, i));

                    if (!LBox_disk1Serial.Items.Contains(CellStringValue(10, i)))
                        LBox_disk1Serial.Items.Add(CellStringValue(10, i));

                    if (!LBox_disk2Serial.Items.Contains(CellStringValue(13, i)))
                        LBox_disk2Serial.Items.Add(CellStringValue(13, i));

                    //if (!LBox_osLicence.Items.Contains(CellStringValue(13, i)))
                    //    LBox_osLicence.Items.Add(CellStringValue(13, i));

                    if (!LBox_osLanguage.Items.Contains(CellStringValue(21, i)))
                        LBox_osLanguage.Items.Add(CellStringValue(21, i));

                    if (!LBox_osBuild.Items.Contains(CellStringValue(20, i)))
                        LBox_osBuild.Items.Add(CellStringValue(20, i));

                    if (!LBox_installedOS.Items.Contains(CellStringValue(19, i)))
                        LBox_installedOS.Items.Add(CellStringValue(19, i));

                    if (!LBox_gpu2.Items.Contains(CellStringValue(16, i)))
                        LBox_gpu2.Items.Add(CellStringValue(16, i));

                    if (!LBox_gpu1.Items.Contains(CellStringValue(15, i)))
                        LBox_gpu1.Items.Add(CellStringValue(15, i));


                    if (!LBox_model.Items.Contains(CellStringValue(4, i)))
                        LBox_model.Items.Add(CellStringValue(4, i));

                    if (!LBox_cpu.Items.Contains(CellStringValue(6, i)))
                        LBox_cpu.Items.Add(CellStringValue(6, i));

                    if (!LBox_ram.Items.Contains(CellStringValue(7, i)))
                        LBox_ram.Items.Add(CellStringValue(7, i));

                    if (!LBox_disk1Size.Items.Contains(CellStringValue(8, i)))
                        LBox_disk1Size.Items.Add(CellStringValue(8, i));

                    if (!LBox_disk2Size.Items.Contains(CellStringValue(11, i)))
                        LBox_disk2Size.Items.Add(CellStringValue(11, i));

                    if (!LBox_diagonal.Items.Contains(CellStringValue(18, i)))
                        LBox_diagonal.Items.Add(CellStringValue(18, i));

                    if (!LBox_res.Items.Contains(CellStringValue(17, i)))
                        LBox_res.Items.Add(CellStringValue(17, i));

                    if (!LBox_optical.Items.Contains(CellStringValue(14, i)))
                        LBox_optical.Items.Add(CellStringValue(14, i));

                    if (!LBox_label.Items.Contains(CellStringValue(23, i)))
                        LBox_label.Items.Add(CellStringValue(23, i));

                }

                #region old
                // StringBuilder  model = new StringBuilder();
                // StringBuilder cpu = new StringBuilder();
                // StringBuilder ram = new StringBuilder();
                // StringBuilder diskSize1 = new StringBuilder();
                // StringBuilder diskSize2 = new StringBuilder();
                // StringBuilder diskModel1 = new StringBuilder();
                // StringBuilder diskModel2 = new StringBuilder();
                // StringBuilder diskSerial1 = new StringBuilder();
                // StringBuilder diskSerial2 = new StringBuilder();
                // StringBuilder diagonal = new StringBuilder();
                // StringBuilder res = new StringBuilder();
                // StringBuilder optical = new StringBuilder();
                // StringBuilder osLabel = new StringBuilder();
                // StringBuilder gpu1 = new StringBuilder();
                // StringBuilder gpu2 = new StringBuilder();
                // StringBuilder osInstalled = new StringBuilder();
                // StringBuilder osBuild = new StringBuilder();
                // StringBuilder osLanguage = new StringBuilder();
                // StringBuilder serial = new StringBuilder();

                

                // for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                // {
                //     if(!model.ToString().Contains(dataGridView2.SelectedRows[i].Cells[4].Value.ToString()))
                //     {
                //         model.AppendLine(dataGridView2.SelectedRows[i].Cells[4].Value.ToString());
                //     } //model

                //     if (!cpu.ToString().Contains(dataGridView2.SelectedRows[i].Cells[6].Value.ToString()))
                //     {
                //         cpu.AppendLine(dataGridView2.SelectedRows[i].Cells[6].Value.ToString());
                //     } //cpu

                //     if (!ram.ToString().Contains(dataGridView2.SelectedRows[i].Cells[7].Value.ToString()))
                //     {
                //         ram.AppendLine(dataGridView2.SelectedRows[i].Cells[7].Value.ToString());
                //     } //ram

                //     if (!diskSize1.ToString().Contains(dataGridView2.SelectedRows[i].Cells[8].Value.ToString()))
                //     {
                //         diskSize1.AppendLine(dataGridView2.SelectedRows[i].Cells[8].Value.ToString());
                //     } //hdd1 size

                //     if (!diskSize2.ToString().Contains(dataGridView2.SelectedRows[i].Cells[11].Value.ToString()))
                //     {
                //         diskSize2.AppendLine(dataGridView2.SelectedRows[i].Cells[11].Value.ToString());
                //     } //hdd2 size

                //     if (!diskModel1.ToString().Contains(dataGridView2.SelectedRows[i].Cells[9].Value.ToString()))
                //     {
                //         diskModel1.AppendLine(dataGridView2.SelectedRows[i].Cells[9].Value.ToString());
                //     } //hdd1 model

                //     if (!diskModel2.ToString().Contains(dataGridView2.SelectedRows[i].Cells[12].Value.ToString()))
                //     {
                //         diskModel2.AppendLine(dataGridView2.SelectedRows[i].Cells[12].Value.ToString());
                //     }//hdd2 model

                //     if (!diskSerial1.ToString().Contains(dataGridView2.SelectedRows[i].Cells[10].Value.ToString()))
                //     {
                //         diskSerial1.AppendLine(dataGridView2.SelectedRows[i].Cells[10].Value.ToString());
                //     }//hdd1 serial

                //     if (!diskSerial2.ToString().Contains(dataGridView2.SelectedRows[i].Cells[13].Value.ToString()))
                //     {
                //         diskSerial2.AppendLine(dataGridView2.SelectedRows[i].Cells[13].Value.ToString());
                //     }//hdd2 serial

                //     if (!diagonal.ToString().Contains(dataGridView2.SelectedRows[i].Cells[18].Value.ToString()))
                //     {
                //         diagonal.AppendLine(dataGridView2.SelectedRows[i].Cells[18].Value.ToString());
                //     } //diagonal

                //     if (!res.ToString().Contains(dataGridView2.SelectedRows[i].Cells[17].Value.ToString()))
                //     {
                //         res.AppendLine(dataGridView2.SelectedRows[i].Cells[17].Value.ToString());
                //     }//resname

                //     if (!optical.ToString().Contains(dataGridView2.SelectedRows[i].Cells[14].Value.ToString()))
                //     {
                //         optical.AppendLine(dataGridView2.SelectedRows[i].Cells[14].Value.ToString());
                //     } //optical

                //     if (!osLabel.ToString().Contains(dataGridView2.SelectedRows[i].Cells[23].Value.ToString()))
                //     {
                //         osLabel.AppendLine(dataGridView2.SelectedRows[i].Cells[23].Value.ToString());
                //     } //os label

                //     if (!gpu1.ToString().Contains(dataGridView2.SelectedRows[i].Cells[15].Value.ToString()))
                //     {
                //         gpu1.AppendLine(dataGridView2.SelectedRows[i].Cells[15].Value.ToString());
                //     }//gpu 1

                //     if (!gpu2.ToString().Contains(dataGridView2.SelectedRows[i].Cells[16].Value.ToString()))
                //     {
                //         gpu2.AppendLine(dataGridView2.SelectedRows[i].Cells[16].Value.ToString());
                //     }//gpu 2

                //     if (!osInstalled.ToString().Contains(dataGridView2.SelectedRows[i].Cells[19].Value.ToString()))
                //     {
                //         osInstalled.AppendLine(dataGridView2.SelectedRows[i].Cells[19].Value.ToString());
                //     } // os installed

                //     if (!osBuild.ToString().Contains(dataGridView2.SelectedRows[i].Cells[20].Value.ToString()))
                //     {
                //         osBuild.AppendLine(dataGridView2.SelectedRows[i].Cells[20].Value.ToString());
                //     } //os build

                //     if (!osLanguage.ToString().Contains(dataGridView2.SelectedRows[i].Cells[21].Value.ToString()))
                //     {
                //         osLanguage.AppendLine(dataGridView2.SelectedRows[i].Cells[21].Value.ToString());
                //     } //os language
                //     if (!serial.ToString().Contains(dataGridView2.SelectedRows[i].Cells[5].Value.ToString()))
                //     {
                //         serial.AppendLine(dataGridView2.SelectedRows[i].Cells[5].Value.ToString());
                //     } //serial number

                // }

                // RBox_osLanguage.Text = osLanguage.ToString();
                // RBox_isbuild.Text = osBuild.ToString();
                // RBox_osInstalled.Text = osInstalled.ToString();
                // RBox_gpu2.Text = gpu2.ToString();
                // RBox_gpu1.Text = gpu1.ToString();
                // RBox_osLabel.Text = osLabel.ToString();
                // RBox_optical.Text = optical.ToString();
                // RBox_res.Text = res.ToString();
                // RBox_diagonal.Text = diagonal.ToString();

                // RBox_disk2SN.Text = diskSerial2.ToString();
                // RBox_diskSerial1.Text = diskSerial1.ToString();

                // RBox_disk2Model.Text = diskModel2.ToString();
                // RBox_disk1Model.Text = diskModel1.ToString();

                // RBox_hdd2Size.Text = diskSize2.ToString();
                // RBox_hdd1Size.Text = diskSize1.ToString();
                // RBox_ram.Text = ram.ToString();
                // RBox_cpu.Text = cpu.ToString();
                //// LBox_model.Text = model.ToString();
                // RBox_serial.Text = serial.ToString();

                #endregion
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int index = e.RowIndex;
            //DataGridViewRow selectedRow= dataGridView2.Rows[index];
            //MessageBox.Show(selectedRow.Cells[4].Value.ToString());
           
        }

        private void TBsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        #region clipBoard events


        #endregion

        private void LBox_serial_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_serial.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }

        }

        private void LBox_disk1Serial_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_disk1Serial.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_disk2Serial_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_disk2Serial.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_osLicence_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_osLicence.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_diskModel1_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_diskModel1.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_diskModel2_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_diskModel2.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_osLanguage_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_osLanguage.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_label_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_label.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_optical_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_optical.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_res_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_res.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_diagonal_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_diagonal.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_disk2Size_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_disk2Size.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_disk1Size_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_disk1Size.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_ram_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_ram.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_installedOS_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_installedOS.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_gpu2_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_gpu2.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_gpu1_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_gpu1.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_cpu_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_cpu.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }

        private void LBox_model_Click(object sender, EventArgs e)
        {
            try
            {
                string s = LBox_model.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
            catch (Exception)
            {
            }
        }
    }
}
