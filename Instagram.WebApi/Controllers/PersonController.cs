using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Instagram.WebApi.Controllers
{
    public class Person
    {
        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public List<Certificate> CertificateList { get; set; }
    }

    public class Certificate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime IssuedDate { get; set; }
    }

    public class PersonController : ApiController
    {
        static List<Person> persons = new List<Person>
        {
            new Person {
                Id = 0,
                First ="Hoang",
                Last ="Nguyen Le",
                CertificateList = new List<Certificate>()
                    {
                        new Certificate()
                        {
                            Id = 0, Name ="MCP", IssuedDate = DateTime.Now
                        },
                         new Certificate()
                        {
                            Id = 1, Name ="MCP", IssuedDate = DateTime.Now
                        },
                          new Certificate()
                        {
                            Id = 2, Name ="MCP", IssuedDate = DateTime.Now
                        }
                    }
            },
            new Person {
                Id = 1,
                First ="Hieu",
                Last ="Nguyen Le",
                CertificateList = new List<Certificate>()
                    {
                        new Certificate()
                        {
                            Id = 0, Name ="MCP", IssuedDate = DateTime.Now
                        }
                    }
            },
            new Person {
                Id = 1,
                First ="Quan",
                Last ="Nguyen Le",
                CertificateList = new List<Certificate>()
                    {
                        new Certificate()
                        {
                            Id = 0, Name ="MCP", IssuedDate = DateTime.Now
                        }
                    }
            }
        };
        // GET api/<controller>
        public IEnumerable<Person> Get()
        {
            return persons;
        }

        //[Route("api/person/getallpersons")]
        public HttpResponseMessage GetPersons()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("Hello api", Encoding.Unicode);
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { MaxAge = TimeSpan.FromMinutes(20) };
            return response;
            //return Request.CreateResponse(HttpStatusCode.OK, persons);
        }

        [Route("api/person/getpersonbyid/{id}")]
        public IHttpActionResult GetPerson(int id)
        {
            var person = persons.Where(p => p.Id == id).FirstOrDefault();
            if (persons == null)
            {
                NotFound();
            }
            return Ok(person);
        }

        // GET api/<controller>/5
        public Person Get(int id)
        {
            return persons.Find(p => p.Id == id);
        }

        // POST api/<controller>
        //public void Post([FromBody]Person person)
        //{
        //    persons.Add(person);
        //}

        [Route("api/person/postperson")]
        public HttpResponseMessage Post([FromBody]Person person)
        {
            var response = Request.CreateResponse(HttpStatusCode.Created, person);
            string uri = Url.Link("DefaultApi", new
            {
                id = person.Id
            });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Person person)
        {
            var oldperson = persons.Where(e => e.Id == id).FirstOrDefault();
            if (oldperson != null)
            {
                persons.Remove(oldperson);
                persons.Add(person);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var person = persons.Where(e => e.Id == id).FirstOrDefault();
            if (person != null)
            {
                persons.Remove(person);
            }
        }
    }
}