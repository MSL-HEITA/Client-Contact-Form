using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Data.SqlClient;

namespace binay.Pages.Client
{
    public class IndexModel : PageModel
    {
        string connectstring = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";
        public List<ClientInformation> listclients = new List<ClientInformation>();
        
        public void OnGet()
        {


            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectstring))
                {

                    connection.Open();
                    string sql = "select * from client";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader Reader = command.ExecuteReader())
                            while (Reader.Read())
                            {
                                ClientInformation clientinfor = new ClientInformation();
                                clientinfor.client_id = "" + Reader.GetValue(0);
                                clientinfor.client_name = Reader.GetString(1);
                                clientinfor.no_of_contacts = Reader.GetInt32(2);

                                listclients.Add(clientinfor);
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:  " + e.ToString());
            }
        }
    }

    public class ClientInformation
    {
        public string client_id;
        public string client_name;
        public int no_of_contacts;
       

    }
}

        

