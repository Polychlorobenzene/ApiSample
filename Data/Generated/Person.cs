using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiSample.Data.Entities
{
    /// <summary>
    /// Represents a Person.
    /// NOTE1: This class is generated from a T4 template - you should not modify it manually.
	/// NOTE2: This table was sourced from the includeTables.ttinclude file at the solution root

    /// </summary>
    public partial class Person : BaseEntity<int>, IBaseEntity<int> 
    { 
		//Simple Properties
	
		[Required]
        public override int  Id 
		{
			get { return base.Id;}
			set { base.Id = value;}
		}
		
		[Required]
        [MaxLength(255)]
        public string FirstName { get; set; }
		
		[Required]
        [MaxLength(255)]
        public string LastName { get; set; }
		
		[MaxLength(255)]
        public string MiddleName { get; set; }
		
		//Navigation Properties 
		
		//Constructor
		public Person() : base()
		{
        			
		}
	}
}
