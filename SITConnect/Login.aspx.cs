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
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        string SITConnectDBConnectionString_190704d = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Session["userId"])) && Convert.ToString(Session["sessionGUID"]) == Convert.ToString(Request.Cookies.Get("sessionGUID").Value))
            {
                Response.Redirect("/Profile");
            }
            if (Convert.ToString(Request.QueryString["requireLogin"]) == "true")
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.Cookies.Get("loggedIn"))))
                {
                    Request.Cookies.Get("loggedIn").Expires = DateTime.Now.AddDays(-7);
                }
                lbLoginError.Text = "You need to login before continuing";
                lbLoginError.ForeColor = System.Drawing.Color.Red;
                lbLoginError.Visible = true;
            }
            if (Convert.ToString(Request.QueryString["passwordReset"]) == "true")
            {
                lbLoginError.Text = "Password has been reset. You can now login with your new password.";
                lbLoginError.ForeColor = System.Drawing.Color.Green;
                lbLoginError.Visible = true;
            }
            if (Convert.ToString(Request.QueryString["registration"]) == "success")
            {
                lbLoginError.Text = "Registration successful. Check your email for verification link.";
                lbLoginError.ForeColor = System.Drawing.Color.Green;
                lbLoginError.Visible = true;
            }
            if (Convert.ToString(Request.QueryString["forgotPass"]) == "true")
            {
                var email = Convert.ToString(Request.QueryString["email"]);
                if (String.IsNullOrEmpty(email))
                {
                    lbLoginError.Text = "Enter email address in the \"Email\" field, then click \"Forgot Password?\"";
                    lbLoginError.ForeColor = System.Drawing.Color.Red;
                    lbLoginError.Visible = true;
                } else
                {
                    string userId = getUserIdFromDB_190704d(email);
                    DateTime passwordLastChanged = getPasswordChangedDateTimeFromDb(email);
                    double minSincePasswordLastChanged = (DateTime.Now - passwordLastChanged).TotalMinutes;
                    if (!String.IsNullOrEmpty(userId))
                    {
                        if (minSincePasswordLastChanged > 5)
                        {
                            var jwtoken = new JwtBuilder()
                              .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                              .WithSecret(System.Configuration.ConfigurationManager.AppSettings["JWT_KEY"])
                              .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds())
                              .AddClaim("id", Convert.ToInt32(userId))
                              .AddClaim("type", "reset")
                              .Encode();
                            String tokenUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/Reset?token=" + jwtoken;
                            await SendEmail_190704d(email, getFirstNameFromDb(userId), "reset", tokenUrl);
                        } else
                        {
                            lbLoginError.Text = $"You need to wait {5-minSincePasswordLastChanged} mins before you can reset your password.";
                        }
                    }
                    lbLoginError.Text = "A password reset link will be sent if this email exists.";
                    lbLoginError.ForeColor = System.Drawing.Color.Green;
                    lbLoginError.Visible = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SHA512Managed hashing_190704d = new SHA512Managed();
            string email_190704d = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbLoginEmail.Text.ToString().Trim(), true);
            string password_190704d = tbLoginPassword.Text.ToString();
            string hashFromDb_190704d = getHashFromDB_190704d(email_190704d);
            string saltFromDb_190704d = getSaltFromDB_190704d(email_190704d);

            try
            {
                if (validateCaptcha_190704d())
                {
                    if (!String.IsNullOrEmpty(getDateTimeEmailVerifiedFromDB_190704d(email_190704d)))
                    {
                        if (saltFromDb_190704d != null && saltFromDb_190704d.Length > 0 && hashFromDb_190704d != null && hashFromDb_190704d.Length > 0)
                        {
                            string pwdWithSalt_190704d = password_190704d + saltFromDb_190704d;
                            byte[] hashWithSalt_190704d = hashing_190704d.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt_190704d));
                            string userHash_190704d = Convert.ToBase64String(hashWithSalt_190704d);
                            if (userHash_190704d.Equals(hashFromDb_190704d))
                            {
                                DateTime lockoutDateTime = getLockoutDateTimeFromDb(email_190704d);
                                if (lockoutDateTime != null)
                                {
                                    if ((DateTime.Now - lockoutDateTime).TotalMinutes < 5)
                                    {
                                        lbLoginError.Text = $"Account locked. Please try again in {5 - (DateTime.Now - lockoutDateTime).TotalMinutes} mins.";
                                        lbLoginError.ForeColor = System.Drawing.Color.Red;
                                    }
                                    else
                                    {
                                        resetIncorrectPasswordCounter(email_190704d);
                                        Guid sessionGuid = Guid.NewGuid();
                                        Session["userId"] = getUserIdFromDB_190704d(email_190704d);
                                        Session["sessionGUID"] = sessionGuid.ToString();
                                        HttpCookie sessionGuidCookie = new HttpCookie("sessionGUID", sessionGuid.ToString());
                                        sessionGuidCookie.HttpOnly = true;
                                        sessionGuidCookie.Secure = true;
                                        Response.Cookies.Add(sessionGuidCookie);
                                        Response.Cookies.Add(new HttpCookie("loggedIn", "true"));
                                        Response.Redirect("Profile.aspx", false);
                                    }
                                }
                                else
                                {
                                    resetIncorrectPasswordCounter(email_190704d);
                                    Guid sessionGuid = Guid.NewGuid();
                                    Session["userId"] = getUserIdFromDB_190704d(email_190704d);
                                    Session["sessionGUID"] = sessionGuid.ToString();
                                    HttpCookie sessionGuidCookie = new HttpCookie("sessionGUID", sessionGuid.ToString());
                                    sessionGuidCookie.HttpOnly = true;
                                    sessionGuidCookie.Secure = true;
                                    Response.Cookies.Add(sessionGuidCookie);
                                    Response.Cookies.Add(new HttpCookie("loggedIn", "true"));
                                    Response.Redirect("Profile.aspx", false);
                                }
                            }
                            else
                            {
                                DateTime lockoutDateTime = getLockoutDateTimeFromDb(email_190704d);
                                if (lockoutDateTime != null)
                                {
                                    if ((DateTime.Now - lockoutDateTime).TotalMinutes < 5)
                                    {
                                        lbLoginError.Text = $"Account locked. Please try again in {5 - (DateTime.Now - lockoutDateTime).TotalMinutes} mins.";
                                        lbLoginError.ForeColor = System.Drawing.Color.Red;
                                    }
                                    else
                                    {
                                        if (checkIncorrectPasswordAttempts(email_190704d) == "locked")
                                        {
                                            lbLoginError.Text = $"Account locked. Please try again in 5 mins.";
                                            lbLoginError.ForeColor = System.Drawing.Color.Red;
                                        }
                                        else
                                        {
                                            lbLoginError.Text = "Email or password is invalid. Please try again.";
                                            lbLoginError.ForeColor = System.Drawing.Color.Red;
                                        }
                                    }
                                }
                                else
                                {
                                    if (checkIncorrectPasswordAttempts(email_190704d) == "locked")
                                    {
                                        lbLoginError.Text = $"Account locked. Please try again in 5 mins.";
                                        lbLoginError.ForeColor = System.Drawing.Color.Red;
                                    }
                                    else
                                    {
                                        lbLoginError.Text = "Email or password is invalid. Please try again.";
                                        lbLoginError.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                                lbLoginError.Visible = true;
                            }
                        }
                        else
                        {
                            lbLoginError.Text = $"This account does not exist, please create an account";
                            lbLoginError.ForeColor = System.Drawing.Color.Red;
                            lbLoginError.Visible = true;
                        }
                    }
                    else
                    {
                        lbLoginError.Text = $"Please verify your email (<a href='/ResendVerification?email={System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(tbLoginEmail.Text.Trim(), false)}'>Resend Email</a>)";
                        lbLoginError.ForeColor = System.Drawing.Color.Red;
                        lbLoginError.Visible = true;
                    }
                }
                else
                {
                    lbLoginError.Text = "Captcha failed, please try again";
                    lbLoginError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }

        }

        protected string getHashFromDB_190704d(string email_190704d)
        {
            string hash_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT PasswordHash FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
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
                                hash_190704d = reader_190704d["PasswordHash"].ToString();
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
            return hash_190704d;
        }

        protected string getSaltFromDB_190704d(string email_190704d)
        {
            string salt_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT PasswordSalt FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
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
                                salt_190704d = reader_190704d["PasswordSalt"].ToString();
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
            return salt_190704d;
        }

        protected string getUserIdFromDB_190704d(string email_190704d)
        {
            string userId_190704d = null;
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT Id FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["Id"] != null)
                        {
                            if (reader_190704d["Id"] != DBNull.Value)
                            {
                                userId_190704d = reader_190704d["Id"].ToString();
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
            return userId_190704d;
        }

        protected string getDateTimeEmailVerifiedFromDB_190704d(string email_190704d)
        {
            string DateTimeEmailVerified_190704d = "default";
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT Id, DateTimeEmailVerified FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {
                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["Id"] != null)
                        {
                            DateTimeEmailVerified_190704d = "";
                        }
                        if (reader_190704d["DateTimeEmailVerified"] != null)
                        {
                            if (reader_190704d["DateTimeEmailVerified"] != DBNull.Value)
                            {
                                DateTimeEmailVerified_190704d = reader_190704d["DateTimeEmailVerified"].ToString();
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
            return DateTimeEmailVerified_190704d;
        }

        protected DateTime getLockoutDateTimeFromDb(string email_190704d)
        {
            DateTime lockout_190704d = new DateTime(1970, 01, 01);
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT LockoutDateTime FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {

                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["LockoutDateTime"] != null)
                        {
                            if (reader_190704d["LockoutDateTime"] != DBNull.Value)
                            {
                                lockout_190704d = Convert.ToDateTime(reader_190704d["LockoutDateTime"]);
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
            return lockout_190704d;
        }

        protected DateTime getPasswordChangedDateTimeFromDb(string email_190704d)
        {
            DateTime passwordChanged_190704d = new DateTime(1970, 01, 01);
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT PasswordLastChanged FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
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
                                passwordChanged_190704d = Convert.ToDateTime(reader_190704d["PasswordLastChanged"]);
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
            return passwordChanged_190704d;
        }

        protected string checkIncorrectPasswordAttempts(string email_190704d)
        {
            int incorrect_190704d = 0;
            string res_190704d = "";
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT IncorrectPasswordAttempts FROM Account WHERE Email=@Email";
            SqlCommand command_190704d = new SqlCommand(sql_190704d, connection_190704d);
            command_190704d.Parameters.AddWithValue("@Email", email_190704d);
            try
            {
                connection_190704d.Open();
                using (SqlDataReader reader_190704d = command_190704d.ExecuteReader())
                {

                    while (reader_190704d.Read())
                    {
                        if (reader_190704d["IncorrectPasswordAttempts"] != null)
                        {
                            if (reader_190704d["IncorrectPasswordAttempts"] != DBNull.Value)
                            {
                                incorrect_190704d = Convert.ToInt32(reader_190704d["IncorrectPasswordAttempts"]);
                            }
                        }
                    }

                }
                if (incorrect_190704d == 2)
                {
                    using (SqlCommand command2_190704d = new SqlCommand("UPDATE Account SET IncorrectPasswordAttempts = 0, LockoutDateTime = @ldt WHERE Email = @email;"))
                    {
                        using (SqlDataAdapter sda2 = new SqlDataAdapter())
                        {
                            command2_190704d.CommandType = CommandType.Text;
                            command2_190704d.Parameters.AddWithValue("@ldt", DateTime.Now);
                            command2_190704d.Parameters.AddWithValue("@email", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(email_190704d, true));
                            command2_190704d.Connection = connection_190704d;
                            command2_190704d.ExecuteNonQuery();
                            res_190704d = "locked";
                        }
                    }
                }
                else
                {
                    using (SqlCommand command2_190704d = new SqlCommand("UPDATE Account SET IncorrectPasswordAttempts = IncorrectPasswordAttempts + 1 WHERE Email = @email;"))
                    {
                        using (SqlDataAdapter sda2 = new SqlDataAdapter())
                        {
                            command2_190704d.CommandType = CommandType.Text;
                            command2_190704d.Parameters.AddWithValue("@email", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(email_190704d, true));
                            command2_190704d.Connection = connection_190704d;
                            command2_190704d.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection_190704d.Close(); }
            return res_190704d;
        }

        public void resetIncorrectPasswordCounter (string email)
        {
            try
            {
                using (SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d))
                {
                    using (SqlCommand command_190704d = new SqlCommand("UPDATE Account SET IncorrectPasswordAttempts = 0 WHERE Email = @email;"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            command_190704d.CommandType = CommandType.Text;
                            command_190704d.Parameters.AddWithValue("@email", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(email, true));
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
    }
}