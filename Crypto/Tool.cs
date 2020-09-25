using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto
{
    public partial class Tool : Form
    {
        public const string _sharedKey = "zI0IL/Hamswr/7lbcRXe0kPhhfMirCXqxyKGaycrX+Q=";
        public const int _saltbytes = 24;
        public const int _iterations = 10000;
        public const int _codeLength = 6;

        public Tool()
        {
            InitializeComponent();
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtEncrypted.Text = Encrypt(this.txtPlainText.Text);
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = Encoding.UTF8.GetBytes(_sharedKey);

            // Hash the key with SHA256
            keyBytes = SHA256.Create().ComputeHash(keyBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, keyBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return string.Empty;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var keyBytes = Encoding.UTF8.GetBytes(_sharedKey);

            keyBytes = SHA256.Create().ComputeHash(keyBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, keyBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[_saltbytes];

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000))
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Key = key.GetBytes(aes.KeySize / 8);
                        aes.IV = key.GetBytes(aes.BlockSize / 8);

                        aes.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }

                        encryptedBytes = ms.ToArray();
                    }
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[_saltbytes];

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000))
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Key = key.GetBytes(aes.KeySize / 8);
                        aes.IV = key.GetBytes(aes.BlockSize / 8);
                        aes.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }
            }

            return decryptedBytes;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            this.txtDecrypted.Text = Decrypt(this.txtTextToDecrypt.Text);
        }

        private void btnHASH_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtToHASH.Text))
            {
                txtToHASH.Text = string.Empty;
            }

            // generate a 128-bit salt using a secure PRNG
            var salt = Encoding.UTF8.GetBytes(_sharedKey);

            // derive a 256-bit sub-key (use HMACSHA1 with 10,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: this.txtToHASH.Text,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterations,
                numBytesRequested: 32));

            this.txtHASHed.Text = hashed;
        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;

            DataTable dtexcel = new DataTable();
            dtexcel.Columns.Add("Data", typeof(String));
            dtexcel.Columns.Add("Result", typeof(String));


            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  

                    DataSet users = new DataSet();
                    oleAdpt.Fill(users, "users");

                    //j = users.Rows.Count;
                    var stringlist = users.Tables[0].Rows.Cast<DataRow>().Select(dr => dr[0].ToString()).ToList();

                    dtexcel.Clear();

                    for (var i = 0; i < stringlist.Count; i++)
                    {
                        DataRow row = dtexcel.NewRow();
                        row["Data"] = stringlist[i].ToString();
                        row["Result"] = this.rdEncrypt.Checked ? Encrypt(stringlist[i].ToString()) : Decrypt(stringlist[i].ToString());
                        dtexcel.Rows.Add(row);
                    }

                    dtexcel.Columns.Remove("F1");

                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch { }
            }
            return dtexcel;
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                        dataGridView1.Visible = true;
                        dataGridView1.DataSource = dtExcel;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }
    }
}
