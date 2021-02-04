using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Verify : System.Web.UI.Page
    {
        string SITConnectDBConnectionString_190704d = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.Params.Get("token");
            if (String.IsNullOrEmpty(token))
            {
                lbVerifyHeader.Text = "Invalid Token!";
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
                    Token tokenObj = JsonSerializer.Deserialize<Token>(json);
                    if (tokenObj.type == "verify")
                    {
                        if (verifyEmail(tokenObj.id.ToString()))
                            lbVerifyHeader.Text = "Email Verified!";
                        else
                            lbVerifyHeader.Text = "Unknown Error!";
                    }
                    else
                    {
                        lbVerifyHeader.Text = "Invalid Token!";
                    }
                }
                catch (TokenExpiredException)
                {
                    lbVerifyHeader.Text = "Expired Token!";
                }
                catch (SignatureVerificationException)
                {
                    lbVerifyHeader.Text = "Invalid Token!";
                }
            }
        }
        public bool verifyEmail(string id)
        {
            try
            {
                using (SqlConnection connection_190704d = new SqlConnection(SITConnectDBConnectionString_190704d))
                {
                    Int32 insertedId_190704d;
                    using (SqlCommand command_190704d = new SqlCommand("UPDATE Account SET DateTimeEmailVerified = @dtev WHERE Id = @id;"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            command_190704d.CommandType = CommandType.Text;
                            command_190704d.Parameters.AddWithValue("@dtev", DateTime.Now);
                            command_190704d.Parameters.AddWithValue("@id", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(id, true));
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
                lbVerifyHeader.Text = "Unknown Error";
                return false;
            }
        }

        public class Token
        {
            public int exp { get; set; }
            public object id { get; set; }
            public string type { get; set; }
        }

    }
}