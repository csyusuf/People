using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleApi.Data;
using PeopleApi.Models;
using System.Web;

namespace PeopleApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly PeopleContext context;

        public PersonController(PeopleContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// This method gets all people information from DB.
        /// </summary>
        /// <returns>returns a list of person info</returns>
        [HttpGet]
        public IEnumerable<PersonInfo> GetAll()
        {
            try
            {
                return context.Persons.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Get person from DB for specified id.
        /// </summary>
        /// <param name="id">record id for db entry</param>
        /// <returns>returns PersonInfo object</returns>
        [HttpGet("{id}", Name = "GetPerson")]
        public IActionResult GetById(int id)
        {
            try
            {
                var person = context.Persons.FirstOrDefault(x => x.ID == id);
                if (person == null)
                {
                    return NotFound();
                }
                return new ObjectResult(person);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        /// <summary>
        /// Creates a new entry in Person db table for the specified person information.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///  
        ///     POST /Person
        ///         {
        ///             "FirstName" : "firstname",
        ///             "LastName" : "lastname",
        ///             "Gender" : "gender",
        ///             "Age" : 99
        ///         }
        /// 
        /// </remarks>
        /// <param name="person">PersonInfo object</param>
        /// <returns>returns PersonInfo object with assigned record id</returns>
        [HttpPost]
        public IActionResult Create([FromBody] PersonInfo person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest();
                }

                context.Persons.Add(person);
                context.SaveChanges();

                return CreatedAtRoute("GetPerson", new { id = person.ID }, person);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Updates person info in Person db table for specified person.
        /// </summary>
        /// <remarks>
        /// 
        ///     Sample request:
        ///     
        ///     POST /Person    
        ///         {
        ///             "ID" : 1
        ///             "FirstName" : "firstname",
        ///             "LastName" : "lastname",
        ///             "Gender" : "gender",
        ///             "Age" : 99
        ///         }
        /// </remarks>
        /// <param name="person">New person information to update related db record</param>
        /// <returns>if successed returns nothing.</returns>
        
        [HttpPut("{id}")]
        public IActionResult Update([FromBody]PersonInfo person)
        {
            try
            {
                if (person == null)
                {
                    return BadRequest();
                }

                var dbPerson = context.Persons.FirstOrDefault(p => p.ID == person.ID);

                if (dbPerson == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    dbPerson.FirstName = person.FirstName;
                    dbPerson.LastName = person.LastName;
                    dbPerson.Gender = person.Gender;
                    dbPerson.Age = person.Age;

                    context.SaveChanges();
                }

                return new NoContentResult();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// deletes a record for specified id. Id is a unique number so this method just delete one record from db if it exist.
        /// </summary>
        /// <param name="id">record id in db table</param>
        /// <returns>if successed returns nothing. it just returns for doesn't exist record</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var dbPerson = context.Persons.FirstOrDefault(p => p.ID == id);
                if (dbPerson == null)
                {
                    return NotFound();
                }

                context.Persons.Remove(dbPerson);
                context.SaveChanges();
                return new NoContentResult();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
