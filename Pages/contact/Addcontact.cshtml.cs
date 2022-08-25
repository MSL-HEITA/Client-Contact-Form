using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using binay.Pages;
using binay.Pages.Client;
using System.Data.SqlClient;

namespace binay.Pages.contact
{
    public class AddcontactModel : PageModel
    {

        public ContactInformation contactinfor = new ContactInformation();
        public ClientContact clientcontactinf = new ClientContact();
        public ClientInformation clientinfor = new ClientInformation();
        public string emessage = "";
        public string sucmessage = "";
        public List<string> listclient = new List<string>();
        public List<string> emaillist = new List<string>();
        string connectionsting = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";

        public void OnGet()
        {
            
            try
            {
                using (SqlConnection connect = new SqlConnection(connectionsting))
                {
                    connect.Open();
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
            

            contactinfor.FirstName = Request.Form["First_name"];
            contactinfor.SurName = Request.Form["SureName"];
            contactinfor.Email = Request.Form["email"];
           

            clientcontactinf.ClientId = Request.Form["Affiliate"];


            using (SqlConnection connect = new SqlConnection(connectionsting))
            {
                connect.Open();

                String nsql = "SELECT Email FROM contacts;";
                using (SqlCommand command = new SqlCommand(nsql, connect))
                {
                    using (SqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            String emails = "" + reader.GetString(0);
                            emaillist.Add(emails);

                        }
                        reader.Close();
                        reader.Dispose();
                    }
                }

            }
            foreach( var email in emaillist)
            {
                if (email.Equals(contactinfor.Email))
                {
                    emessage = "This email is already in use ";
                    return;
                    
                }
                
            }


                if (contactinfor.FirstName.Length == 0 || contactinfor.SurName.Length == 0 || contactinfor.Email.Length == 0)
            {
                emessage = "Please Fill in all the fields";
                return;
            }

            else
            {
                try
                {

                    using (SqlConnection connection = new SqlConnection(connectionsting))
                    {
                        connection.Open();
                        string sql = "INSERT INTO contacts " + " (Email, FirstName, SurName,No_of_Clients ) VALUES" + "( @Email,@Firstname, @SurName, @No_of_Clients);";

                        using (SqlCommand command1 = new SqlCommand(sql, connection))
                        {

                            command1.Parameters.AddWithValue("@Email", contactinfor.Email);
                            command1.Parameters.AddWithValue("@Firstname", contactinfor.FirstName);
                            command1.Parameters.AddWithValue("@SurName", contactinfor.SurName);
                            command1.Parameters.AddWithValue("@No_of_Clients", 0);


                            command1.ExecuteNonQuery();

                        }
                        if (clientcontactinf.ClientId.Length > 0)
                        {
                            string newsql = "SELECT id FROM contacts WHERE Email=@Email";
                            using (SqlCommand command = new SqlCommand(newsql, connection))
                            {
                                command.Parameters.AddWithValue("@Email", contactinfor.Email);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        clientcontactinf.contactId = reader.GetInt32(0);

                                    }
                                    reader.Close();
                                    reader.Dispose();
                                }
                            }

                            String cmd = "INSERT INTO Client_Contact " + "(client_id, contact_id) VALUES " + " (@client_id, @contact_id);";
                            using (SqlCommand comm = new SqlCommand(cmd, connection))
                            {
                               
                                comm.Parameters.AddWithValue("@client_id", clientcontactinf.ClientId);
                                comm.Parameters.AddWithValue("@contact_id", clientcontactinf.contactId);

                                comm.ExecuteNonQuery();

                            }

                            String cmd2 = "SELECT count(client_id) FROM Client_Contact  WHERE contact_id=@contact_id  ;";

                            using (SqlCommand comm = new SqlCommand(cmd2, connection))
                            {
                                comm.Parameters.AddWithValue("@contact_id", clientcontactinf.contactId);

                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        contactinfor.No_of_Clients = reader.GetInt32(0);


                                    }

                                }
                                
                                string cmd2_1 = "UPDATE contacts SET No_of_Clients = @No_of_Clients  WHERE id=@id ;";
                                using (SqlCommand comm2 = new SqlCommand(cmd2_1, connection))
                                {

                                    comm2.Parameters.AddWithValue("@No_of_Clients", contactinfor.No_of_Clients);
                                    comm2.Parameters.AddWithValue("@id", clientcontactinf.contactId);
                                    comm2.ExecuteNonQuery();
                                }
                            }

                            String cmd3 = "SELECT  count(contact_id) FROM Client_Contact WHERE client_id=@client_id  ; ";
                            using (SqlCommand comm = new SqlCommand(cmd3, connection))
                            {
                                comm.Parameters.AddWithValue("@client_id", clientcontactinf.ClientId);
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        clientinfor.no_of_contacts =  reader.GetInt32(0);

                                    }

                                }
                                string cmd4 = "UPDATE client SET No_of_contacts= @No_of_contacts WHERE client_id= @client_id;";
                                using (SqlCommand comm2 = new SqlCommand(cmd4, connection))
                                {
                                   
                                    comm2.Parameters.AddWithValue("@No_of_contacts", clientinfor.no_of_contacts);
                                    comm2.Parameters.AddWithValue("@client_id", clientcontactinf.ClientId);
                                    comm2.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    emessage = e.Message;
                    System.Diagnostics.Debug.WriteLine("Exception:  " + e.ToString());
                }

                contactinfor.FirstName = "";
                contactinfor.SurName = "";
                contactinfor.Email = "";
                clientcontactinf.ClientId = "";
                sucmessage = "New client added ";

                Response.Redirect("/contact/Contact");
            }
        }
    } 
}

