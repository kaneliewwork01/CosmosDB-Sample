using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CoreSQL_Sample.Models
{
    public class FamilyModel
    {
        // The item must have an Id property serialized as id in JSON.
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string LastName { get; set; }
        public ParentModel[] Parents { get; set; }
        public ChildModel[] Children { get; set; }
        public AddressModel Address { get; set; }
        public bool IsRegistered { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ParentModel
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
    }

    public class ChildModel
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public int Grade { get; set; }
        public PetModel[] Pets { get; set; }
    }

    public class PetModel
    {
        public string GivenName { get; set; }
    }

    public class AddressModel
    {
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
    }

    public class MonsterModel
    {
        // The item must have an Id property serialized as id in JSON.
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string GivenName { get; set; }
        public bool IsDanger { get; set; }
        public decimal AverageWeight { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string LastName { get; set; }
    }
}
