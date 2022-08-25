using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Data.SqlClient;

namespace binay.Pages.Client
{
    public class UpdateModel : PageModel
    {
        string connectionsting = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";
        public ClientInformation clientinfor = new ClientInformation();
        public string emessage = "";
        public string sucmessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];
            
            try
            {
                
                using (SqlConnection connect = new SqlConnection(connectionsting))
                {
                    connect.Open();
                    string sql = "SELECT * FROM client WHERE client_id=@client_id";
                    using (SqlCommand command = new SqlCommand(sql, connect))
                    {
                        command.Parameters.AddWithValue("@client_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                clientinfor.client_id = "" + reader.GetString(0);
                                clientinfor.client_name = reader.GetString(1);
                                clientinfor.no_of_contacts = reader.GetInt32(2);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                emessage = e.Message;
            }
        }

        public void OnPost()
        {
            clientinfor.client_name = Request.Form["Client_name"];
            clientinfor.client_id = Request.Form["id"];
            if (clientinfor.client_name.Length == 0)
            {
                emessage = "Client field can not be empty";
                return;
            }
            try
            {
                
                using (SqlConnection connect = new SqlConnection(connectionsting))
                {
                    connect.Open();
                    string sql = "UPDATE client SET  client_name=@name " + "WHERE client_id=@client_id";
                    using (SqlCommand command = new SqlCommand(sql, connect))
                    {//only updating name
                        
                        command.Parameters.AddWithValue("@name", clientinfor.client_name);
                        command.Parameters.AddWithValue("@client_id", clientinfor.client_id);

                        command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception e)
            {
                emessage = e.Message;
                return;
            }
            Response.Redirect("/Client/Index");
        }
    }
}