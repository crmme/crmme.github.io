using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace crmme
{
    public static class MsgMgr
    {
        private static string NextMessageUrl { get; set; } = "https://graph.microsoft.com/v1.0/me/messages";
        private static AccessTokenResult tokenResult { get; set; }

        public async static Task<List<Model.EmailAddress>> GetEmailAddresses(IAccessTokenProvider TokenProvider, HttpClient Http)
        {
            List<Model.EmailAddress> response = new List<Model.EmailAddress>();
            if (tokenResult == null || tokenResult.Status != AccessTokenResultStatus.Success)
            {
                tokenResult = await TokenProvider.RequestAccessToken(
             new AccessTokenRequestOptions
             {

                 Scopes = new[] { "https://graph.microsoft.com/Mail.Read.Shared", "https://graph.microsoft.com/User.Read" }

             });
            }
            if (tokenResult.TryGetToken(out var token))
            {
              
                Http.DefaultRequestHeaders.Clear();
                Http.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Value);
                var webresponse = await Http.GetFromJsonAsync<Model.MailResponse>(NextMessageUrl);
                NextMessageUrl = webresponse.OdataNextLink;
                foreach (var msg in webresponse.Value)
                {
                    if (msg.From != null)
                    {
                        response.Add(msg.From.EmailAddress);
                    }
                    foreach (var m in msg.ToRecipients)
                    {

                        response.Add(m.EmailAddress);

                    }
                    foreach (var m in msg.CcRecipients)
                    {

                        response.Add(new Model.EmailAddress() { Address = m.Address, Name = m.Name });

                    }
                }
            }








            return removeDuplicates(response);
        }

        private static List<Model.EmailAddress> removeDuplicates(List<Model.EmailAddress> list)
        {
            return list.Distinct().ToList();
        }

        public static List<Model.EmailAddress> CleanUp(List<Model.EmailAddress> List, string myaddress)
        {
            List.RemoveAll(a => a.Address == null);
            List.RemoveAll(a => a.Address == myaddress);
            List.RemoveAll(a => a.Address.Contains("noreply"));
            List.RemoveAll(a => a.Address.Contains("no-reply"));

            return List.Distinct().ToList();
        }

        public static async Task<int> AddToContacts(List<Model.EmailAddress> list, IAccessTokenProvider TokenProvider, HttpClient Http)
        {
            int contactscreated = 0;

            var newtokenResult = await TokenProvider.RequestAccessToken(
            new AccessTokenRequestOptions
            {

                Scopes = new[] { "https://graph.microsoft.com/Contacts.ReadWrite", "https://graph.microsoft.com/People.Read.All", "https://graph.microsoft.com/User.Read" }

            });

            if (newtokenResult.TryGetToken(out var token))
            {
                Http.DefaultRequestHeaders.Clear();
                Http.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Value);

                foreach (var address in list)
                {

                    string getUrl = $"https://graph.microsoft.com/v1.0/me/people/?$search={"\"" + address.Address + "\""}";
                    
                    var thiscontact = await Http.GetFromJsonAsync<Model.ContactResponse>(getUrl);
          
                    if (thiscontact.value.Count==0)
                    {
                       
                        var contact = new Model.Contact();
                        contact.givenName = address.Name.Split(" ", 2)[0];
                        if (address.Name.Split(" ", 2).Length > 1)
                        {
                            contact.surname = address.Name.Split(" ", 2)[1];
                        }
                        else
                        {
                            contact.surname = "";
                        }

                        string ParentFolderId = "AAMkADcxZDA2ZDUxLTU2YjEtNGU1NS1iMzFiLTg2MmM0ZTcyNWQ3OQAuAAAAAAAFBc84A2esR5IdcMTyQ07vAQDcaC4Ez_S4Qq-gmw2AZm7bAAMcbmXDAAA=";
                        contact.EmailAddresses = new List<Model.EmailAddress>()
                        {
                            new Model.EmailAddress
                            {
                                Address = address.Address,
                                Name = address.Name
                            }
                        };


                        await Http.PostAsJsonAsync<Model.Contact>($"https://graph.microsoft.com/v1.0/me/contactfolders/{ParentFolderId}/contacts", contact);
                        contactscreated++;
                    
                    }
                    else
                    {
                        
                    }
                }
            }
            return contactscreated;
        }
    }

   
}
