using JWT.Algorithms;
using JWT.Builder;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class ResendVerification : System.Web.UI.Page
    {
        string SITConnectDBConnectionString_190704d = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request.Params.Get("email")))
            {
                lbResendHeader.Text = "No email in params!";
            } else
            {
                Account acc = getAccountFromDB_190704d(Request.Params.Get("email"));
                if (String.IsNullOrEmpty(acc.DateTimeEmailVerified.ToString()))
                {
                    var jwtoken = new JwtBuilder()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(System.Configuration.ConfigurationManager.AppSettings["JWT_KEY"])
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds())
                      .AddClaim("id", acc.Id)
                      .AddClaim("type", "verify")
                      .Encode();
                    String tokenUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/Verify?token=" + jwtoken;
                    SendEmail_190704d(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request.Params.Get("email"), true), System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(acc.FirstName, true), "verify", tokenUrl).Wait();
                    lbResendHeader.Text = "Verify link resent, check email!";
                } else
                {
                    lbResendHeader.Text = "Email already verified!";
                }
            }
        }

        protected Account getAccountFromDB_190704d(string email_190704d)
        {
            Account acc = new Account();
            SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d);
            string sql_190704d = "SELECT * FROM Account WHERE Email=@Email";
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
                                acc.Id = Convert.ToInt32(reader_190704d["Id"]);
                            }
                        }
                        if (reader_190704d["FirstName"] != null)
                        {
                            if (reader_190704d["FirstName"] != DBNull.Value)
                            {
                                acc.FirstName = reader_190704d["FirstName"].ToString();
                            }
                        }
                        if (reader_190704d["LastName"] != null)
                        {
                            if (reader_190704d["LastName"] != DBNull.Value)
                            {
                                acc.LastName = reader_190704d["LastName"].ToString();
                            }
                        }
                        if (reader_190704d["email"] != null)
                        {
                            if (reader_190704d["email"] != DBNull.Value)
                            {
                                acc.Email = reader_190704d["email"].ToString();
                            }
                        }
                        if (reader_190704d["PasswordHash"] != null)
                        {
                            if (reader_190704d["PasswordHash"] != DBNull.Value)
                            {
                                acc.PasswordHash = reader_190704d["PasswordHash"].ToString();
                            }
                        }
                        if (reader_190704d["PasswordSalt"] != null)
                        {
                            if (reader_190704d["PasswordSalt"] != DBNull.Value)
                            {
                                acc.PasswordSalt = reader_190704d["PasswordSalt"].ToString();
                            }
                        }
                        if (reader_190704d["DateOfBirth"] != null)
                        {
                            if (reader_190704d["DateOfBirth"] != DBNull.Value)
                            {
                                acc.DateOfBirth = Convert.ToDateTime(reader_190704d["DateOfBirth"]);
                            }
                        }
                        if (reader_190704d["CreditCard"] != null)
                        {
                            if (reader_190704d["CreditCard"] != DBNull.Value)
                            {
                                acc.CreditCard = reader_190704d["CreditCard"].ToString();
                            }
                        }
                        if (reader_190704d["DateTimeEmailVerified"] != null)
                        {
                            if (reader_190704d["DateTimeEmailVerified"] != DBNull.Value)
                            {
                                acc.DateTimeEmailVerified = Convert.ToDateTime(reader_190704d["DateTimeEmailVerified"]);
                            }
                        }
                        if (reader_190704d["DateTimeRegistered"] != null)
                        {
                            if (reader_190704d["DateTimeRegistered"] != DBNull.Value)
                            {
                                acc.DateTimeRegistered = Convert.ToDateTime(reader_190704d["DateTimeRegistered"]);
                            }
                        }
                        if (reader_190704d["PasswordLastChanged"] != null)
                        {
                            if (reader_190704d["PasswordLastChanged"] != DBNull.Value)
                            {
                                acc.PasswordLastChanged = Convert.ToDateTime(reader_190704d["PasswordLastChanged"]);
                            }
                        }
                        if (reader_190704d["IncorrectPasswordAttempts"] != null)
                        {
                            if (reader_190704d["IncorrectPasswordAttempts"] != DBNull.Value)
                            {
                                acc.IncorrectPasswordAttempts = Convert.ToInt32(reader_190704d["IncorrectPasswordAttempts"]);
                            }
                        }
                        if (reader_190704d["LockoutDateTime"] != null)
                        {
                            if (reader_190704d["LockoutDateTime"] != DBNull.Value)
                            {
                                acc.DateTimeEmailVerified = Convert.ToDateTime(reader_190704d["LockoutDateTime"]);
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
            return acc;
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