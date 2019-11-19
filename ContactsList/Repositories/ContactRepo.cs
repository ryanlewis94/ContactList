using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using ContactsList.Helpers;
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.IO;

namespace ContactsList.Repositories
{
    public class ContactRepo : IContactRepo
    {
        ContactDb _context = new ContactDb();
        public EntityTagHeaderValue Etag;

        public async Task<List<Contact>> GetContacts()
        {
            //return _context.Contacts.ToList();

            using (var request = new HttpRequestMessage(HttpMethod.Get, ""))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                
                using (var response = await ApiHelper.ApiClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Etag = response.Headers.ETag;
                        Console.WriteLine(Etag);

                        var stream = await response.Content.ReadAsStreamAsync();
                        List<Contact> contacts = stream.ReadAndDeserializeFromJson<List<Contact>>();

                        return contacts;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        public async Task<Contact> AddContact(Contact contact)
        {
            //_context.Contacts.Add(contact);
            //_context.SaveChanges();
            //return contact;

            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(contact);
            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(HttpMethod.Post, ""))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var streamContent = new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await ApiHelper.ApiClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var stream = await response.Content.ReadAsStreamAsync();
                            var createdContact = stream.ReadAndDeserializeFromJson<Contact>();
                            return createdContact;
                        }
                        else
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                    }
                }
            }
        }

        public async Task<Contact> UpdateContact(Contact contact)
        {
            //_context.Contacts.First(c => c.Id == contact.Id);

            //_context.SaveChanges();
            //return contact;

            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(contact);
            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(HttpMethod.Put, ""))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.TryAddWithoutValidation("If-Match", $"{Etag}");

                using (var streamContent = new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await ApiHelper.ApiClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var stream = await response.Content.ReadAsStreamAsync();
                            var updatedContact = stream.ReadAndDeserializeFromJson<Contact>();
                            return updatedContact;
                        }
                        else
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                    }
                }
            }
        }

        public async Task DeleteContact(Contact contact)
        {
            //var contactToDelete = _context.Contacts.First(c => c.Id == contact.Id);
            //if (contactToDelete != null)
            //{
            //    _context.Contacts.Remove(contactToDelete);
            //}

            //_context.SaveChanges();
            //return contact;

            var serializedContactToDelete = JsonConvert.SerializeObject(contact);

            var request = new HttpRequestMessage(HttpMethod.Delete, "");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(serializedContactToDelete);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpResponseMessage response = await ApiHelper.ApiClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    //var content = await response.Content.ReadAsStringAsync();
                    return;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public Task DiscardChanges()
        {
            foreach (DbEntityEntry entry in _context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }

            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
