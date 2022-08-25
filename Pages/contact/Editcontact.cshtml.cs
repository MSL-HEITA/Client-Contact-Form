using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using binay.Pages;
using binay.Pages.Client;

namespace binay.Pages.contact
{
    public class EditcontactModel : PageModel

    {
        string connectionsting = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";
        public ContactInformation Contactinfor = new ContactInformation();
        public List<string> listclient = new List<string>();
        public List<string> removelistclient = new List<string>();
        public ClientContact ClientContactinfor = new ClientContact();
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
                    string sql = "SELECT * FROM contacts WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connect))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                Contactinfor.id = "" + reader.GetInt32(0);
                                Contactinfor.Email = reader.GetString(1);
                                Contactinfor.FirstName = reader.GetString(3);
                                Contactinfor.SurName = reader.GetString(4);

                            }
                        }
                    }
                    String Newsql = "SELECT client_id FROM Client_Contact where contact_id=@contact_id;";
                    using (SqlCommand command = new SqlCommand(Newsql, connect))
                    {
                        command.Parameters.AddWithValue("@contact_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())

                        {
                            while (reader.Read())
                            {
                                String client_id = "" + reader.GetString(0);
                                removelistclient.Add(client_id);
                            }

                        }

                    }
                    String nsql = "SELECT client_id FROM client;";
                    using (SqlCommand command = new SqlCommand(nsql, connect))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())

                        {
                            while (reader.Read())
                            {
                                String client_id = "" + reader.GetString(0);
                                listclient.Add(client_id);

                            }
                            reader.Close();
                            reader.Dispose();
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
            Contactinfor.FirstName = Request.Form["First_name"];
            Contactinfor.SurName = Request.Form["Surename"];
            Contactinfor.Email = Request.Form["Email"];
            Contactinfor.id = Request.Form["id"];

            ClientContactinfor.ClientId = Request.Form["remove affiliation"];
            string add_new_affiliation = Request.Form["add affiliation"];

            if (Contactinfor.FirstName.Length == 0 || Contactinfor.SurName.Length == 0 || Contactinfor.Email.Length == 0)
            {
                emessage = "Please Fill in all the fields";
                return;
            }
            try
            {

                using (SqlConnection connect = new SqlConnection(connectionsting))
                {
                    connect.Open();
                    string c = "SELECT client_id FROM Client_Contact WHERE contact_id =@id";
                    using (SqlCommand command1 = new SqlCommand(c, connect))
                    {
                        command1.Parameters.AddWithValue("@id", Contactinfor.id);
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                String compareclientid = reader.GetString(0);

                                if (add_new_affiliation.Equals(compareclientid))
                                {
                                    emessage = "This contact details are already linked to this client";
                                    return;
                                }
                            }
                        }
                    }

                    string sql = "UPDATE contacts SET  FirstName=@FirstName,SurName=@SureName,Email=@Email WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connect))
                    {

                        command.Parameters.AddWithValue("@FirstName", Contactinfor.FirstName);
                        command.Parameters.AddWithValue("@SureName", Contactinfor.SurName);
                        command.Parameters.AddWithValue("@Email", Contactinfor.Email);
                        command.Parameters.AddWithValue("@id", Contactinfor.id);

                        command.ExecuteNonQuery();
                    }
                    if (ClientContactinfor.ClientId.Length > 0)
                    {

                        string cmd = "DELETE FROM Client_Contact  WHERE client_id=@client_id AND  contact_id=@contact_id ;";
                        using (SqlCommand comm = new SqlCommand(cmd, connect))
                        {
                            comm.Parameters.AddWithValue("@client_id", ClientContactinfor.ClientId);
                            comm.Parameters.AddWithValue("@contact_id", Contactinfor.id);
                            comm.ExecuteNonQuery();
                        }

                        String cmd2 = " SELECT  count(client_id) FROM Client_Contact WHERE contact_id=@contact_id  ;";

                        using (SqlCommand comm = new SqlCommand(cmd2, connect))
                        {
                            comm.Parameters.AddWithValue("@contact_id", Contactinfor.id);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Contactinfor.No_of_Clients = reader.GetInt32(0);


                                }

                            }
                        }

                        string cmd2_1 = "UPDATE contacts SET No_of_Clients = @No_of_Clients  WHERE id=@id ;";
                        using (SqlCommand comm2 = new SqlCommand(cmd2_1, connect))
                        {

                            comm2.Parameters.AddWithValue("@No_of_Clients", Contactinfor.No_of_Clients);
                            comm2.Parameters.AddWithValue("@id", Contactinfor.id);
                            comm2.ExecuteNonQuery();
                        }

                        ////////////////////////////////
                        String cmd3 = " SELECT  count(contact_id) FROM Client_Contact WHERE client_id=@client_id; ";
                        using (SqlCommand comm = new SqlCommand(cmd3, connect))
                        {
                            comm.Parameters.AddWithValue("@client_id", ClientContactinfor.ClientId);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    clientinfor.no_of_contacts = reader.GetInt32(0);

                                }

                            }
                        }
                        string cmd4 = "UPDATE client SET No_of_contacts= @No_of_contacts WHERE client_id= @client_id;";
                        using (SqlCommand comm2 = new SqlCommand(cmd4, connect))
                        {

                            comm2.Parameters.AddWithValue("@No_of_contacts", clientinfor.no_of_contacts);
                            comm2.Parameters.AddWithValue("@client_id", ClientContactinfor.ClientId);
                            comm2.ExecuteNonQuery();

                        }


                    }
                    if (add_new_affiliation.Length > 0)
                    {
                        
                        string cmd = "INSERT INTO Client_Contact (client_id, contact_id) values (@client_id, @contact_id);";
                        using (SqlCommand command = new SqlCommand(cmd, connect))
                        {
                            command.Parameters.AddWithValue("@client_id", add_new_affiliation);
                            command.Parameters.AddWithValue("@contact_id", Contactinfor.id);

                            command.ExecuteNonQuery();
                        }

                        String cmd2 = "SELECT  count(client_id) FROM Client_Contact WHERE contact_id=@contact_id ;";

                        using (SqlCommand comm = new SqlCommand(cmd2, connect))
                        {
                            comm.Parameters.AddWithValue("@contact_id", Contactinfor.id);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Contactinfor.No_of_Clients = reader.GetInt32(0);


                                }

                            }
                        }   
                        
                        string cmd2_1 = "UPDATE contacts SET No_of_Clients = @No_of_Clients  WHERE id=@id ;";
                        using (SqlCommand comm2 = new SqlCommand(cmd2_1, connect))
                        {

                            comm2.Parameters.AddWithValue("@No_of_Clients", Contactinfor.No_of_Clients);
                            comm2.Parameters.AddWithValue("@id", Contactinfor.id);
                            comm2.ExecuteNonQuery();
                        }

                        ////////////////////////////////
                        String cmd3 = " SELECT count(contact_id) FROM Client_Contact WHERE client_id=@client_id  ;  ";
                        using (SqlCommand comm = new SqlCommand(cmd3, connect))
                        {
                            comm.Parameters.AddWithValue("@client_id", add_new_affiliation);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    clientinfor.no_of_contacts = reader.GetInt32(0);

                                }

                            }
                        }
                        string cmd4 = "UPDATE client SET No_of_contacts= @No_of_contacts WHERE client_id= @client_id;";
                        using (SqlCommand comm2 = new SqlCommand(cmd4, connect))
                        {

                            comm2.Parameters.AddWithValue("@No_of_contacts", clientinfor.no_of_contacts);
                            comm2.Parameters.AddWithValue("@client_id", add_new_affiliation);
                            comm2.ExecuteNonQuery();

                        }

                    }

                   

                    
                    
                    
                }



            }
            catch (Exception e)
            {
                emessage = e.Message;
                System.Diagnostics.Debug.WriteLine("Exception:  " + e.ToString());
                return;
            }
            Response.Redirect("/contact/Contact");

        }

        
    }
    
       
    

 

}

