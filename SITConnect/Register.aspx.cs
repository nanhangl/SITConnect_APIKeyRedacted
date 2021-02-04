using JsonWebToken;
using JsonWebToken.Internal;
using JWT.Algorithms;
using JWT.Builder;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SITConnect
{
    public partial class Register : System.Web.UI.Page
    {
        string SITConnectDBConnectionString_190704d = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        static string finalHash_190704d;
        static string salt_190704d;
        byte[] Key_190704d;
        byte[] IV_190704d;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Session["userId"])) && Convert.ToString(Session["sessionGUID"]) == Convert.ToString(Request.Cookies.Get("sessionGUID").Value))
            {
                Response.Redirect("/Profile");
            }
            if (Page.IsPostBack)
            {
                validateSubmission_190704d();
            }
        }
        public void validateSubmission_190704d()
        {
            bool inputOk = true;
            string fieldErrors = "";
            string emailRegexPattern = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";
            if (!Regex.IsMatch(tbFirstName.Text.ToString(), "^[A-Za-z0-9\\s]+$"))
            {
                fieldErrors += "First Name is empty or invalid<br />";
                inputOk = false;
            }
            if (!Regex.IsMatch(tbLastName.Text.ToString(), "^[A-Za-z0-9\\s]+$"))
            {
                fieldErrors += "Last Name is empty or invalid<br />";
                inputOk = false;
            }
            if (!Regex.IsMatch(tbEmail.Text.ToString(), emailRegexPattern))
            {
                fieldErrors += "Email does not meet requirements<br />";
                inputOk = false;
            }
            if (tbPassword.Text.ToString().Length < 11 || !Regex.IsMatch(tbPassword.Text.ToString(), "[A-Z]") || !Regex.IsMatch(tbPassword.Text.ToString(), "[a-z]") || !Regex.IsMatch(tbPassword.Text.ToString(), "[0-9]") || !Regex.IsMatch(tbPassword.Text.ToString(), "[^A-Za-z0-9]"))
            {
                fieldErrors += "Password does not meet requirements<br />";
                inputOk = false;
            }
            if (String.IsNullOrEmpty(tbDateOfBirth.Text.ToString()))
            {
                fieldErrors += "Date of Birth is empty<br />";
                inputOk = false;
            }
            if (!Regex.IsMatch($"{tbCardNumber1.Text}{tbCardNumber2.Text}{tbCardNumber3.Text}{tbCardNumber4.Text}", "^[0-9]{16}(?!=-)$"))
            {
                fieldErrors += "Card Number must be 16 digits<br />";
                inputOk = false;
            }
            if (String.IsNullOrEmpty(tbExpiry.Text.ToString()))
            {
                fieldErrors += "Card Expiry is empty<br />";
                inputOk = false;
            }
            if (inputOk)
            {
                if (validateCaptcha_190704d())
                {
                    string pwd_190704d = tbPassword.Text.ToString().Trim();
                    RNGCryptoServiceProvider rng_190704d = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];
                    rng_190704d.GetBytes(saltByte);
                    salt_190704d = Convert.ToBase64String(saltByte);
                    SHA512Managed hashing_190704d = new SHA512Managed();
                    string pwdWithSalt_190704d = pwd_190704d + salt_190704d;
                    byte[] hashWithSalt_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt_190704d));
                    finalHash_190704d = Convert.ToBase64String(hashWithSalt_190704d);
                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key_190704d = cipher.Key;
                    IV_190704d = cipher.IV;
                    createAccount_190704d();
                }
                else
                {
                    lbRegistrationError.Text = "";
                    lbRegistrationError.Text = "Captcha failed, please try again";
                    lbRegistrationError.Visible = true;
                }
            }
            else
            {
                lbRegistrationError.Text = "";
                lbRegistrationError.Text = fieldErrors;
                lbRegistrationError.Visible = true;
            }
        }

        public void createAccount_190704d()
        {
            try
            {
                using (SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d))
                {
                    Int32 insertedId_190704d;
                    using (SqlCommand command_190704d = new SqlCommand("INSERT INTO Account OUTPUT Inserted.ID VALUES(@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt, @DateOfBirth, @CreditCard, @DateTimeEmailVerified, @DateTimeRegistered, @PasswordLastChanged, @IncorrectPasswordAttempts, @LockoutDateTime);"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            command_190704d.CommandType = CommandType.Text;
                            command_190704d.Parameters.AddWithValue("@FirstName", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbFirstName.Text.Trim(), true));
                            command_190704d.Parameters.AddWithValue("@LastName", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbLastName.Text.Trim(), true));
                            command_190704d.Parameters.AddWithValue("@Email", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbEmail.Text.Trim(), true));
                            command_190704d.Parameters.AddWithValue("@PasswordHash", finalHash_190704d);
                            command_190704d.Parameters.AddWithValue("@PasswordSalt", salt_190704d);
                            command_190704d.Parameters.AddWithValue("@DateOfBirth", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbDateOfBirth.Text.Trim(), true));
                            command_190704d.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData_190704d(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode($"{tbCardNumber1.Text}{tbCardNumber2.Text}{tbCardNumber3.Text}{tbCardNumber4.Text}*{tbExpiry.Text}", true))));
                            command_190704d.Parameters.AddWithValue("@DateTimeEmailVerified", DBNull.Value);
                            command_190704d.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            command_190704d.Parameters.AddWithValue("@PasswordLastChanged", DateTime.Now);
                            command_190704d.Parameters.AddWithValue("@IncorrectPasswordAttempts", 0);
                            command_190704d.Parameters.AddWithValue("@LockoutDateTime", DBNull.Value);
                            command_190704d.Connection = connection_190704d;
                            connection_190704d.Open();
                            insertedId_190704d = (Int32)command_190704d.ExecuteScalar();
                            connection_190704d.Close();
                        }
                    }
                    using (SqlCommand command2_190704d = new SqlCommand("INSERT INTO EncryptionKey VALUES(@IV, @Key, @AccountId)"))
                    {
                        using (SqlDataAdapter sda2 = new SqlDataAdapter())
                        {
                            command2_190704d.CommandType = CommandType.Text;

                            command2_190704d.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV_190704d));
                            command2_190704d.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key_190704d));
                            command2_190704d.Parameters.AddWithValue("@AccountId", insertedId_190704d);
                            command2_190704d.Connection = connection_190704d;
                            connection_190704d.Open();
                            command2_190704d.ExecuteNonQuery();
                            connection_190704d.Close();
                        }
                    }
                    using (SqlCommand command2_190704d = new SqlCommand("INSERT INTO PasswordHistory VALUES(@PasswordHash, @PasswordSalt, @SetDate, @AccountId)"))
                    {
                        using (SqlDataAdapter sda2 = new SqlDataAdapter())
                        {
                            command2_190704d.CommandType = CommandType.Text;

                            command2_190704d.Parameters.AddWithValue("@PasswordHash", finalHash_190704d);
                            command2_190704d.Parameters.AddWithValue("@PasswordSalt", salt_190704d);
                            command2_190704d.Parameters.AddWithValue("@SetDate", DateTime.Now);
                            command2_190704d.Parameters.AddWithValue("@AccountId", insertedId_190704d);
                            command2_190704d.Connection = connection_190704d;
                            connection_190704d.Open();
                            command2_190704d.ExecuteNonQuery();
                            connection_190704d.Close();
                        }
                    }

                    var jwtoken = new JwtBuilder()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(System.Configuration.ConfigurationManager.AppSettings["JWT_KEY"])
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds())
                      .AddClaim("id", insertedId_190704d)
                      .AddClaim("type", "verify")
                      .Encode();
                    String tokenUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/Verify?token=" + jwtoken;
                    SendEmail_190704d(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbEmail.Text.Trim(), true), System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbFirstName.Text.Trim(), true), "verify", tokenUrl).Wait();
                    Response.Redirect("/Login?registration=success");
                }
            }
            catch (Exception ex)
            {
                lbRegistrationError.Text = "Email already in use";
                lbRegistrationError.Visible = true;
                //throw new Exception(ex.ToString());
            }
        }

        public byte[] encryptData_190704d(string data_190704d)
        {
            byte[] cipherText_190704d = null;
            try
            {
                RijndaelManaged cipher_190704d = new RijndaelManaged();
                cipher_190704d.IV = IV_190704d;
                cipher_190704d.Key = Key_190704d;
                ICryptoTransform encryptTransform = cipher_190704d.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data_190704d);
                cipherText_190704d = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

            }
            return cipherText_190704d;
        }

        public class MyObject_190704d
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool validateCaptcha_190704d()
        {
            bool result_190704d = true;
            string captchaResponse_190704d = Request.Form["g-recaptcha-response"];
            HttpWebRequest req_190704d = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=REDACTED&response=" + captchaResponse_190704d);

            try
            {
                using (WebResponse wResponse = req_190704d.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(jsonResponse);
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject_190704d jsonObject = js.Deserialize<MyObject_190704d>(jsonResponse);
                        result_190704d = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result_190704d;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public async Task SendEmail_190704d(string recipientEmail, string recipientName, string emailType, string url)
        {
            var apiKey = System.Configuration.ConfigurationManager.AppSettings["SENDGRID_API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("project_190704d@outlook.sg", "SITConnect");
            var to = new EmailAddress(recipientEmail, recipientName);
            var subject = "Default Subject";
            var plainTextContent = "Default Content";
            var htmlContent = "<strong>Default HTML Content</strong>";

            if (emailType == "verify")
            {
                subject = "[SITConnect] Verify your email address";
                plainTextContent = $"Hello {recipientName}, click on the link to verify your email address: {url}. Be quick! Link expires in 15 minutes!";
                htmlContent = $"Hello {recipientName},<br/>click on the link to verify your email address:<br/>{url}<br/>Be quick! Link expires in 15 minutes!";
            }
            else
            {
                subject = "[SITConnect] Reset your password";
                plainTextContent = $"Hello {recipientName}, click on the link to reset your password: {url}. Be quick! Link expires in 15 minutes!";
                htmlContent = $"Hello {recipientName},<br/>click on the link to reset your password:<br/>{url}<br/>Be quick! Link expires in 15 minutes!";
            }
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}