using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Reset : System.Web.UI.Page
    {
        public string SITConnectDBConnectionString_190704d = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public string userId { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.Params.Get("token");
            if (String.IsNullOrEmpty(token))
            {
                lbLoginError.Text = "Invalid Token!";
                lbLoginError.Visible = true;
                lbNewPass.Visible = false;
                tbNewPassword.Visible = false;
                btnReset.Visible = false;
            }
            else
            {
                try
                {
                    IJsonSerializer serializer = new JsonNetSerializer();
                    var provider = new UtcDateTimeProvider();
                    IJwtValidator validator = new JwtValidator(serializer, provider);
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                    var json = decoder.Decode(token, System.Configuration.ConfigurationManager.AppSettings["JWT_KEY"], verify: true);
                    Verify.Token tokenObj = JsonSerializer.Deserialize<Verify.Token>(json);
                    if (tokenObj.type == "reset")
                    {
                        userId = tokenObj.id.ToString();
                        if (Page.IsPostBack)
                        {
                            validate_190704d();
                        }
                    }
                    else
                    {
                        lbLoginError.Text = "Invalid Token!";
                        lbLoginError.Visible = true;
                        lbNewPass.Visible = false;
                        tbNewPassword.Visible = false;
                        btnReset.Visible = false;
                    }
                }
                catch (TokenExpiredException)
                {
                    lbLoginError.Text = "Expired Token!";
                    lbLoginError.Visible = true;
                    lbNewPass.Visible = false;
                    tbNewPassword.Visible = false;
                    btnReset.Visible = false;
                }
                catch (SignatureVerificationException)
                {
                    lbLoginError.Text = "Invalid Token!";
                    lbLoginError.Visible = true;
                    lbNewPass.Visible = false;
                    tbNewPassword.Visible = false;
                    btnReset.Visible = false;
                }
            }
        }

        public void validate_190704d()
        {
            bool inputOk = true;
            string fieldErrors = "";
            if (tbNewPassword.Text.ToString().Length < 11 || !Regex.IsMatch(tbNewPassword.Text.ToString(), "[A-Z]") || !Regex.IsMatch(tbNewPassword.Text.ToString(), "[a-z]") || !Regex.IsMatch(tbNewPassword.Text.ToString(), "[0-9]") || !Regex.IsMatch(tbNewPassword.Text.ToString(), "[^A-Za-z0-9]"))
            {
                fieldErrors += "Password does not meet requirements<br />";
                inputOk = false;
            }
            if (inputOk)
            {
                bool previousPass = false;
                List<List<string>> last2Pass = getLast2PasswordsFromDb(userId);
                SHA512Managed hashing_190704d = new SHA512Managed();
                for (var index = 0; index < 2; index++)
                {
                    string pwdWithSalt_190704d = tbNewPassword.Text.ToString().Trim() + last2Pass[1][index];
                    byte[] hashWithSalt_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt_190704d));
                    string userHash_190704d = Convert.ToBase64String(hashWithSalt_190704d);
                    if (userHash_190704d.Equals(last2Pass[0][index]))
                    {
                        previousPass = true;
                        break;
                    }
                    if (last2Pass[1].Count < 2)
                    {
                        break;
                    }
                }
                if (previousPass)
                {
                    lbLoginError.Text = "You cannot use your previous passwords as your new password.";
                    lbLoginError.Visible = true;
                } else
                {
                    byte[] saltByte = new byte[8];
                    RNGCryptoServiceProvider rng_190704d = new RNGCryptoServiceProvider();
                    rng_190704d.GetBytes(saltByte);
                    string salt_190704d = Convert.ToBase64String(saltByte);
                    string pwdWithSalt_190704d = tbNewPassword.Text.ToString().Trim() + salt_190704d;
                    byte[] hashWithSalt_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt_190704d));
                    string finalHash_190704d = Convert.ToBase64String(hashWithSalt_190704d);
                    updatePassword(finalHash_190704d, salt_190704d);
                    Response.Redirect("/Login?passwordReset=true");
                }
            }
            else
            {
                lbLoginError.Text = "";
                lbLoginError.Text = fieldErrors;
                lbLoginError.Visible = true;
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

        protected bool updatePassword(string hash, string salt)
        {
            try
            {
                using (SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d))
                {
                    using (SqlCommand command_190704d = new SqlCommand("UPDATE Account SET PasswordHash = @ph, PasswordSalt = @ps, PasswordLastChanged = @plc, IncorrectPasswordAttempts = 0, LockoutDateTime = @ldt WHERE Id = @id; INSERT INTO PasswordHistory(PasswordHash, PasswordSalt, SetDate, AccountId) VALUES(@ph, @ps, @plc, @id);"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            command_190704d.CommandType = CommandType.Text;
                            command_190704d.Parameters.AddWithValue("@ph", hash);
                            command_190704d.Parameters.AddWithValue("@ps", salt);
                            command_190704d.Parameters.AddWithValue("@plc", DateTime.Now);
                            command_190704d.Parameters.AddWithValue("@ldt", DateTime.Now.AddMinutes(-5));
                            command_190704d.Parameters.AddWithValue("@id", userId);
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