using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using binay.Pages;

namespace binay.Pages.contact
{
    public class ContactModel : PageModel
    {
        public List<ContactInformation> listcontact = new List<ContactInformation>();
        public ClientContact clientcontactinf = new ClientContact();
        public void OnGet()

        {
            
            try
            {
                string connectstring = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectstring))
                {
                    connection.Open();
                    string sql = "SELECT * FROM contacts ORDER BY SurName";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader Reader = command.ExecuteReader())
                            while (Reader.Read())
                            {
                                ContactInformation contactinfor = new ContactInformation();
                                contactinfor.id = "" + Reader.GetInt32(0);
                                contactinfor.Email = Reader.GetString(1);
                                contactinfor.No_of_Clients = Reader.GetInt32(2);
                                contactinfor.FirstName = Reader.GetString(3);
                                contactinfor.SurName = Reader.GetString(4);

                                


                               listcontact.Add(contactinfor);
                            }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:  " + e.ToString());
            }
        }
    }

    public class ContactInformation
    {
        public string id ;
        public string FirstName;
        public string SurName;
        public string Email;
        public int No_of_Clients;


    }
}


