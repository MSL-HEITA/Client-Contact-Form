using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Data.SqlClient;

namespace binay.Pages.Client
{
    public class CreateClientModel : PageModel
    {
        
        
        public ClientInformation clientinfor = new ClientInformation();
        public string emessage = "";
        public string sucmessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string connectionsting = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";
            String letterId;
            clientinfor.client_name = Request.Form["Client_name"];
            String name = clientinfor.client_name.Trim();
            name = name.ToUpper();


            if (name.Length == 0)
            {
                emessage = "Client field can not be empty";
                return;
            }

            else if (name.Length < 3 && !name.Contains(" "))
            {


                Random rnd = new Random();
                int ascii_index = rnd.Next(65, 91);
                char myRandomUpperCase = Convert.ToChar(ascii_index);
                letterId = name + myRandomUpperCase;


            }
            else if (name.Length >= 3 && !name.Contains(" "))
            {
                letterId = name.Substring(0, 3);
            }

            else 
            {
                String[] arrayname = name.Split(" ");

                if (arrayname.Length < 3)
                {
                    letterId = arrayname[0][0].ToString() + arrayname[1][0].ToString() + arrayname[1][1].ToString();
                }

                else {
                    letterId = arrayname[0][0].ToString() + arrayname[1][0].ToString() + arrayname[2][0].ToString();
                }

            }

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionsting))
                {
                    connect.Open();
                    int numId = 0;
                    string cmd = "SELECT MAX(CAST(SUBSTRING([client_id], 4, 6) AS INT)) FROM[client]";



                    using (SqlCommand command = new SqlCommand(cmd, connect))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                String compId = "" + reader.GetValue(0);


                                if (compId.Length == 0)
                                {
                                    String strNumId = String.Format("{0:000}", numId);
                                    clientinfor.client_id = letterId + strNumId;
                                }
                                else
                                {
                                    numId = int.Parse(compId) + 1;
                                    String strNumId = String.Format("{0:000}", numId);
                                    clientinfor.client_id = letterId + strNumId;

                                }
                            }
                           
                        }
                    }
                    string sql = "INSERT INTO client " + "(client_id,client_name,no_of_contacts) VALUES " + "(@client_id,@client_name,@no_of_contacts);";

                    using (SqlCommand command = new SqlCommand(sql, connect))
                    {
                        command.Parameters.AddWithValue("@client_id", clientinfor.client_id);
                        command.Parameters.AddWithValue("@client_name", clientinfor.client_name);
                        command.Parameters.AddWithValue("@no_of_contacts", clientinfor.no_of_contacts);

                        command.ExecuteNonQuery();
                    }

                    
                
                    }
                
            }

                //string comparecmd = "SELECT client_id FROM client WHERE client_id =@newClient_id; ";
                //using (SqlCommand command = new SqlCommand(comparecmd, connect))
                //{
                //    using (SqlDataReader reader = command.ExecuteReader())
                //    {
                //        if (reader.Read())
                //        {
                //            //for loop to get id one by one


                //        }
                //    }
                //    
            
            

            catch (Exception e)
            {
                emessage = e.Message;
                System.Diagnostics.Debug.WriteLine("Exception:  " + e.ToString());
                return;
            }

            clientinfor.client_name = "";
            sucmessage = "New client added ";

            Response.Redirect("/Client/Index");
        }
    }
}

        