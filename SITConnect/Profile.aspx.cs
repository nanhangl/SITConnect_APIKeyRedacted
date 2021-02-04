using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Profile : System.Web.UI.Page
    {
        string SITConnectDBConnectionString_190704d = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        double minsFromPasswordLastChanged;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(Session["userId"])) || Convert.ToString(Session["sessionGUID"]) != Convert.ToString(Request.Cookies.Get("sessionGUID").Value))
                {
                    Response.Redirect("/Login?requireLogin=true");
                }
                else
                {
                    string userId_190704d = Session["UserId"].ToString();
                    minsFromPasswordLastChanged = (DateTime.Now - getPasswordLastChangedFromDb(userId_190704d)).TotalMinutes;
                    lbProfileFirstName.Text = getFirstNameFromDb(userId_190704d);
                    lbProfileLastName.Text = getLastNameFromDb(userId_190704d);
                    lbProfileEmail.Text = getEmailFromDb(userId_190704d);
                    lbProfileDateOfBirth.Text = getDateOfBirthFromDb(userId_190704d);
                    lbProfileCreditCardEncrypted.Text = getCreditCardFromDb(userId_190704d);
                    lbProfileCreditCardDecrypted.Text = decryptData_190704d(Convert.FromBase64String(getCreditCardFromDb(userId_190704d)), userId_190704d).Replace("*", " ");
                    if (minsFromPasswordLastChanged < 5)
                    {
                        btnChangePassword.Enabled = false;
                        changePasswordNotice.InnerText = $" (You can change your password in {5-minsFromPasswordLastChanged} mins)";
                        changePasswordNotice.Visible = true;
                    } else if (minsFromPasswordLastChanged > 15)
                    {
                        lbProfileDateOfBirth.Text = "REDACTED - CHANGE PASSWORD FIRST";
                        lbProfileCreditCardEncrypted.Text = "REDACTED - CHANGE PASSWORD FIRST";
                        lbProfileCreditCardDecrypted.Text = "REDACTED - CHANGE PASSWORD FIRST";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
                Response.Redirect("/Login?requireLogin=true");
            }
        }

        protected string getFirstNameFromDb(string id_190704d)
        {
            string firstName_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT FirstName FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["FirstName"] != null)
                        {
                            if (reader_190704d["FirstName"] != DBNull.Value)
                            {
                                firstName_190704d = reader_190704d["FirstName"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return firstName_190704d;
        }

        protected string getLastNameFromDb(string id_190704d)
        {
            string LastName_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT LastName FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["LastName"] != null)
                        {
                            if (reader_190704d["LastName"] != DBNull.Value)
                            {
                                LastName_190704d = reader_190704d["LastName"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return LastName_190704d;
        }

        protected string getEmailFromDb(string id_190704d)
        {
            string Email_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT Email FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["Email"] != null)
                        {
                            if (reader_190704d["Email"] != DBNull.Value)
                            {
                                Email_190704d = reader_190704d["Email"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return Email_190704d;
        }

        protected string getDateOfBirthFromDb(string id_190704d)
        {
            string DateOfBirth_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT DateOfBirth FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["DateOfBirth"] != null)
                        {
                            if (reader_190704d["DateOfBirth"] != DBNull.Value)
                            {
                                DateOfBirth_190704d = reader_190704d["DateOfBirth"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return DateOfBirth_190704d;
        }

        protected string getCreditCardFromDb(string id_190704d)
        {
            string CreditCard_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT CreditCard FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["CreditCard"] != null)
                        {
                            if (reader_190704d["CreditCard"] != DBNull.Value)
                            {
                                CreditCard_190704d = reader_190704d["CreditCard"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return CreditCard_190704d;
        }
        protected string getIVFromDb(string id_190704d)
        {
            string IV_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT IV FROM EncryptionKey WHERE accountId=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["IV"] != null)
                        {
                            if (reader_190704d["IV"] != DBNull.Value)
                            {
                                IV_190704d = reader_190704d["IV"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return IV_190704d;
        }

        protected string getKeyFromDb(string id_190704d)
        {
            string Key_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT EKey FROM EncryptionKey WHERE accountId=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["EKey"] != null)
                        {
                            if (reader_190704d["EKey"] != DBNull.Value)
                            {
                                Key_190704d = reader_190704d["EKey"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return Key_190704d;
        }

        protected string decryptData_190704d(byte[] cipherText_190704d, string id)
        {
            string plainText_190704d = null;
            try
            {
                RijndaelManaged cipher_190704d = new RijndaelManaged();
                cipher_190704d.IV = Convert.FromBase64String(getIVFromDb(id));
                cipher_190704d.Key = Convert.FromBase64String(getKeyFromDb(id));
                ICryptoTransform decryptTransform_190704d = cipher_190704d.CreateDecryptor();
                using (MemoryStream msDecrypt_190704d = new MemoryStream(cipherText_190704d))
                {
                    using (CryptoStream csDecrypt_190704d = new CryptoStream(msDecrypt_190704d, decryptTransform_190704d, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt_190704d = new StreamReader(csDecrypt_190704d))
                        {
                            plainText_190704d = srDecrypt_190704d.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText_190704d;
        }

            protected void btnChangePasswordChange_Click(object sender, EventArgs e)
        {
            SHA512Managed hashing_190704d = new SHA512Managed();
            string password_190704d = tbCurrentPassword.Text.ToString();
            string newpassword_190704d = tbNewPassword.Text.ToString();
            string hashFromDb_190704d = getPasswordHashFromDb(Session["userId"].ToString());
            string saltFromDb_190704d = getPasswordSaltFromDb(Session["userId"].ToString());
            try
            {
                if (saltFromDb_190704d != null && saltFromDb_190704d.Length > 0 && hashFromDb_190704d != null && hashFromDb_190704d.Length > 0)
                {
                    string pwdWithSalt_190704d = password_190704d + saltFromDb_190704d;
                    byte[] hashWithSalt_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt_190704d));
                    string userHash_190704d = Convert.ToBase64String(hashWithSalt_190704d);
                    if (userHash_190704d.Equals(hashFromDb_190704d))
                    {
                        if (newpassword_190704d.Length < 11 || !Regex.IsMatch(newpassword_190704d, "[A-Z]") || !Regex.IsMatch(newpassword_190704d, "[a-z]") || !Regex.IsMatch(newpassword_190704d, "[0-9]") || !Regex.IsMatch(newpassword_190704d, "[^A-Za-z0-9]"))
                        {
                            Response.Cookies.Add(new HttpCookie("passwordChange", "notcomplex"));
                        }
                        else
                        {
                            bool previousPass = false;
                            List<List<string>> last2Pass = getLast2PasswordsFromDb(Session["userId"].ToString());
                            SHA512Managed hashing2_190704d = new SHA512Managed();
                            for (var index = 0; index < 2; index++)
                            {
                                string pwdWithSalt1_190704d = tbNewPassword.Text.ToString().Trim() + last2Pass[1][index];
                                byte[] hashWithSalt1_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt1_190704d));
                                string userHash1_190704d = Convert.ToBase64String(hashWithSalt1_190704d);
                                if (userHash1_190704d.Equals(last2Pass[0][index]))
                                {
                                    previousPass = true;
                                    break;
                                }
                            }
                            if (previousPass)
                            {
                                Response.Cookies.Add(new HttpCookie("passwordChange", "previousPass"));
                            }
                            else
                            {
                                byte[] saltByte2 = new byte[8];
                                RNGCryptoServiceProvider rng2_190704d = new RNGCryptoServiceProvider();
                                rng2_190704d.GetBytes(saltByte2);
                                string salt2_190704d = Convert.ToBase64String(saltByte2);
                                string pwdWithSalt2_190704d = tbNewPassword.Text.ToString().Trim() + salt2_190704d;
                                byte[] hashWithSalt2_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt2_190704d));
                                string finalHash2_190704d = Convert.ToBase64String(hashWithSalt2_190704d);
                                if (minsFromPasswordLastChanged > 5)
                                {
                                    changePassword_190704d(finalHash2_190704d, salt2_190704d);
                                    Response.Cookies.Add(new HttpCookie("passwordChange", "success"));
                                }
                                else
                                    Response.Cookies.Add(new HttpCookie("passwordChange", "minPassAge"));
                            }
                        }
                    }
                    else
                    {
                        Response.Cookies.Add(new HttpCookie("passwordChange", "wrongPassword"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }

        protected string getPasswordHashFromDb(string id_190704d)
        {
            string PasswordHash_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT PasswordHash FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["PasswordHash"] != null)
                        {
                            if (reader_190704d["PasswordHash"] != DBNull.Value)
                            {
                                PasswordHash_190704d = reader_190704d["PasswordHash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return PasswordHash_190704d;
        }

        protected string getPasswordSaltFromDb(string id_190704d)
        {
            string PasswordSalt_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT PasswordSalt FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["PasswordSalt"] != null)
                        {
                            if (reader_190704d["PasswordSalt"] != DBNull.Value)
                            {
                                PasswordSalt_190704d = reader_190704d["PasswordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return PasswordSalt_190704d;
        }

        protected DateTime getPasswordLastChangedFromDb(string id_190704d)
        {
            DateTime lastChanged_190704d = new DateTime(1970,01,01);
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT PasswordLastChanged FROM Account WHERE Id=@Id";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["PasswordLastChanged"] != null)
                        {
                            if (reader_190704d["PasswordLastChanged"] != DBNull.Value)
                            {
                                lastChanged_190704d = Convert.ToDateTime(reader_190704d["PasswordLastChanged"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return lastChanged_190704d;
        }

        public void changePassword_190704d(string newpasswordhash_190704d, string newsalt_190704d)
        {
            try
            {
                using (SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d))
                {
                    using (SqlCommand command_190704d = new SqlCommand("UPDATE Account SET PasswordHash = @nph, PasswordSalt = @ns, PasswordLastChanged = @plc WHERE Id = @id; INSERT INTO PasswordHistory VALUES(@nph, @ns, @plc, @id);"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            command_190704d.CommandType = CommandType.Text;
                            command_190704d.Parameters.AddWithValue("@nph", newpasswordhash_190704d);
                            command_190704d.Parameters.AddWithValue("@ns", newsalt_190704d);
                            command_190704d.Parameters.AddWithValue("@plc", DateTime.Now);
                            command_190704d.Parameters.AddWithValue("@id", Session["userId"]);
                            command_190704d.Connection = connection_190704d;
                            connection_190704d.Open();
                            command_190704d.ExecuteNonQuery();
                            connection_190704d.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected List<List<string>> getLast2PasswordsFromDb(string id_190704d)
        {
            List<string> PasswordHashs_190704d = new List<string>();
            List<string> PasswordSalts_190704d = new List<string>();
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT TOP 2 * FROM PasswordHistory WHERE AccountId = @Id ORDER BY SetDate DESC";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Id", id_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["PasswordHash"] != DBNull.Value)
                        {
                            PasswordHashs_190704d.Add(reader_190704d["PasswordHash"].ToString());
                        }
                        if (reader_190704d["PasswordSalt"] != DBNull.Value)
                        {
                            PasswordSalts_190704d.Add(reader_190704d["PasswordSalt"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            List<List<string>> Last2Passwords_190704d = new List<List<string>>();
            Last2Passwords_190704d.Add(PasswordHashs_190704d);
            Last2Passwords_190704d.Add(PasswordSalts_190704d);
            return Last2Passwords_190704d;
        }

        protected bool updatePassword(string hash, string salt, string id_190704d)
        {
            try
            {
                using (SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d))
                {
                    using (SqlCommand command_190704d = new SqlCommand("UPDATE Account SET PasswordHash = @ph, PasswordSalt = @ps, PasswordLastChanged = @plc WHERE Id = @id; INSERT INTO PasswordHistory(PasswordHash, PasswordSalt, SetDate, AccountId) VALUES(@ph, @ps, @plc, @id);"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            command_190704d.CommandType = CommandType.Text;
                            command_190704d.Parameters.AddWithValue("@ph", hash);
                            command_190704d.Parameters.AddWithValue("@ps", salt);
                            command_190704d.Parameters.AddWithValue("@plc", DateTime.Now);
                            command_190704d.Parameters.AddWithValue("@id", id_190704d);
                            command_190704d.Connection = connection_190704d;
                            connection_190704d.Open();
                            command_190704d.ExecuteNonQuery();
                            connection_190704d.Close();
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
    }
}