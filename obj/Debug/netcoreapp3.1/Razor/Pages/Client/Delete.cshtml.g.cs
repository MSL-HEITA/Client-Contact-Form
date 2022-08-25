#pragma checksum "C:\Users\CBB\source\repos\binay\Pages\Client\Delete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7eb9212cf71566571653686b01947a1b9d037855"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(binay.Pages.Client.Pages_Client_Delete), @"mvc.1.0.razor-page", @"/Pages/Client/Delete.cshtml")]
namespace binay.Pages.Client
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\CBB\source\repos\binay\Pages\_ViewImports.cshtml"
using binay;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\CBB\source\repos\binay\Pages\Client\Delete.cshtml"
using System.Data.SqlClient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7eb9212cf71566571653686b01947a1b9d037855", @"/Pages/Client/Delete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ff7a6c8ce39f67c3be0aeeabe90d380cbdb4b73f", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Client_Delete : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\CBB\source\repos\binay\Pages\Client\Delete.cshtml"
  
    String emessage = "";
    try
    {
        int count = 0;
        List<int> contactID = new List<int>();
        string id = Request.Query["id"];
        string connectionsting = "Data Source=localhost;Initial Catalog=binarycitpact;Integrated Security=True";
        using (SqlConnection connect = new SqlConnection(connectionsting))
        {
            connect.Open();

            string cmd = "SELECT contact_id FROM Client_Contact WHERE client_id=@client_id ";
            using (SqlCommand comm = new SqlCommand(cmd, connect))
            {
                comm.Parameters.AddWithValue("@client_id", id);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contactID.Add(reader.GetInt32(0));
                    }
                }
            }

            foreach (int id_id in contactID)
            {
                string cm = "SELECT No_of_Clients FROM contacts WHERE id=@id";
                using (SqlCommand comm = new SqlCommand(cm, connect))
                {
                    comm.Parameters.AddWithValue("@id", id_id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }

                    }
                }
                string cmd4 = "UPDATE contacts SET No_of_Clients =@no WHERE id= @id;";
                using (SqlCommand comm2 = new SqlCommand(cmd4, connect))
                {
                    count -= 1;
                    comm2.Parameters.AddWithValue("@no", count);
                    comm2.Parameters.AddWithValue("@id", id_id);
                    comm2.ExecuteNonQuery();

                }
            }

            string sql = "DELETE FROM client WHERE client_id=@id";
            using (SqlCommand command = new SqlCommand(sql, connect))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }

            string cmd2 = "DELETE FROM Client_Contact  WHERE client_id IS NULL ;";
            using (SqlCommand comm = new SqlCommand(cmd2, connect))
            {

                comm.ExecuteNonQuery();
            }




        }
    }



    catch (Exception e)
    {
        System.Diagnostics.Debug.WriteLine("Exception:  " + e.ToString());
        emessage = e.Message;
    }

    Response.Redirect("/client/Index");

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Pages_Client_Delete> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Client_Delete> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Pages_Client_Delete>)PageContext?.ViewData;
        public Pages_Client_Delete Model => ViewData.Model;
    }
}
#pragma warning restore 1591
